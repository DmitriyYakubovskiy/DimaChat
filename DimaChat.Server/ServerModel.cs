using System.Net.Sockets;
using System.Net;
using DimaChat.Client.Models;
using System.IO;
using System.Text;
using System.Reflection.PortableExecutable;

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

        public void Stop()
        {
            tcpListener.Stop(); 
            for(int i= 0; i < clients.Count; i++)
            {
                clients[i].Close();
            } 
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

        private void RemoveConnection(string id)
        {
            ClientModel client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Console.WriteLine($"{client.Name} disconnect!");
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
                ClientModel client = new ClientModel(tcpClient, new StreamReader(tcpClient.GetStream()));
                clients.Add(client);

                Thread clientThread = new Thread(() => ReceiveMessageAsync(clients[clients.Count-1])) { IsBackground = true };
                clientThread.Start();

                await Task.Delay(10);
                Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} Connect!"); ;
            }
        }

        private async Task ReceiveMessageAsync(ClientModel client)
        {
            try
            {
                while (true)
                {
                    string? message = await client.GetStreamReader().ReadLineAsync();
                    var messages=message.Split('\t');
                    if (string.IsNullOrEmpty(client.Name)) client.Name = messages[1];
                    if (string.IsNullOrEmpty(messages[2])) continue;
                    Console.WriteLine($"{client.Name} отправил сообщение");
                    await SendMessage(client, message);
                    await Task.Delay(10);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                RemoveConnection(client.Id);
            }
        }

        private async Task SendMessage(ClientModel client, string message)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i] != client)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    await clients[i].GetStream().WriteAsync(data, 0, data.Length);
                }
            }
        }
    }
}

