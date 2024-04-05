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

public class MainWindowViewModel : INotifyPropertyChanged
{
    public IReadOnlyCollection<ChatModel> Chats => chatCollection.Chats;
    public ICommand AddChatCommand => addChatCommand;
    public ICommand OpenChatCommand => openChatCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private ClientModel client;
    public ClientModel Client
    {
        get => client;
        set
        {
            client = value;
            OnPropertyChanged();
        }
    }

    private ChatModelsCollection chatCollection;
    private DimaChatService signal;
    private Window window;
    private readonly Command openChatCommand;
    private readonly Command addChatCommand;

    public MainWindowViewModel(Window window, ClientModel client, DimaChatService signal)
    {
        this.client = client;
        this.window = window;
        this.signal = signal;
        openChatCommand = new GenericCommand<ChatModel>(OpenChat);
        addChatCommand = new DelegateCommand(_ => AddChat());
        chatCollection = new ChatModelsCollection();
        chatCollection.CollectionChanged += (_, e) =>
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                OnPropertyChanged(nameof(Chats));
            }
        };
        signal.ReceiveChats();
        signal.RecieveError();
        signal.ChatsArrived += AddChats;
        Task.Run(() => signal.SendChatsRequest(client.Id));
        window.Closing += WindowClosing;
    }

    private void OpenChat(ChatModel chatModel)
    {
        var chatWindow = new ChatWindowView(window);
        chatWindow.DataContext = new ChatWindowViewModel(chatWindow ,client, signal, chatModel.Name, chatModel.Id);
        if (chatWindow.ShowDialog() != true) return;
    }

    private async Task AddChat()
    {
        var addChatWindow = new AddChatView(window);
        addChatWindow.DataContext = new AddChatViewModel(addChatWindow, signal, client.Id);
        if (addChatWindow.ShowDialog() != true) return;
        await signal.SendChatsRequest(client.Id);
    }

    private void AddChats(List<ChatModel> chats)
    {
        chatCollection = new ChatModelsCollection(chats);
        OnPropertyChanged(nameof(Chats));
    }

    private void WindowClosing(object sender, CancelEventArgs e)
    {
        signal.ChatsArrived-= AddChats;
        window.DialogResult = true;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
