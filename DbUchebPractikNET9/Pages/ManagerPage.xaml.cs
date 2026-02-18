using System.Windows.Controls;
using DbUchebPractikNET9.Data;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class ManagerPage : Page
    {
        private readonly AppDbContext _db;
        private readonly User _currentUser;

        public ManagerPage(AppDbContext db, User currentUser)
        {
            InitializeComponent();
            _db = db;
            _currentUser = currentUser;
        }
    }
}