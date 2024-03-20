using DimaChat.Client.Models;
using DimaChat.Client.ViewModels;
using DimaChat.Client.Views;
using System.Net.Sockets;
using System.Windows;

namespace DimaChat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ip;
        private int port;
        private ClientModel client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ip=IpTextBox.Text;
            port=Convert.ToInt32(PortTextBox.Text);
            client = new ClientModel();
            var window = new ChatWindowView(client);
            window.DataContext = new ChatWindowViewModel();
            window.Show();
        }
    }
}