using DimaChat.Client.Commands;
using DimaChat.Client.Services;
using DimaChat.Client.Views;
using DimaChat.DataAccess.Collections;
using DimaChat.DataAccess.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;

public class ChatWindowViewModel : INotifyPropertyChanged
{
    public ICommand SendMessageCommand => sendMessageCommand;
    public IReadOnlyCollection<MessageModel> Messages => messageCollection.Messages;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string message = "";
    public string Message
    {
        get => message;
        set
        {
            message = value;
            OnPropertyChanged(nameof(Message));
        }
    }

    private Command sendMessageCommand;
    private MessageModelsCollection messageCollection;
    private DimaChatService signal;
    private ClientModel client;
    private int index = 1;

    public ChatWindowViewModel(ClientModel client)
    {
        HubConnection connection = App.HubConnectionConfiguration();

        messageCollection = new MessageModelsCollection();
        signal = new DimaChatService(connection,client);
        signal.ConnectAsync();
        signal.ReceiveMessages();
        //messageCollection = client.GetMessageModelsCollection();
        messageCollection.CollectionChanged += (_, e) =>
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(Messages));
            }
        };
        this.client = client;
        sendMessageCommand = new DelegateCommand(_ => SendMessage());
    }

    public async void SendMessage()
    {
        var messageModel = new MessageModel(client.Name, message, index);
        await signal.SendMessage(messageModel);
        message = String.Empty;
        OnPropertyChanged(nameof(Message));
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
