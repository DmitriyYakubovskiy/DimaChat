using DimaChat.Client.Commands;
using DimaChat.Client.Services;
using DimaChat.Client.Views;
using DimaChat.DataAccess.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DimaChat.Client.ViewModels;

public class RegisterViewModel
{    public ICommand OkCommand => okCommand;
    public ICommand CancelCommand => cancelCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private string name = "";
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string password = "";
    public string Password
    {
        get => password;
        set
        {
            password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    private ClientModel client;
    private Window window;
    private Command cancelCommand;
    private Command okCommand;
    private bool serverResponse = false;

    public RegisterViewModel(Window window)
    {
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
        this.window = window;
    }

    private void SetRegistrationInfo(ClientModel client)
    {
        serverResponse = true;
        if (client == null) this.client = null!;
        else this.client = client.Clone() as ClientModel;
    }

    private async void Ok()
    {
        try
        {
            if (name?.Length < 3 || password?.Length < 3)
            {
                MessageBox.Show("Пароль и имя должны быть длиннее 2 символов");
                return;
            }
            HubConnection connection = App.HubConnectionConfiguration();
            DimaChatService signal = new DimaChatService(connection);
            int waitingTime = 10;

            await signal.ConnectAsync();
            await signal.SendRegistrationMessage(Name, Password);
            signal.ReceiveRegistrationMessage();
            signal.RegistartionResponseArrived += SetRegistrationInfo;

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (serverResponse) break;
                if (stopwatch.Elapsed.TotalSeconds >= waitingTime)
                {
                    MessageBox.Show("Превышено время ожидания");
                    signal.AuthorizationResponseArrived -= SetRegistrationInfo;
                    return;
                }
                Task.Delay(100).Wait();
            }
            stopwatch.Stop();

            if (client != null)
            {
                window.Close();
            }
            else
            {
                MessageBox.Show("Пользватель с таким именем уже существует");
            }
            signal.RegistartionResponseArrived -= SetRegistrationInfo;
            serverResponse = false;
        }
        catch (SocketException ex)
        {
            MessageBox.Show($"SocketException: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception: {ex.Message}");
        }
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
