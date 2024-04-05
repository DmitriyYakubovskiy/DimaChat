using System.Windows;

namespace DimaChat.Client;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(Window window)
    {
        Owner=window;
        InitializeComponent();
    }
}