using DimaChat.Client.ViewModels;
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
            var window = new MainWindow();
            window.DataContext = new MainWindowViewModel();
            window.Show();
        }
    }
}
