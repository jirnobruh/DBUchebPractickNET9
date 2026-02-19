using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class CartPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;
        private readonly MainWindow _main;
        private Cart _cart;

        public CartPage(AppDbContext db, User currentUser, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
            _main = main;

            LoadCart();
        }

        private void LoadCart()
        {
            _cart = _db.Carts.FirstOrDefault(c => c.IdUser == _currentUser.UserID);

            if (_cart == null)
            {
                CartGrid.ItemsSource = null;
                return;
            }

            CartGrid.ItemsSource = _db.CartItems
                .Where(ci => ci.IdCart == _cart.CartID)
                .Include(ci => ci.Technic)
                .ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new ClientPage(_db, _currentUser, _main));
        }

        private void MakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_cart == null)
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            var items = _db.CartItems
                .Where(ci => ci.IdCart == _cart.CartID)
                .Include(ci => ci.Technic)
                .ToList();

            if (!items.Any())
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            // Создаём заказ
            var order = new Order
            {
                IdUser = _currentUser.UserID,
                OrderDate = DateTime.UtcNow,
                IdOrderStatus = 1, // например, "Новый"
                IdDeliveryOption = 1 // временно
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            // Добавляем позиции заказа
            foreach (var item in items)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    IdOrder = order.OrderID,
                    IdTechnic = item.IdTechnic,
                    Quantity = item.Quantity,
                    PricePerDay = item.Technic.PricePerDay
                });
            }

            // Очищаем корзину
            _db.CartItems.RemoveRange(items);
            _db.SaveChanges();

            MessageBox.Show("Заказ оформлен!");

            _main.MainFrame.Navigate(new ClientPage(_db, _currentUser, _main));
        }
    }
}