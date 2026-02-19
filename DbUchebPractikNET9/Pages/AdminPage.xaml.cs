using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class AdminPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;
        private readonly MainWindow _main;

        public AdminPage(AppDbContext db, User currentUser, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
            _main = main;

            LoadUsers();
            LoadTechnic();
            LoadCategories();
            LoadOrders();
            LoadOrderStatuses();
        }
        
        private void LoadOrderStatuses()
        {
            var statuses = _db.OrderStatuses.ToList();
            AdminStatusFilter.ItemsSource = statuses;
            AdminStatusFilter.DisplayMemberPath = "StatusTitle";
            AdminStatusFilter.SelectedValuePath = "OrderStatusID";
        }
        
        private void LoadOrders()
        {
            OrdersGrid.ItemsSource = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryOption)
                .ToList();
        }

        private void LoadUsers()
        {
            UsersGrid.ItemsSource = _db.Users
                .Include(u => u.Role)
                .ToList();
        }

        private void LoadTechnic()
        {
            TechnicGrid.ItemsSource = _db.Technics
                .Include(t => t.Category)
                .Include(t => t.Status)
                .ToList();
        }

        private void LoadCategories()
        {
            CategoryGrid.ItemsSource = _db.TechnicCategories.ToList();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }
        
        private void AdminApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var query = _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderStatus)
                .Include(o => o.DeliveryOption)
                .AsQueryable();

            // Поиск по клиенту
            if (!string.IsNullOrWhiteSpace(AdminSearchClientBox.Text))
            {
                string text = AdminSearchClientBox.Text.ToLower();
                query = query.Where(o =>
                    o.User.FirstName.ToLower().Contains(text) ||
                    o.User.LastName.ToLower().Contains(text));
            }

            // Фильтр по статусу
            if (AdminStatusFilter.SelectedValue is int statusId)
            {
                query = query.Where(o => o.IdOrderStatus == statusId);
            }

            // Фильтр по дате "с"
            if (AdminDateFrom.SelectedDate is DateTime from)
            {
                query = query.Where(o => o.OrderDate >= from);
            }

            // Фильтр по дате "по"
            if (AdminDateTo.SelectedDate is DateTime to)
            {
                query = query.Where(o => o.OrderDate <= to);
            }

            OrdersGrid.ItemsSource = query.ToList();
        }
        
        private void ViewOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is not Order order)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            _main.MainFrame.Navigate(new OrderDetailsPage(_db, order, _main));
        }

        private void AdminChangeStatus_Click(object sender, RoutedEventArgs e)
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
                "Изменить статус заказа",
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
            LoadOrders();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is not Order order)
            {
                MessageBox.Show("Выберите заказ");
                return;
            }

            if (MessageBox.Show("Удалить заказ вместе с позициями?", "Подтверждение",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            var items = _db.OrderItems.Where(oi => oi.IdOrder == order.OrderID).ToList();
            _db.OrderItems.RemoveRange(items);
            _db.Orders.Remove(order);
            _db.SaveChanges();
            LoadOrders();
        }

        private void ChangeRole_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not User user)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            var roles = _db.Roles.ToList();

            string list = string.Join("\n", roles.Select(r => $"{r.RoleID} — {r.RoleTitle}"));

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Введите ID новой роли:\n\n{list}",
                "Изменить роль",
                "");

            if (!int.TryParse(input, out int newRoleId))
                return;

            if (!roles.Any(r => r.RoleID == newRoleId))
            {
                MessageBox.Show("Неверный ID роли");
                return;
            }

            user.IdRole = newRoleId;
            _db.SaveChanges();

            LoadUsers();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is not User user)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }

            if (MessageBox.Show("Удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                LoadUsers();
            }
        }

        private void AddTechnic_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new AddTechnicPage(_db, _main));
        }

        private void EditTechnic_Click(object sender, RoutedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is not Technic technic)
            {
                MessageBox.Show("Выберите технику");
                return;
            }

            _main.MainFrame.Navigate(new EditTechnicPage(_db, technic, _main));
        }

        private void DeleteTechnic_Click(object sender, RoutedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is not Technic technic)
            {
                MessageBox.Show("Выберите технику");
                return;
            }

            if (MessageBox.Show("Удалить технику?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _db.Technics.Remove(technic);
                _db.SaveChanges();
                LoadTechnic();
            }
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавление категории сделаем следующим шагом");
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Редактирование категории сделаем следующим шагом");
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryGrid.SelectedItem is not TechnicCategory category)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (MessageBox.Show("Удалить категорию?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _db.TechnicCategories.Remove(category);
                _db.SaveChanges();
                LoadCategories();
            }
        }
    }
}