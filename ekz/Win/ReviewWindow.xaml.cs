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
    /// Логика взаимодействия для ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        private Order _selectedOrder;

        public ReviewWindow(Order selectedOrder)
        {
            InitializeComponent();
            _selectedOrder = selectedOrder;
            LoadProductsFromOrder();
            cbRating.SelectedIndex = 4;
        }

        private void LoadProductsFromOrder()
        {
            var products = DB.storeEntities.OrderItem
                .Where(oi => oi.OrderID == _selectedOrder.OrderID)
                .Select(oi => oi.Product)
                .ToList();

            cbProduct.ItemsSource = products;

            if (products.Any())
            {
                cbProduct.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("В выбранном заказе нет товаров", "Информация",MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbProduct.SelectedItem == null || cbRating.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(txtReviewText.Text))
                {
                    MessageBox.Show("Заполните все поля", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newReview = new Review
                {
                    UserID = _selectedOrder.UserID,
                    ProductID = ((Product)cbProduct.SelectedItem).ProductID,
                    ReviewText = txtReviewText.Text,
                    Rating = int.Parse(((ComboBoxItem)cbRating.SelectedItem).Content.ToString()),
                    ReviewDate = DateTime.Now
                };
                DB.storeEntities.Review.Add(newReview);
                UpdateProductAverageRating((Product)cbProduct.SelectedItem);
                DB.storeEntities.SaveChanges();

                MessageBox.Show("Отзыв успешно сохранен", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отзыва: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProductAverageRating(Product product)
        {
            product.AverageRating = DB.storeEntities.Review
                .Where(r => r.ProductID == product.ProductID)
                .Average(r => (decimal?)r.Rating) ?? 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
