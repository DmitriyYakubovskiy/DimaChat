using System.Windows;

namespace DimaChat.Client.Views;
/// <summary>
/// Логика взаимодействия для AddClientView.xaml
/// </summary>
public partial class AddClientView : Window
{
    public AddClientView(Window window)
    {
        this.Owner= window; 
        InitializeComponent();
    }
}
