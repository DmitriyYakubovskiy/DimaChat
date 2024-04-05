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

public class LogInViewModel:INotifyPropertyChanged
{    
    public ICommand OkCommand => okCommand;
    public ICommand RegCommand => regCommand;

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
    private Command okCommand;
    private Command regCommand;
    private bool serverResponse = false;

    public LogInViewModel(Window window)
    {
        okCommand = new DelegateCommand(_ => Ok());
        regCommand = new DelegateCommand(_ => Reg());
        this.window = window;
    }

    private void SetAuthorizationInfo(ClientModel client)
    {
        serverResponse = true;
        if (client == null) this.client = null!;
        else this.client=client.Clone() as ClientModel;
    }

    private async void Ok()
    {
        try
        {
            if (name?.Length < 1 || password?.Length < 1)
            {
                MessageBox.Show("Пустое поле ввода!");
                return;
            }
            HubConnection connection = App.HubConnectionConfiguration();
            DimaChatService signal = new DimaChatService(connection);
            int waitingTime = 10;

            await signal.ConnectAsync();
            signal.ReceiveAuthorizeMessage();
            await signal.SendAuthorizeMessage(Name, Password);
            signal.AuthorizationResponseArrived += SetAuthorizationInfo;

            Name = String.Empty;
            Password = String.Empty;
            OnPropertyChanged();

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (serverResponse) break;
                if (stopwatch.Elapsed.TotalSeconds >= waitingTime)
                {
                    MessageBox.Show("Превышено время ожидания");
                    signal.AuthorizationResponseArrived -= SetAuthorizationInfo;
                    return;
                }
                Task.Delay(100).Wait();
            }
            stopwatch.Stop();

            if (client!=null)
            {
                var mainWindow = new MainWindow(window);
                mainWindow.DataContext = new MainWindowViewModel(mainWindow, client, signal);
                if (mainWindow.ShowDialog()!=true) return;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            signal.AuthorizationResponseArrived -= SetAuthorizationInfo;
            serverResponse = false;
        }
        catch(SocketException ex)
        {
            MessageBox.Show($"SocketException: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception: {ex.Message}");
        }
    }

    private void Reg()
    {
        var window = new RegisterView();
        var viewModel = new RegisterViewModel(window);
        window.DataContext = viewModel;
        if (window.ShowDialog() != true) return;
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

