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

    private ClientModel client;
    public ClientModel Client
    {
        get => client;
        set
        {
            client = value;
            OnPropertyChanged();
        }
    }

    private Window window;
    private Command cancelCommand;
    private Command okCommand;
    private bool? isRegistered = null!;

    public RegisterViewModel(Window window)
    {
        okCommand = new DelegateCommand(_ => Ok());
        cancelCommand = new DelegateCommand(_ => Cancel());
        client = new ClientModel();
        this.window = window;
    }

    private void SetRegistration(bool? b)
    {
        isRegistered = b;
    }

    private async void Ok()
    {
        try
        {
            HubConnection connection = App.HubConnectionConfiguration();

            DimaChatService signal = new DimaChatService(connection);
            await signal.ConnectAsync();
            await signal.SendRegistrationMessage(client.Name, client.Password);
            signal.ReceiveRegistrationMessage();
            signal.RegistartionResponseArrived+=SetRegistration;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (isRegistered != null || stopwatch.Elapsed.TotalSeconds >= 10) break;
                Task.Delay(100).Wait();
            }
            stopwatch.Stop();

            if (isRegistered == null) MessageBox.Show("Превышено время ожидания");
            else if (isRegistered == true)
            {
                window.Close();
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
            signal.RegistartionResponseArrived-=SetRegistration;
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
