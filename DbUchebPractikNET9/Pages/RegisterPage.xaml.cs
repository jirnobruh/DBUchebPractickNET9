using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Helpers;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class RegisterPage : Page
    {
        private readonly AppDbContext _db;
        private readonly MainWindow _main;

        public RegisterPage(AppDbContext db, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _main = main;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string first = FirstNameBox.Text.Trim();
            string last = LastNameBox.Text.Trim();
            string phone = PhoneBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (first == "" || last == "" || phone == "" || email == "" || password == "")
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (_db.Users.Any(u => u.Email == email))
            {
                MessageBox.Show("Пользователь с таким email уже существует");
                return;
            }

            var user = new User
            {
                FirstName = first,
                LastName = last,
                Phone = phone,
                Email = email,
                PasswordHash = PasswordHasher.Hash(password),
                CreatedAt = DateTime.UtcNow,
                IdRole = 3
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            MessageBox.Show("Аккаунт создан! Теперь войдите.");

            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }

        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }
    }
}