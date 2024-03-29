using DimaChat.Client.Commands;
using DimaChat.Client.Models;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels
{
    public class ChatWindowViewModel : INotifyPropertyChanged
    {
        private int index = 1;
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
        public IReadOnlyCollection<MessageModel> Messages => messageCollection.Messages;

        private MessageModelsCollection messageCollection;
        private string message="";
        private readonly Command sendMessageCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ChatWindowViewModel(ClientModel client)
        {
            messageCollection = client.GetMessageModelsCollection();
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

        public void SendMessage()
        {
            client.SendMessage(index,message);
            message = String.Empty;
            OnPropertyChanged(nameof(Message));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
