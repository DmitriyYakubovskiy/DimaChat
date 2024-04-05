using System.Windows;

namespace DimaChat.Client.Views;
/// <summary>
/// Логика взаимодействия для AddChatView.xaml
/// </summary>
public partial class AddChatView : Window
{
    public AddChatView(Window window)
    {
        Owner = window;
        InitializeComponent();
    }
}
