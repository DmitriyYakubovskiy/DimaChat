using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimaChat.Client.Models
{
    public class MessageModel : INotifyPropertyChanged, ICloneable
    {
        public string Name
        {
            get => name;
            set
            {
                name= value;
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get=> content;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public int AddresseeId
        {
            get => addresseeId;
            set
            {
                addresseeId = value;
                OnPropertyChanged();
            }
        }
        private string name;
        private string content;
        private int addresseeId;

        public MessageModel(string name, string content, int addresseeId)
        {
            this.name = name;
            this.content = content;
            this.addresseeId = addresseeId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
