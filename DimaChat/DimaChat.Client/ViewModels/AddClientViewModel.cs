using DimaChat.Client.Commands;
using DimaChat.Client.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace DimaChat.Client.ViewModels;

public class AddClientViewModel : INotifyPropertyChanged
{
    public ICommand OkCommand => okCommand;
    public ICommand CancelCommand => cancelCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string clientName;
    public string ClientName
    {
        get => clientName;
        set
        {
            clientName = value;
            OnPropertyChanged(nameof(ClientName));
        }
    }

    private DimaChatService signal;
    private Window window;
    private Command okCommand;
    private Command cancelCommand;
    private int chatId;

    public AddClientViewModel(Window window, DimaChatService signal, int chatId)
    {
        this.window = window;
        this.signal = signal;
        this.chatId = chatId;
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
    }

    private async void Ok()
    {
        if (clientName.Length < 1)
        {
            MessageBox.Show("Пустое поле ввода!");
            return;
        }
        await signal.AddClientToChat(clientName, chatId);
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
