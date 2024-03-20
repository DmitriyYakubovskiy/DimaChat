using DimaChat.Client.Models;
using System.Windows;

namespace DimaChat.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для ChatWindowView.xaml
    /// </summary>
    public partial class ChatWindowView : Window
    {
        private int index = 0;
        private ClientModel client;

        public ChatWindowView(ClientModel client)
        {
            InitializeComponent();
            this.client = new ClientModel();
            this.client.Connect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage(index,MessageTextBox.Text);
        }
    }
}
