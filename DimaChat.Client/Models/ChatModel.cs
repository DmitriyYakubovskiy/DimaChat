using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimaChat.Client.Models;

public class ChatModel : INotifyPropertyChanged
{
    private int id;
    private string name;
    
    public int Id
    {
        get => id;
        set
        {
            id = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
