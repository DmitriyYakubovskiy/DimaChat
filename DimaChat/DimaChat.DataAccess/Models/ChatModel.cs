using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimaChat.DataAccess.Models;

public class ChatModel : INotifyPropertyChanged
{
    public List<ClientModel> Clients => clients;
    public List<MessageModel> Messages => messages;
    public event PropertyChangedEventHandler? PropertyChanged;

    private int id;
    public int Id
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    private List<ClientModel> clients;
    private List<MessageModel> messages;

    public ChatModel(int id, string name, List<ClientModel> clients, List<MessageModel> messages)
    {
        Id = id;
        Name = name;
        this.clients = clients;
        this.messages = messages;
    }
    public ChatModel(string name, List<ClientModel> clients, List<MessageModel> messages) : this(0, name, clients, messages) { }
    public ChatModel() : this(0, "", new List<ClientModel>(), new List<MessageModel>()) { }

    public void AddClient(ClientModel client)
    {
        clients.Add(client);
    }

    public void AddMessage(MessageModel message)
    {
        messages.Add(message);
    }

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
