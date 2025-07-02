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
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private Product _product;
        public AddEditProductWindow()
        {
            InitializeComponent();
            _product = new Product();
            cmbCategory.ItemsSource = DB.storeEntities.Category.ToList();
        }
        public AddEditProductWindow(Product product) : this()
        {
            _product = product;
            Title = "Редактировать товар";

            txtName.Text = _product.Name;
            txtDescription.Text = _product.Description;
            txtPrice.Text = _product.Price.ToString();
            txtStockQuantity.Text = _product.StockQuantity.ToString();
            cmbCategory.SelectedItem = cmbCategory.ItemsSource.Cast<Category>().FirstOrDefault(c => c.CategoryID == _product.CategoryID);
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) ||
                    string.IsNullOrWhiteSpace(txtPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtStockQuantity.Text) ||
                    cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                _product.Name = txtName.Text;
                _product.Description = txtDescription.Text;
                _product.Price = decimal.Parse(txtPrice.Text);
                _product.StockQuantity = int.Parse(txtStockQuantity.Text);
                _product.CategoryID = ((Category)cmbCategory.SelectedItem).CategoryID;
                if (_product.ProductID == 0) 
                {
                    DB.storeEntities.Product.Add(_product);
                }
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
