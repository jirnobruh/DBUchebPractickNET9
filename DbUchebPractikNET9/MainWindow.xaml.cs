using System.Windows;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;
using DbUchebPractikNET9.Pages;

namespace DbUchebPractikNET9
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _db;

        public MainWindow(AppDbContext db)
        {
            InitializeComponent();
            _db = db;

            MainFrame.Navigate(new LoginPage(_db, this));
        }

        public void NavigateToAdmin(User user)
        {
            MainFrame.Navigate(new AdminPage(_db, user));
        }

        public void NavigateToManager(User user)
        {
            MainFrame.Navigate(new ManagerPage(_db, user));
        }

        public void NavigateToTechnician(User user)
        {
            MainFrame.Navigate(new TechnicianPage(_db, user, this));
        }

        public void NavigateToClient(User user)
        {
            MainFrame.Navigate(new ClientPage(_db, user, this));
        }
    }
}