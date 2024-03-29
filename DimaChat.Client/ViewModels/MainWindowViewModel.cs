using DimaChat.Client.Commands;
using DimaChat.Client.Models;
using DimaChat.Client.Views;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Window window;
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
        public IReadOnlyCollection<ChatModel> Chats => chatCollection.Chats;

        private ChatModelsCollection chatCollection;



        public ICommand AddChatCommand=>addChatCommand;
        public ICommand OpenChatCommand => openChatCommand;
        private readonly Command openChatCommand;
        private readonly Command addChatCommand;
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel(Window window, ClientModel client)
        {
            this.client = client;
            this.window = window;
            openChatCommand = new DelegateCommand(_ => OpenChat());
            addChatCommand = new DelegateCommand(_ => AddChat());
            chatCollection= new ChatModelsCollection(); 
            chatCollection.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    OnPropertyChanged(nameof(Chats));
                }
            };
        }

        public void OpenChat()
        {
            var chatWindow = new ChatWindowView(window);
            chatWindow.DataContext = new ChatWindowViewModel(client);
            chatWindow.Show();
        }

        public void AddChat()
        {
            var addChatWindow = new AddChatView(window);
            addChatWindow.DataContext = new AddChatViewModel(window);
            addChatWindow.Show();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
