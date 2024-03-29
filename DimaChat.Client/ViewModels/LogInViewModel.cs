using DimaChat.Client.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DimaChat.Client.Models;
using System.Windows;
using DimaChat.Client.Enums;
using System.Diagnostics;

namespace DimaChat.Client.ViewModels;

public class LogInViewModel
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
    public ICommand RegCommand => regCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    private ClientModel client;
    private Command okCommand;
    private Command regCommand;
    private Window window;

    public LogInViewModel(Window window)
    {
        okCommand = new DelegateCommand(_ => Ok());
        regCommand = new DelegateCommand(_ => Reg());
        client = new ClientModel();
        this.window = window;
        this.window.Closing += OnWindowClosing;
    }

    private void OnWindowClosing(object sender, CancelEventArgs e)
    {
        client.Close();
    }

    private async void Ok()
    {
        //try
        //{
        client.Connect();
        int count = client.GetCountMessages();
        client.SendMessage((int)ServerCommands.VerificationData, client.Password);
        MessageModel messageModel = client.GetLastMessage();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (true)
        {
            messageModel = client.GetLastMessage();
            if (messageModel?.Name == "server")
            {
                break;
            }
            if (stopwatch.ElapsedMilliseconds >= 10000)
            {
                MessageBox.Show("Превышено время ожидания!");
                stopwatch.Stop();
                return;
            }
            await Task.Delay(1000);
        }

        if (messageModel?.AddresseeId != (int)ServerCommands.VerificationResult)
        {
            MessageBox.Show("Ошибка2");
            return;
        }
        if (messageModel?.Content != "Ok")
        {
            MessageBox.Show("Неверное имя или пароль");
            return;
        }

        var mainWindow = new MainWindow(window);
        var viewModel = new MainWindowViewModel(mainWindow, client);
        mainWindow.DataContext = viewModel;
        if (mainWindow.ShowDialog() != true) return;
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show($"Exception: {ex.Message}");
        //}
    }

    private void Reg()
    {
        var window = new RegisterView();
        var viewModel = new RegisterViewModel(window);
        window.DataContext = viewModel;
        window.Show();
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

