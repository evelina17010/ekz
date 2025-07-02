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
using System.Windows.Shapes;
using ekz.DBconn;

namespace ekz.Win
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
            cmbUser.ItemsSource = DB.storeEntities.User.ToList();
            cmbStatus.SelectedIndex = 0;
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbUser.SelectedItem == null)
                {
                    MessageBox.Show("Выберите пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var newOrder = new Order
                {
                    UserID = ((User)cmbUser.SelectedItem).UserID,
                    OrderDate = dpOrderDate.SelectedDate ?? DateTime.Now,
                    Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    PaymentMethod = (cmbPaymentMethod.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    ShippingAddress = ((User)cmbUser.SelectedItem).Address,
                    TotalAmount = 0 
                };
                DB.storeEntities.Order.Add(newOrder);
                DB.storeEntities.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
