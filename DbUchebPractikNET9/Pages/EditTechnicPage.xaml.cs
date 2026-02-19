using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class EditTechnicPage : Page
    {
        private readonly AppDbContext _db;
        private readonly MainWindow _main;
        private readonly Technic _technic;

        public EditTechnicPage(AppDbContext db, Technic technic, MainWindow main)
        {
            InitializeComponent();
            _db = db;
            _main = main;
            _technic = technic;

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

            TitleBox.Text = _technic.Title;
            DescriptionBox.Text = _technic.Description;
            CategoryBox.SelectedValue = _technic.IdCategory;
            StatusBox.SelectedValue = _technic.IdStatus;
            PriceBox.Text = _technic.PricePerDay.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом");
                return;
            }

            _technic.Title = TitleBox.Text;
            _technic.Description = DescriptionBox.Text;
            _technic.IdCategory = (int)CategoryBox.SelectedValue;
            _technic.IdStatus = (int)StatusBox.SelectedValue;
            _technic.PricePerDay = price;

            _db.SaveChanges();

            MessageBox.Show("Изменения сохранены");
            _main.MainFrame.Navigate(new AdminPage(_db, null, _main));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _main.MainFrame.Navigate(new AdminPage(_db, null, _main));
        }
    }
}