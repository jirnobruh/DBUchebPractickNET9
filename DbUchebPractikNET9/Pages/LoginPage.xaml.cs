using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Helpers;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class LoginPage : Page
    {
        private readonly AppDbContext _db;
        private readonly MainWindow _main;

        public LoginPage(AppDbContext db, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _main = main;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (email.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Введите email и пароль");
                return;
            }

            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }

            // Переход по ролям
            switch (user.Role.RoleTitle.ToLower())
            {
                case "администратор":
                    //_main.NavigateToAdmin(user);
                    break;

                case "менеджер":
                    //_main.NavigateToManager(user);
                    break;

                case "техник":
                    //_main.NavigateToTechnician(user);
                    break;

                default:
                    //_main.NavigateToClient(user);
                    break;
            }
        }
    }
}