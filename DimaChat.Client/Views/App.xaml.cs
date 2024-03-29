using DimaChat.Client.ViewModels;
using DimaChat.Client.Views;
using System.Windows;

namespace DimaChat.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { ShutdownMode = ShutdownMode.OnMainWindowClose; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new LogInView();
            window.DataContext = new LogInViewModel(window);
            window.Show();
        }
    }
}
