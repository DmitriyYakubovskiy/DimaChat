using DimaChat.Client.Commands;
using DimaChat.Client.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels
{
    public class ChatWindowViewModel : INotifyPropertyChanged
    {
        private int index = 0;
        private ClientModel client;

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand SendMessageCommand => sendMessageCommand;
        public IReadOnlyCollection<MessageModel> Messages => messagesCollection.Messages;

        private MessagesCollectionModel messagesCollection;
        private string message="";
        private readonly Command sendMessageCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatWindowViewModel(string name, string ip, int port)
        {
            messagesCollection = new MessagesCollectionModel();
            messagesCollection.CollectionChanged += (_, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    OnPropertyChanged(nameof(Messages));
                }
            };
            client = new ClientModel(messagesCollection);
            client.Name = name;
            client?.Connect();
            sendMessageCommand = new DelegateCommand(_ => SendMessage());
        }

        public void SendMessage()
        {
            client.SendMessage(index,message);
            message = "";
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
