using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class AddTechnicPage : Page
    {
        private readonly AppDbContext _db;
        private readonly MainWindow _main;

        public AddTechnicPage(AppDbContext db, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _main = main;

            LoadData();
        }

        private void LoadData()
        {
            CategoryBox.ItemsSource = _db.TechnicCategories.ToList();
            CategoryBox.DisplayMemberPath = "CategoryTitle";
            CategoryBox.SelectedValuePath = "CategoryID";

            StatusBox.ItemsSource = _db.TechnicStatuses.ToList();
            StatusBox.DisplayMemberPath = "StatusTitle";
            StatusBox.SelectedValuePath = "StatusID";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text) ||
                string.IsNullOrWhiteSpace(PriceBox.Text) ||
                CategoryBox.SelectedValue == null ||
                StatusBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом");
                return;
            }

            var technic = new Technic
            {
                Title = TitleBox.Text,
                Description = DescriptionBox.Text,
                IdCategory = (int)CategoryBox.SelectedValue,
                IdStatus = (int)StatusBox.SelectedValue,
                PricePerDay = price
            };

            _db.Technics.Add(technic);
            _db.SaveChanges();

            MessageBox.Show("Техника добавлена");
            _main.MainFrame.Navigate(new AdminPage(_db, null, _main));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new AdminPage(_db, null, _main));
        }
    }
}