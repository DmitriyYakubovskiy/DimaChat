using System.Net.Sockets;
using System.Net;
using DimaChat.Client.Models;
using System.Text;
using DimaChat.Server.Managers;
using DimaChat.Client.Enums;
using System.Xml.Linq;
using Windows.Media.Protection.PlayReady;

namespace DimaChat.Server.Models;

public class ServerModel
{
    private string ip;
    private int port;
    private DatabaseManager databaseManager;
    TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
    List<ClientModel> clients = new List<ClientModel>();
    List<ChatModel> chats = new List<ChatModel>();

    public ServerModel(string ip, int port)
    {
        databaseManager = new DatabaseManager();
        clients=databaseManager.GetClients();
        chats=databaseManager.GetChats();
        this.ip = ip;
        this.port = port;
    }

    public void Stop()
    {
        tcpListener.Stop();
        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].Close();
        }
    }

    public void Start()
    {
        //try
        //{
            tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            var threadGetMessages = new Thread(ListenAsync) { IsBackground = true };
            threadGetMessages.Start();
        //}
        //catch (SocketException ex)
        //{
        //    Console.WriteLine($"Socket exception: {ex.Message}");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Exception: {ex.Message}");
        //}
    }

    private void RemoveConnection(string name)
    {
        //ClientModel client = clients.FirstOrDefault(c => c.Name == name);
        //if (client != null)
        //{
        //    Console.WriteLine($"{client.Name} disconnect!");
        //    client.Close();
        //}
    }

    private async void ListenAsync()
    {
        //try
        //{
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                ClientModel client = new ClientModel(tcpClient, new StreamReader(tcpClient.GetStream()));
                clients.Add(client);

                Thread clientThread = new Thread(() => ReceiveMessageAsync(clients[clients.Count - 1])) { IsBackground = true };
                clientThread.Start();

                await Task.Delay(10);
                Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} Connect!"); ;
            }
        //}
        //catch (SocketException ex)
        //{
        //    Console.WriteLine($"Socket exception: {ex.Message}");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Exception: {ex.Message}");
        //}
    }

    private async Task ReceiveMessageAsync(ClientModel client)
    {
        //try
        //{
            while (true)
            {
                MessageModel messageModel;
                string? message = await client.GetStreamReader().ReadLineAsync();
            if(String.IsNullOrEmpty(message)) continue;
                string[] infoMessage = message.Split('\t');

                if (infoMessage.Length == 3) messageModel = new MessageModel(infoMessage[1], infoMessage[2], Convert.ToInt32(infoMessage[0]));
                else continue;
            if (messageModel.AddresseeId == (int)ServerCommands.AddChat) databaseManager.UpdateChats(messageModel.Content);
                if(messageModel.AddresseeId == (int)ServerCommands.Disconnect)
                {
                    RemoveConnection(messageModel.Name);
                    break;
                }
                if (messageModel.AddresseeId == (int)ServerCommands.VerificationData)
                {
                    await LoginVerification(messageModel,client);
                    continue;
                }
                if (messageModel.AddresseeId == (int)ServerCommands.Registration)
                {
                    if (RegistrationVerification(messageModel.Name, messageModel.Content)) await SendMessage(new MessageModel("server", "Ok", (int)ServerCommands.RegistrationResult), client);
                    else await SendMessage(new MessageModel("server", "Cancel", (int)ServerCommands.RegistrationResult), client);
                    continue;
                }
                if (string.IsNullOrEmpty(messageModel.Content)) continue;
                Console.WriteLine($"{client.Name} отправил сообщение");
                await SendMessage(client, messageModel);
                await Task.Delay(10);
            }
        //}
        //catch (SocketException ex)
        //{
        //    Console.WriteLine($"Socket exception: {ex.Message}");
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Exception: {ex.Message}");
        //}
    }

    private async Task LoginVerification(MessageModel messageModel, ClientModel client)
    {
        if (LoginVerification(messageModel.Name, messageModel.Content))
        {
            var lastClient = clients.FirstOrDefault(x => x.Name == client.Name);
            client.Id = lastClient.Id;
            clients.Remove(lastClient);
            clients.Add(client);
            await SendMessage(new MessageModel("server", "Ok", (int)ServerCommands.VerificationResult), client);
        }
        else
        {
            await SendMessage(new MessageModel("server", "Cancel", (int)ServerCommands.VerificationResult), client);
        }
    }

    private bool LoginVerification(string name, string password)
    {
        var client = clients.FirstOrDefault(x => x.Name == name);
        if (client == null) return false;
        return client.Password == password;
    }

    private bool RegistrationVerification(string name, string password)
    {
        var client = clients.FirstOrDefault(x => x.Name == name);
        if (client == null)
        {
            databaseManager.UpdateClients(new ClientModel() { Name = name, Password = password });
            clients.Add(new ClientModel() { Name = name, Password=password });
            return true;
        }
        if (client.Password?.Length > 3) return true;
        return false;
    }

    private async Task SendMessage(MessageModel message, ClientModel addressee)
    {
        byte[] data = Encoding.Unicode.GetBytes(message.ToString());
        await addressee.GetStream().WriteAsync(data, 0, data.Length);
    }

    private async Task Broadcast(ClientModel client, MessageModel message)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i] != client)
            {
                byte[] data = Encoding.Unicode.GetBytes(message.ToString());
                await clients[i].GetStream().WriteAsync(data, 0, data.Length);
            }
        }
    }

    private async Task SendMessage(ClientModel client, MessageModel message)
    {
        var chat=chats.FirstOrDefault(x=>x.Id == message.AddresseeId);
        for (int i = 0; i < chat.Clients.Count; i++)
        {
            if (chat.Clients[i].Id != client.Id)
            {
                byte[] data = Encoding.Unicode.GetBytes(message.ToString());
                var clientReceiver = clients.FirstOrDefault(x => x.Id == chat.Clients[i].Id);
                await clientReceiver.GetStream().WriteAsync(data, 0, data.Length);
            }
        }
    }
}

