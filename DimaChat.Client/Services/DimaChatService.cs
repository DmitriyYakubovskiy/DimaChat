using DimaChat.DataAccess.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Windows;

namespace DimaChat.Client.Services;

public class DimaChatService
{
    public event Action<List<ChatModel>> ChatsArrived;
    public event Action<ClientModel> AuthorizationResponseArrived;
    public event Action<bool?> RegistartionResponseArrived;

    private readonly HubConnection connection;
    private ClientModel client;

    public DimaChatService(HubConnection connection, ClientModel clientModel)
    {
        this.connection = connection;
        client = clientModel;
    }

    public DimaChatService(HubConnection connection) :this(connection,new ClientModel()) { }

    public async Task ConnectAsync()
    {
        await connection.StartAsync();
    }

    public void ReceiveMessages()
    {
        connection.On<MessageModel>("Receive",  (message)=>
        {
            //App.Current.Dispatcher.Invoke(delegate
            //{
            //    chat.Add(message);
            //});
        });
    }

    public void ReceiveAuthorizeMessage()
    {
        connection.On<ClientModel>("AuthorizeReceive", (client) =>
        {
            AuthorizationResponseArrived?.Invoke(client);
        });
    }

    public void ReceiveRegistrationMessage()
    {
        connection.On<bool>("RegistrationReceive", (isRegistered) =>
        {
            RegistartionResponseArrived?.Invoke(isRegistered);
        });
    }

    public void ReceiveChats()
    {
        connection.On<List<ChatModel>>("ReceiveChats", (chats) =>
        {
            ChatsArrived?.Invoke(chats);
        });
    }

    public async Task PushNewChat(string chatName, int clientId)
    {
        await connection.SendAsync("GetNewChat", chatName, clientId);
    }

    public async Task SendChatsRequest(int chatId)
    {
        await connection.SendAsync("PushChats", chatId);
    }

    public async Task SendMessage(MessageModel message)
    {
        await connection.SendAsync("Send", message);
    }

    public async Task SendAuthorizeMessage(string name, string password)
    {
        await connection.SendAsync("AuthorizeSend", name, password);
    }

    public async Task SendRegistrationMessage(string name, string password)
    {
        await connection.SendAsync("RegistrationSend", name, password);
    }
}
