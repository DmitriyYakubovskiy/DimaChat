using DimaChat.Client.Commands;
using DimaChat.Client.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;

public class AddChatViewModel: INotifyPropertyChanged
{
    public ICommand OkCommand => okCommand;
    public ICommand CancelCommand => cancelCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string chatName = "";
    public string ChatName
    {
        get => chatName;
        set
        {
            chatName = value;
            OnPropertyChanged(nameof(ChatName));
        }
    }

    private DimaChatService signal;
    private Window window;
    private Command okCommand;
    private Command cancelCommand;
    private int clientId;

    public AddChatViewModel(Window window, DimaChatService signal, int clientId)
    {
        this.window = window;
        this.signal = signal;
        this.clientId = clientId;
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
    }

    private async void Ok()
    {
        if (chatName.Length < 1)
        {
            MessageBox.Show("Пустое поле ввода!");
            return;
        }
        await signal.PushNewChat(chatName, clientId);
        window.DialogResult = true;
        window.Close();
    }

    private void Cancel()
    {
        window.DialogResult = false;
        window.Close(); 
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
