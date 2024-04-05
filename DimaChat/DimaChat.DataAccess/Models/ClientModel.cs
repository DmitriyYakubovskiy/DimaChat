using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimaChat.DataAccess.Models;

public class ClientModel : INotifyPropertyChanged, ICloneable
{
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

    private string password;
    public string Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged();
        }
    }

    public ClientModel(int id, string name, string password)
    {
        Id = id;
        Name = name;
        Password = password;
    }
    public ClientModel(string name, string password) : this(0, name, password) { }
    public ClientModel() : this(0, "", "") { }

    public object Clone()
    {
        return new ClientModel
        {
            Id = Id,
            Name = Name,
            Password = password,
        };
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
