using DimaChat.Client.ViewModels;
using DimaChat.DataAccess.Mappers;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DimaChat.Client.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() { ShutdownMode = ShutdownMode.OnMainWindowClose; }

        public static HubConnection HubConnectionConfiguration()
        {
            return new HubConnectionBuilder()
            .WithUrl("http://localhost:5167/dimachat")
            .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingClient));
            services.AddAutoMapper(typeof(MappingMessage));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ConfigureServices(new ServiceCollection());
            var window = new LogInView();
            window.DataContext = new LogInViewModel(window);
            window.Show();
        }
    }
}
