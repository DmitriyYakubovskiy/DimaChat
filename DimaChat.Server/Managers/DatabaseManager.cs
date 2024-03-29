using DimaChat.Client.Models;
using DimaChat.Server.DataAccess.Contexts;
using DimaChat.Server.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DimaChat.Server.Managers;

public class DatabaseManager
{
    private readonly IConfiguration configuration;

    public DatabaseManager()
    {
        configuration = BuildConfiguration();
    }

    public List<ClientModel> GetClients()
    {
        var context = new DimaChatDbContext(configuration);
        var items = new List<ClientModel>();
        foreach (var client in context.Clients)
        {
            items.Add(new ClientModel
            {
                Id = client.Id,
                Name = client.Name,
                Password = client.Password,
            });
        }
        return items;
    }

    public List<Server.Models.ChatModel> GetChats()
    {
        var context = new DimaChatDbContext(configuration);
        var items = new List<Server.Models.ChatModel>();
        foreach (var chat in context.Chats.Include(x=>x.Clients))
        {
            items.Add(new Server.Models.ChatModel(chat.Clients.ToList())
            {
                Id = chat.Id,
                Name = chat.Name,
            });
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
                if (clientModel.Name == client.Name)
                {
                    check = false;
                    break;
                }
            }
            if (check) context.Clients.Add(new ClientEntity { Name = clientModel.Name, Password = clientModel.Password });

            context.SaveChanges();
        }
    }

    public void UpdateChats(string chatName)
    {
        using (var context = new DimaChatDbContext(configuration))
        {
            bool check = true;
            foreach (var chat in context.Clients)
            {
                if (chatName == chat.Name)
                {
                    check = false;
                    break;
                }
            }
            if (check) context.Chats.Add(new ChatEntity { Name = chatName });

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
