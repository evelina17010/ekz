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
    /// Логика взаимодействия для AddReviewWindow.xaml
    /// </summary>
    public partial class AddReviewWindow : Window
    {
        private int _userId;

        public Order SelectedOrder { get; }

        public AddReviewWindow(System.Collections.Generic.List<Product> products, int userId)
        {
            InitializeComponent();
            _userId = userId;
            cbProduct.ItemsSource = products;
            cbRating.SelectedIndex = 4; 
        }

        public AddReviewWindow(Order selectedOrder)
        {
            SelectedOrder = selectedOrder;
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
                    UserID = _userId,
                    ProductID = ((Product)cbProduct.SelectedItem).ProductID,
                    ReviewText = txtReviewText.Text,
                    Rating = int.Parse(((ComboBoxItem)cbRating.SelectedItem).Content.ToString()),
                    ReviewDate = DateTime.Now
                };

                DB.storeEntities.Review.Add(newReview);

                var product = (Product)cbProduct.SelectedItem;
                product.AverageRating = DB.storeEntities.Review.Where(r => r.ProductID == product.ProductID)   .Average(r => (decimal?)r.Rating) ?? 0;
                DB.storeEntities.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении отзыва: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
