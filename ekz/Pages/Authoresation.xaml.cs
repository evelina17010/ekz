using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ekz.DBconn;
using ekz.Fun;

namespace ekz.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authoresation.xaml
    /// </summary>
    public partial class Authoresation : Page
    {
        public static User user;
        public Authoresation()
        {
            InitializeComponent();
        }
        public User sotrudnik { get; private set; }

        private void btnVhod_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTxb.Text.Trim();
            string password = parolTxb.Password.Trim();
            user = Authorisation.AuthorisationSotr(login, password);
            sotrudnik = Authorisation.AuthorisationSotr(login, password);
            if (sotrudnik != null)
            {
                if (sotrudnik.Role == "Admin" || sotrudnik.Role == "Manager")
                {
                    NavigationService.Navigate(new AdminPage());
                }
            }
            else MessageBox.Show("Логин или пароль неверный", "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

