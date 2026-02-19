using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Helpers;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

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

            var user = _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email);


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
            switch (user.IdRole)
            {
                case 1:
                    _main.NavigateToAdmin(user);
                    break;

                case 2:
                    _main.NavigateToManager(user);
                    break;

                case 3:
                    _main.NavigateToClient(user);
                    break;

                case 4:
                    _main.NavigateToTechnician(user);
                    break;

                default:
                    MessageBox.Show("Неизвестная роль пользователя");
                    break;
            }
        }
        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new RegisterPage(_db, _main));
        }
    }
}