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
    /// Логика взаимодействия для OrderStatusWindow.xaml
    /// </summary>
    public partial class OrderStatusWindow : Window
    {
        private Order _order;

        public OrderStatusWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            txtCurrentStatus.Text = _order.Status;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbNewStatus.SelectedItem is ComboBoxItem selectedStatus)
            {
                _order.Status = selectedStatus.Content.ToString();
                DB.storeEntities.SaveChanges();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите новый статус", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
