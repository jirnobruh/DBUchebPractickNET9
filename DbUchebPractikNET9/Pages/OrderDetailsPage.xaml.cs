using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class OrderDetailsPage : Page
    {
        private readonly AppDbContext _db;
        private readonly Order _order;
        private readonly MainWindow _main;

        public OrderDetailsPage(AppDbContext db, Order order, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _order = order;
            _main = main;

            LoadOrder();
        }

        private void LoadOrder()
        {
            // Загружаем заказ с зависимостями
            var fullOrder = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryOption)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Technic)
                .First(o => o.OrderID == _order.OrderID);

            OrderInfoText.Text =
                $"Заказ №{fullOrder.OrderID}\n" +
                $"Клиент: {fullOrder.User.FirstName} {fullOrder.User.LastName}\n" +
                $"Дата: {fullOrder.OrderDate}\n" +
                $"Статус: {fullOrder.OrderStatus.StatusTitle}\n" +
                $"Доставка: {fullOrder.DeliveryOption.OptionTitle}";

            // Готовим данные для таблицы
            var items = fullOrder.OrderItems
                .Select(oi => new
                {
                    oi.Technic,
                    oi.Quantity,
                    oi.PricePerDay,
                    Total = oi.Quantity * oi.PricePerDay
                })
                .ToList();

            ItemsGrid.ItemsSource = items;

            // Итоговая сумма
            decimal total = items.Sum(i => i.Total);
            TotalText.Text = $"Итого: {total} ₽";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new ManagerPage(_db, _order.User, _main));
        }
    }
}