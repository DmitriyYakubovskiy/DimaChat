using DimaChat.DataAccess.Models;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DimaChat.DataAccess.Collections;

public class ChatModelsCollection : INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public readonly ObservableCollection<ChatModel> Chats;

    public ChatModelsCollection(List<ChatModel> chats)
    {
        Chats = new ObservableCollection<ChatModel>(chats);
        OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { Chats });
    }

    public void AddChat(ChatModel chat)
    {
        Chats.Add(chat);
        OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { chat });
    }

    public void DeleteMessage(ChatModel chat)
    {
        Chats.Remove(chat);
        OnPropertyChanged(NotifyCollectionChangedAction.Remove, new[] { chat });
    }

    public ChatModelsCollection() : this(new List<ChatModel>()) { }

    private void OnPropertyChanged(NotifyCollectionChangedAction action, IList changedItems)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));
    }

    private void OnPropertyChanged(NotifyCollectionChangedAction action)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
    }
}
