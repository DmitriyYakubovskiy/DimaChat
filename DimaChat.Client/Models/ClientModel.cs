using DimaChat.Client.Enums;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace DimaChat.Client.Models
{
    public class ClientModel : INotifyPropertyChanged, ICloneable
    {
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private string password;
        private int id;
        private string name;
        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        private MessageModelsCollection messagesCollection;

        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ClientModel(TcpClient tcpClient, StreamReader reader)
        {
            messagesCollection = new MessageModelsCollection();
            this.tcpClient = tcpClient;
            this.reader = reader;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        public ClientModel(TcpClient tcpClient) : this(tcpClient, null!) { }

        public ClientModel() : this(new TcpClient()) { }

        public ClientModel(MessageModelsCollection messagesCollection)
        {
            this.messagesCollection = messagesCollection;
            tcpClient = new TcpClient();
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        public async void Connect()
        {
            //try
            //{
                tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 8080);
                reader = new StreamReader(tcpClient.GetStream(), Encoding.Unicode);
                writer = new StreamWriter(tcpClient.GetStream(), Encoding.Unicode);
            var task = new Task(async () => { await ReceiveMessageAsync(); }, token);
                task.Start();
            //}
            //catch (SocketException ex)
            //{
            //    MessageBox.Show($"Socket exception: {ex.Message}");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Exception: {ex.Message}");
            //}
        }

        private async Task ReceiveMessageAsync()
        {
            try
            {
                var stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    if (!tcpClient.Connected) break;
                    MessageModel messageModel;
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    string[] infoMessage = message.Split('\t');

                    if (message == "EXIT") break;
                    if (string.IsNullOrEmpty(infoMessage[1])) continue;
                    if (infoMessage.Length != 3) continue;
                    messageModel = new MessageModel(infoMessage[1], infoMessage[2], Convert.ToInt32(infoMessage[0]));
                    App.Current.Dispatcher.Invoke(delegate
                    {
                            messagesCollection.AddMessage(messageModel);
                    });
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Socket exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}");
            }
        }

        public void SendMessage(int index, string message)
        {
            string messageInfo = $"{index}\t{Name}\t{message}";
            writer.WriteLine(messageInfo);
            writer.Flush();
            messagesCollection.AddMessage(new MessageModel(Name, message, index));
        }

        public MessageModelsCollection GetMessageModelsCollection()
        {
            return messagesCollection;
        }

        public StreamReader GetStreamReader()
        {
            return reader;
        }

        public NetworkStream GetStream()
        {
            return tcpClient.GetStream();
        }

        public void Close()
        {
            if (tcpClient.Connected)
            {
                tcpClient.Client.Disconnect(false);
                tcpClient?.Close();
            }
            reader?.Close();
            writer?.Close();
            cancelTokenSource?.Cancel();
            cancelTokenSource?.Dispose();
        }

        public object Clone()
        {
            return new ClientModel
            {
                Id = Id,
                Name = Name,
                tcpClient = tcpClient,
            };
        }

        public int GetCountMessages()
        {
            return messagesCollection.Messages.Count;
        }

        public MessageModel GetLastMessage()
        {
            return messagesCollection.Messages[messagesCollection.Messages.Count-1];
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
