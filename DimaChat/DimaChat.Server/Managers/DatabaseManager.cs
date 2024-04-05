using AutoMapper;
using DimaChat.DataAccess.Entities;
using DimaChat.DataAccess.Mappers;
using DimaChat.DataAccess.Models;
using DimaChat.Server.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DimaChat.Server.Managers;

public class DatabaseManager
{
    private List<ClientModel> clients;

    private readonly IConfiguration configuration;
    private readonly IMapper clientMapper;
    private readonly IMapper messageMapper;

    public DatabaseManager()
    {
        var clientConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingClient()));
        var chatCongih = new MapperConfiguration(cfg => cfg.AddProfile(new MappingMessage()));
        clientMapper = new Mapper(clientConfig);
        messageMapper = new Mapper(chatCongih);
        configuration = BuildConfiguration();
    }
    public List<ClientEntity> GetClientEntities()
    {
        return new DimaChatDbContext(configuration).Clients.ToList();
    }

    public List<ChatEntity> GetChatEntities()
    {
        return new DimaChatDbContext(configuration).Chats.ToList();
    }

    public List<ChatClientEntity> GetChatClientEntities()
    {
        return new DimaChatDbContext(configuration).ChatClients.ToList();
    }

    public List<ClientModel> GetClientModels()
    {
        var context = new DimaChatDbContext(configuration);
        var items = new List<ClientModel>();
        foreach (var client in context.Clients)
        {
            items.Add(clientMapper.Map<ClientModel>(client));
        }
        clients = new List<ClientModel>(items);
        return items;
    }

    public List<ChatModel> GetChatModels()
    {
        var context = new DimaChatDbContext(configuration);
        var items = new List<ChatModel>();
        //var clients = GetChatModels();
        foreach (var chat in context.Chats.Include(x=>x.Clients).Include(x=>x.Messages))
        {
            var clientModels=new List<ClientModel>();
            foreach (var chatClientEntity in chat.Clients)
            {
                var clientEntity = clients.FirstOrDefault(x => x.Id == chatClientEntity.ClientId);
                clientModels.Add(clientMapper.Map<ClientModel>(clientEntity));
            }
            var messageModels = new List<MessageModel>();
            foreach (var messageEntity in chat.Messages)
            {
                messageModels.Add(messageMapper.Map<MessageModel>(messageEntity));
            }
            items.Add(new ChatModel(chat.ChatId, chat.ChatName, clientModels, messageModels));
        }
        return items;
    }

    public void UpdateClients(ClientModel clientModel)
    {
        using (var context = new DimaChatDbContext(configuration))
        {
            bool check = true;
            foreach (var client in context.Clients)
            {
                if (clientModel.Name == client.ClientName)
                {
                    check = false;
                    break;
                }
            }
            if (check) context.Clients.Add(new ClientEntity { ClientName = clientModel.Name, ClientPassword = clientModel.Password });

            context.SaveChanges();
        }
    }

    public void UpdateChats(ChatModel chatEntity)
    {
        using (var context = new DimaChatDbContext(configuration))
        {
            bool check = false;
            foreach (var chat in context.Clients)
            {
                if (chatEntity.Name == chat.ClientName)
                {
                    check = true;
                    break;
                }
            }
            if (check) return; 
            context.Chats.Add(new ChatEntity() { ChatName=chatEntity.Name});
            context.SaveChanges();
        }
    }

    public void UpdateChatClientEntities(ChatClientEntity chatClientEntity)
    {
        using (var context = new DimaChatDbContext(configuration))
        {
            context.ChatClients.Add(chatClientEntity);
            context.SaveChanges();
        }
    }

    public List<MessageEntity> GetMessages()
    {
        throw new NotImplementedException();
    }

    private IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }
}
