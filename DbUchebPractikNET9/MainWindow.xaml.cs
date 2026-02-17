using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DbUchebPractikNET9.Data;

namespace DbUchebPractikNET9;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(AppDbContext db)
    {
        InitializeComponent();
        
        var users = db.Users.ToList();
        MessageBox.Show($"Подключение работает! Пользователей: {users.Count}");

    }
}