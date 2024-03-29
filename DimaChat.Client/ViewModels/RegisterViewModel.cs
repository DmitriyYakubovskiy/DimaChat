using DimaChat.Client.Commands;
using DimaChat.Client.Enums;
using DimaChat.Client.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;
public class RegisterViewModel
{
    public ClientModel Client
    {
        get => client;
        set
        {
            client = value;
            OnPropertyChanged();
        }
    }

    public ICommand OkCommand => okCommand;
    public ICommand CancelCommand => cancelCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private ClientModel client;
    private Command cancelCommand;
    private Command okCommand;
    private Window window;

    public RegisterViewModel(Window window)
    {
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
        client = new ClientModel();
        this.window = window;
        this.window.Closing += OnWindowClosing;
    }

    private void OnWindowClosing(object sender, CancelEventArgs e)
    {
        //client.Close();
    }

    private async void Ok()
    {
        //try
        //{
            if (!CanOk()) return;
            client.Connect();
            int count = client.GetCountMessages();
            client.SendMessage((int)ServerCommands.Registration, client.Password);
            MessageModel messageModel = client.GetLastMessage();
            while (true)
            {
                messageModel = client.GetLastMessage();
                if (messageModel?.Name == "server")
                {
                    break;
                }
                await Task.Delay(1000);
            }

            if (messageModel?.AddresseeId != (int)ServerCommands.RegistrationResult)
            {
                MessageBox.Show("Ошибка2");
                return;
            }
            if (messageModel?.Content != "Ok")
            {
                MessageBox.Show("Неверное имя или пароль");
                return;
            }
            window.Close();
        //}
        //catch (Exception ex)
        //{
        //    client.Close();
        //    window.Close();
        //    MessageBox.Show($"Exception: {ex.Message}");
        //}
    }

    private bool CanOk()
    {
        if (client.Name?.Length < 3 || client.Password?.Length < 3)
        {
            MessageBox.Show("The field length is less than three characters");
            return false;
        }
        return true;
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
