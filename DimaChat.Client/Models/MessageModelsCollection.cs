﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DimaChat.Client.Models;

public class MessageModelsCollection : INotifyCollectionChanged
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public readonly ObservableCollection<MessageModel> Messages;

    public MessageModelsCollection(List<MessageModel> messages)
    {
        Messages = new ObservableCollection<MessageModel>(messages);
        OnPropertyChanged(NotifyCollectionChangedAction.Add, new[] { Messages });
    }

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

    public MessageModelsCollection() : this(new List<MessageModel>()) { }

    private void OnPropertyChanged(NotifyCollectionChangedAction action, IList changedItems)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItems));
    }

    private void OnPropertyChanged(NotifyCollectionChangedAction action)
    {
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
    }
}