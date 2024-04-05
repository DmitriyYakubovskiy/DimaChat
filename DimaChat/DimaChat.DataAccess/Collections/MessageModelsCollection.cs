using DimaChat.DataAccess.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DimaChat.DataAccess.Collections;

public class MessageModelsCollection : INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public readonly ObservableCollection<MessageModel> Messages;

    public MessageModelsCollection(List<MessageModel> messages)
    {
        Messages = new ObservableCollection<MessageModel>(messages);
        OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { Messages });
    }
    public MessageModelsCollection() : this(new List<MessageModel>()) { }

    public void AddMessage(MessageModel message)
    {
        Messages.Add(message);
        OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { message });
    }

    public void DeleteMessage(MessageModel message)
    {
        Messages.Remove(message);
        OnPropertyChanged(NotifyCollectionChangedAction.Remove, new[] { message });
    }

    private void OnPropertyChanged(NotifyCollectionChangedAction action, IList changedItems)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));
    }
}
