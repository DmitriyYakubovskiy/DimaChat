using DimaChat.Client.ViewModels;
using DimaChat.Client.Views;
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
        private string name;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ip=IpTextBox.Text;
            port=Convert.ToInt32(PortTextBox.Text);
            name=NameTextBox.Text;
            var window = new ChatWindowView(this);
            window.DataContext = new ChatWindowViewModel(name,ip, port);
            window.Show();
        }
    }
}