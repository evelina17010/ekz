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
using ekz.Win;

namespace ekz.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            lvOrders.ItemsSource = DB.storeEntities.Order.ToList();
        }

        private void btnApplyOrderFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = DB.storeEntities.Order.AsQueryable();

                if (dpOrderDate.SelectedDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate == dpOrderDate.SelectedDate.Value);
                }

                if (cmbOrderStatus.SelectedItem is ComboBoxItem selectedStatus)
                {
                    string status = selectedStatus.Content.ToString();
                    if (status != "Все")
                    {
                        query = query.Where(o => o.Status == status);
                    }
                }
                lvOrders.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem is Order selectedOrder)
            {
                if (MessageBox.Show("Удалить выбранный заказ?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DB.storeEntities.Order.Remove(selectedOrder);
                        DB.storeEntities.SaveChanges();
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem is Order selectedOrder)
            {
                var editWindow = new EditOrderWindow(selectedOrder);
                if (editWindow.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddOrderWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadOrders();
            }
        }

        private void btnOtz_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem is Order selectedOrder)
            {
                var reviewWindow = new ReviewWindow(selectedOrder);
                reviewWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите заказ для оставления отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnUpdateOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            if (lvOrders.SelectedItem is Order selectedOrder)
            {
                var statusWindow = new OrderStatusWindow(selectedOrder);
                if (statusWindow.ShowDialog() == true)
                {
                    LoadOrders();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для изменения статуса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }

        private void btnSbros_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }
    } }

