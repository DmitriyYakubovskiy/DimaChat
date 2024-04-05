using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DimaChat.DataAccess.Models;

public class MessageModel : INotifyPropertyChanged, ICloneable
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

    private string content;
    public string Content
    {
        get => content;
        set
        {
            content = value;
            OnPropertyChanged();
        }
    }

    private int chatId;
    public int ChatId
    {
        get => chatId;
        set
        {
            chatId = value;
            OnPropertyChanged();
        }
    }

    public MessageModel(int id, string name, string content, int chatId)
    {
        this.Id = id;
        this.name = name;
        this.content = content;
        this.chatId = chatId;
    }
    public MessageModel(string name, string content, int chatId) : this(0, name, content, chatId) { }
    public MessageModel() : this(0, "", "", 0) { }

    public override string ToString()
    {
        return $"{chatId}\t{name}\t{content}";
    }

    public object Clone()
    {
        throw new NotImplementedException();
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
