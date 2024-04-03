﻿using DimaChat.DataAccess.Collections;
using DimaChat.DataAccess.Entities;
using DimaChat.DataAccess.Models;
using DimaChat.Server.Managers;
using Microsoft.AspNetCore.SignalR;

namespace DimaChat.Server.Hubs;

public class ServerHub : Hub
{
    private List<ClientModel> clients;
    private List<ChatModel> chats;
    private DatabaseManager databaseManager;

    public ServerHub()
    {
        databaseManager = new DatabaseManager();
        clients = databaseManager.GetClientModels();
        chats = databaseManager.GetChatModels();
    }

    public async Task Send(MessageModel message)
    {
        Console.WriteLine(message);
        await this.Clients.All.SendAsync("Receive", message);
    }

    public async Task AuthorizeSend(string name, string password)
    {
        await Clients.Caller.SendAsync("AuthorizeReceive", LoginVerification(name, password));
    }

    public async Task RegistrationSend(string name, string password)
    {
        await Clients.Caller.SendAsync("RegistrationReceive", RegistrationVerification(name, password));
    }

    public async Task Join(MessageModel message)
    {
        try
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, message.ChatId.ToString());
            await Clients.OthersInGroup(message.ChatId.ToString()).SendAsync("JoinToChat", new MessageModelsCollection());
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", "You failed to join the chat!" + ex.Message);
        }
    }

    public async Task Leave(MessageModel message)
    {
        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, message.ChatId.ToString());
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", "You failed to leave from the chat!" + ex.Message);
        }
    }

    public async Task GetNewChat(string chatName, int clientId)
    {
        databaseManager.UpdateChats(new ChatModel { Name=chatName});
        databaseManager.UpdateChatClientEntities(new ChatClientEntity() { ClientId = clientId, ChatId = databaseManager.GetChatEntities().FirstOrDefault(x => x.ChatName == chatName).ChatId });
        chats = databaseManager.GetChatModels();
        await PushChats(clientId);
    }

    public async Task PushChats(int clientId)
    {
        var resultChats= new List<ChatModel>();
        var chatClient = databaseManager.GetChatClientEntities();
        for (int i = 0; i < chatClient.Count; i++)
        {
            if (chatClient[i].ClientId == clientId)
            {
                resultChats.Add(chats.FirstOrDefault(x => x.Id == chatClient[i].ChatId));
            }
        } 
        await Clients.Caller.SendAsync("ReceiveChats", resultChats);
    }

    private ClientModel LoginVerification(string name, string password)
    {
        var clients = databaseManager.GetClientModels();
        var client = clients.FirstOrDefault(x => x.Name == name);
        if (client == null) return null!;
        if(client.Password!=password) return null!;
        return client;
    }

    private bool RegistrationVerification(string name, string password)
    {
        Console.WriteLine(1);
        var clients = databaseManager.GetClientModels();
        var client = clients.FirstOrDefault(x => x.Name == name);
        if (client == null)
        {
            databaseManager.UpdateClients(new ClientModel() { Name = name, Password = password });
            return true;
        }
        return false;
    }
}