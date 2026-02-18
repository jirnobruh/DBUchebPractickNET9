using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class AdminPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;

        public AdminPage(AppDbContext db, User currentUser)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
        }
    }
}