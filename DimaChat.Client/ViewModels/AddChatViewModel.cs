using DimaChat.Client.Commands;
using DimaChat.Client.Enums;
using DimaChat.Client.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;

public class AddChatViewModel
{
    private ChatModel chatModel;
    public ChatModel ChatModel
    {
        get => chatModel;
        set
        {
            chatModel = value;
        }
    }
    private Window window;
    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand OkCommand => okCommand;
    public ICommand CancelCommand => cancelCommand;

    private Command okCommand;
    private Command cancelCommand;

    public AddChatViewModel(Window window)
    {
        this.window = window;
        chatModel=new ChatModel();
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
    }

    private void Ok()
    {
        ClientModel client=new ClientModel();
        client.Connect();
        client.SendMessage((int)ServerCommands.AddChat, chatModel.Name);
        window.Close();
    }

    private void Cancel()
    {
        window.Close(); 
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
