using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;

namespace DbUchebPractikNET9.Pages
{
    public partial class LoginPage : Page
    {
        private MainWindow _main;

        public LoginPage(MainWindow main)
        {
            InitializeComponent();
            _main = main;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var email = txtEmail.Text;
                var pass = txtPassword.Password;

                var user = db.Users
                    .Include("Role")
                    .FirstOrDefault(u => u.Email == email && u.PasswordHash == pass);

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }

                switch (user.Role.RoleTitle)
                {
                    case "администратор":
                        _main.MainFrame.Navigate(new AdminPage(user));
                        break;

                    case "менеджер":
                        _main.MainFrame.Navigate(new ManagerPage(user));
                        break;

                    case "техник":
                        _main.MainFrame.Navigate(new TechnicianPage(user));
                        break;

                    default:
                        _main.MainFrame.Navigate(new ClientPage(user));
                        break;
                }
            }
        }
    }
}