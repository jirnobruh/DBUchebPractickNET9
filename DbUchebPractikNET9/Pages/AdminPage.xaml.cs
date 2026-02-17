using System.Windows.Controls;
using DbUchebPractikNET9.Models;

namespace DbUchebPractikNET9.Pages
{
    public partial class AdminPage : Page
    {
        private User _currentUser;

        public AdminPage(User user)
        {
            InitializeComponent();
            _currentUser = user;

            txtWelcome.Text = $"Добро пожаловать, {_currentUser.FirstName}!";
        }
    }

}