using System.Windows;

namespace DimaChat.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для ChatWindowView.xaml
    /// </summary>
    public partial class ChatWindowView : Window
    {
        public ChatWindowView(Window window)
        {
            Owner = window;
            InitializeComponent();
        }
    }
}
