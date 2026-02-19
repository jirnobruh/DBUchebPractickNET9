using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUchebPractikNET9.Pages
{
    public partial class TechnicianPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;
        private readonly MainWindow _main;

        public TechnicianPage(AppDbContext db, User currentUser, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
            _main = main;

            LoadTechnic();
        }


        private void LoadTechnic()
        {
            TechnicGrid.ItemsSource = _db.Technics
                .Include(t => t.Category)
                .Include(t => t.Status)
                .ToList();
        }
        
        private void LoadServiceHistory(int technicId)
        {
            ServiceHistoryGrid.ItemsSource = _db.TechnicalServices
                .Where(s => s.IdTechnic == technicId)
                .Include(s => s.PerformedUser)
                .OrderByDescending(s => s.TsDate)
                .ToList();
        }
        
        private void TechnicGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is Technic technic)
            {
                LoadServiceHistory(technic.TechnicID);
            }
        }
        
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new LoginPage(_db, _main));
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is not Technic technic)
            {
                MessageBox.Show("Выберите технику");
                return;
            }

            string desc = Microsoft.VisualBasic.Interaction.InputBox(
                "Введите описание выполненного обслуживания:",
                "Добавить ТО",
                "");

            if (string.IsNullOrWhiteSpace(desc))
                return;

            var service = new TechnicalService
            {
                IdTechnic = technic.TechnicID,
                IdPerformedUser = _currentUser.UserID,
                TsDate = DateTime.UtcNow,
                Description = desc
            };

            _db.TechnicalServices.Add(service);
            _db.SaveChanges();

            MessageBox.Show("Запись о ТО добавлена");

            LoadServiceHistory(technic.TechnicID);
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (TechnicGrid.SelectedItem is not Technic technic)
            {
                MessageBox.Show("Выберите технику");
                return;
            }

            var statuses = _db.TechnicStatuses.ToList();

            string list = string.Join("\n", statuses.Select(s => $"{s.TechnicStatusID} — {s.StatusTitle}"));

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Введите ID нового статуса:\n\n{list}",
                "Изменить статус",
                "");

            if (!int.TryParse(input, out int newStatusId))
                return;

            if (!statuses.Any(s => s.TechnicStatusID == newStatusId))
            {
                MessageBox.Show("Неверный ID статуса");
                return;
            }

            technic.IdStatus = newStatusId;
            _db.SaveChanges();

            MessageBox.Show("Статус обновлён");
            LoadTechnic();
        }
    }
}