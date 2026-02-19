using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class ManagerPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;
        private readonly MainWindow _main;

        public ManagerPage(AppDbContext db, User currentUser, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
            _main = main;

            LoadOrders();
            LoadStatuses();
        }
        
        private void LoadStatuses()
        {
            var statuses = _db.OrderStatuses.ToList();
            StatusFilter.ItemsSource = statuses;
            StatusFilter.DisplayMemberPath = "StatusTitle";
            StatusFilter.SelectedValuePath = "OrderStatusID";
        }

        private void LoadOrders()
        {
            OrdersGrid.ItemsSource = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryOption)
                .ToList();
        }
        
        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var query = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryOption)
                .AsQueryable();

            // Поиск по клиенту
            if (!string.IsNullOrWhiteSpace(SearchClientBox.Text))
            {
                string text = SearchClientBox.Text.ToLower();
                query = query.Where(o =>
                    o.User.FirstName.ToLower().Contains(text) ||
                    o.User.LastName.ToLower().Contains(text));
            }

            // Фильтр по статусу
            if (StatusFilter.SelectedValue is int statusId)
            {
                query = query.Where(o => o.IdOrderStatus == statusId);
            }

            // Фильтр по дате "с"
            if (DateFrom.SelectedDate is DateTime from)
            {
                query = query.Where(o => o.OrderDate >= from);
            }

            // Фильтр по дате "по"
            if (DateTo.SelectedDate is DateTime to)
            {
                query = query.Where(o => o.OrderDate <= to);
            }

            OrdersGrid.ItemsSource = query.ToList();
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is not Order order)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            _main.MainFrame.Navigate(new OrderDetailsPage(_db, order, _main));
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is not Order order)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            var statuses = _db.OrderStatuses.ToList();

            string list = string.Join("\n", statuses.Select(s => $"{s.OrderStatusID} — {s.StatusTitle}"));

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Введите ID нового статуса:\n\n{list}",
                "Изменить статус",
                "");

            if (!int.TryParse(input, out int newStatusId))
                return;

            if (!statuses.Any(s => s.OrderStatusID == newStatusId))
            {
                MessageBox.Show("Неверный ID статуса");
                return;
            }

            order.IdOrderStatus = newStatusId;
            _db.SaveChanges();

            MessageBox.Show("Статус обновлён");
            LoadOrders();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }
    }
}