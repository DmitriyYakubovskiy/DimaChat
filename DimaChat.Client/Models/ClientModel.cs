using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace DimaChat.Client.Models
{
    public class ClientModel:ICloneable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "ada";

        private TcpClient tcpClient;

        public ClientModel(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
        }

        public ClientModel() : this(new TcpClient()) { }

        public async Task Connect()
        {
            try
            {
                await tcpClient.ConnectAsync("127.0.0.1", 8080);
                await GetMessages();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task GetMessages()
        {
            while (true)
            {
                var sr = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);
                var message = await sr.ReadLineAsync() ?? String.Empty;
                if (message == "EXIT") break;
                //Dispatcher.Invoke(() => MessageBox.Show(message));
                MessageBox.Show(message);
                await Task.Delay(10);
            }
            Close();
        }

        public void SendMessage(int index, string message)
        {
            message = $"{index}\t{message}";
            var writer = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
            writer.WriteLine(message);
            writer.Flush();
            writer.Close();
        }

        public NetworkStream GetStream()
        {
            return tcpClient.GetStream();
        }

        public void Close()
        {
            tcpClient.Close();
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
