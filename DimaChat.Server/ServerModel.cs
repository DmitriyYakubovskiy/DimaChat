using System.Net.Sockets;
using System.Net;
using DimaChat.Client.Models;
using System.Text;
using System.IO;
using Windows.Media.Protection.PlayReady;

namespace DimaChat.Server
{
    public class ServerModel
    {
        private string ip;
        private int port;
        TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
        List<ClientModel> clients = new List<ClientModel>();
        List<ChatModel> chatModels = new List<ChatModel>();

        public ServerModel(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void Start()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Parse(ip), port);
                var threadGetMessages = new Thread(ListenAsync) { IsBackground = true };
                threadGetMessages.Start();
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

        protected internal void RemoveConnection(string id)
        {
            ClientModel client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                clients.Remove(client);
                client.Close();
            }
        }

        private async void ListenAsync()
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                ClientModel clientObject = new ClientModel(tcpClient);
                clients.Add(clientObject);
                Console.WriteLine("Connect!");
                await clientObject.GetMessages();
            }
        }

        private void SendMessage(TcpClient tcpClient, string message)
        {
            var writer = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8);
            writer.WriteLine(message);
            writer.Flush();
            writer.Close();
        }
    }
}

