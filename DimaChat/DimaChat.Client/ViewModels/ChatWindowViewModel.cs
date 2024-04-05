using DimaChat.Client.Commands;
using DimaChat.Client.Services;
using DimaChat.Client.Views;
using DimaChat.DataAccess.Collections;
using DimaChat.DataAccess.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;

public class ChatWindowViewModel : INotifyPropertyChanged
{
    public ICommand SendMessageCommand => sendMessageCommand;
    public ICommand AddClientCommand => addClientCommand;
    public IReadOnlyCollection<MessageModel> Messages => messageCollection.Messages;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string chatName = "";
    public string ChatName
    {
        get => chatName;
        set
        {
            chatName = value;
            OnPropertyChanged(nameof(chatName));
        }
    }

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

    private Window window;
    private Command sendMessageCommand;
    private Command addClientCommand;
    private MessageModelsCollection messageCollection;
    private DimaChatService signal;
    private ClientModel client;
    private int chatId;

    public ChatWindowViewModel(Window window, ClientModel client, DimaChatService signal, string chatName, int chatId)
    {
        this.window = window;
        this.client = client;
        this.signal = signal;
        this.chatName = $"Chat name: {chatName}";
        this.chatId = chatId;
        signal.ReceiveMessages();
        signal.MessageArrived += AddMessage;
        Task.Run(() => signal.JoinToChat(chatId));
        messageCollection = new MessageModelsCollection();
        messageCollection.CollectionChanged += (_, e) =>
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(Messages));
            }
        };
        sendMessageCommand = new DelegateCommand(_ => Task.Run(()=>SendMessage()));
        addClientCommand = new DelegateCommand(_ => AddClient());
        this.window.Closing += OnWindowClosing;
    }

    private void AddMessage(MessageModel messageModel)
    {
        App.Current.Dispatcher.BeginInvoke(new Action(() => { messageCollection.AddMessage(messageModel); }));
        OnPropertyChanged(nameof(Messages));
    }

    private async Task SendMessage()
    {
        if (message == String.Empty) return;
        var messageModel = new MessageModel(client.Name, message, chatId);
        await signal.SendMessage(messageModel);
        await App.Current.Dispatcher.BeginInvoke(new Action(() => { messageCollection.AddMessage(messageModel); }));
        message = String.Empty;
        OnPropertyChanged(nameof(Message));
    }

    private void AddClient()
    {
        var addClientWindow = new AddClientView(window);
        addClientWindow.DataContext = new AddClientViewModel(addClientWindow, signal, chatId);
        if (addClientWindow.ShowDialog() != true) return;
    }

    private void OnWindowClosing(object sender, CancelEventArgs e)
    {
        Task.Run(()=>signal.LeaveFromChat(chatId));
        signal.RemoveRecieveMessage();
        signal.MessageArrived -= AddMessage;
        window.DialogResult = true;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
