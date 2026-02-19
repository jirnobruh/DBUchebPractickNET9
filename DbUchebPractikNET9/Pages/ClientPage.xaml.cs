using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class ClientPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;
        private readonly MainWindow _main;

        public ClientPage(AppDbContext db, User currentUser, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
            _main = main;

            WelcomeText.Text = $"Добро пожаловать, {_currentUser.FirstName}!";

            LoadTechnic();
        }

        private void LoadTechnic()
        {
            TechnicGrid.ItemsSource = _db.Technics
                .Include(t => t.Category)
                .Include(t => t.Status)
                .Where(t => t.Status.StatusTitle == "доступна")
                .ToList();
        }
        
        private void OpenCart_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new CartPage(_db, _currentUser, _main));
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is not Technic technic)
            {
                MessageBox.Show("Выберите технику");
                return;
            }

            // Находим корзину пользователя
            var cart = _db.Carts.FirstOrDefault(c => c.IdUser == _currentUser.UserID);

            if (cart == null)
            {
                cart = new Cart { IdUser = _currentUser.UserID };
                _db.Carts.Add(cart);
                _db.SaveChanges();
            }

            // Проверяем, есть ли уже этот товар в корзине
            var item = _db.CartItems.FirstOrDefault(ci =>
                ci.IdCart == cart.CartID && ci.IdTechnic == technic.TechnicID);

            if (item == null)
            {
                item = new CartItem
                {
                    IdCart = cart.CartID,
                    IdTechnic = technic.TechnicID,
                    Quantity = 1
                };
                _db.CartItems.Add(item);
            }
            else
            {
                item.Quantity++;
            }

            _db.SaveChanges();

            MessageBox.Show("Техника добавлена в корзину");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }
    }
}