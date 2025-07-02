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
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private Order _order;

        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            _order = order;

            txtOrderId.Text = _order.OrderID.ToString();
            dpOrderDate.SelectedDate = _order.OrderDate;
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if (item.Content.ToString() == _order.Status)
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _order.OrderDate = dpOrderDate.SelectedDate ?? DateTime.Now;
                _order.Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
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
