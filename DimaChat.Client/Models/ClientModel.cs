using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace DimaChat.Client.Models
{
    public class ClientModel:ICloneable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = String.Empty;

        private TcpClient tcpClient;
        private StreamReader reader;
        private StreamWriter writer;
        private MessagesCollectionModel messagesCollection;

        public ClientModel(TcpClient tcpClient, StreamReader reader)
        {
            messagesCollection = new MessagesCollectionModel();
            this.tcpClient = tcpClient;
            this.reader = reader;
        }

        public ClientModel(TcpClient tcpClient) : this(tcpClient, null!) { }

        public ClientModel() : this(new TcpClient()) { }

        public ClientModel(MessagesCollectionModel messagesCollection)
        {
            this.messagesCollection = messagesCollection;
            tcpClient = new TcpClient();
        }

        public void Connect()
        {
            try
            {
                tcpClient.Connect("127.0.0.1", 8080);
                reader = new StreamReader(tcpClient.GetStream(),Encoding.Unicode);
                writer = new StreamWriter(tcpClient.GetStream(), Encoding.Unicode);
                var thread = new Thread(() => ReceiveMessageAsync()) { IsBackground = true };
                thread.Start();
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

        private async Task ReceiveMessageAsync()
        {
            try
            {
                var stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                    if (string.IsNullOrEmpty(message)) continue;
                    if(message=="EXIT") break;
                    var infoMessage=message.Split('\t');
                    MessageModel messageModel = new MessageModel(infoMessage[1], infoMessage[2], Convert.ToInt32(infoMessage[0]));
                    App.Current.Dispatcher.Invoke((Action)delegate
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
            messagesCollection.AddMessage(new MessageModel(Name,message,index));
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
            tcpClient.Close();
            reader.Close();
            writer.Close();
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
    }
}
