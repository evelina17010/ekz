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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadProducts();
            LoadCategories();
        }
        private void LoadProducts()
        {
            lvProducts.ItemsSource = DB.storeEntities.Product.ToList();
        }
        private void LoadCategories()
        {
            cmbCategoryFilter.ItemsSource = DB.storeEntities.Category.ToList();
            cmbCategoryFilter.SelectedIndex = -1;
        }
        private void txtProductSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = txtProductSearch.Text.ToLower();
            var filteredProducts = DB.storeEntities.Product.Where(p => p.Name.ToLower().Contains(searchText)).ToList();
            if (cmbCategoryFilter.SelectedItem != null)
            {
                var selectedCategory = (Category)cmbCategoryFilter.SelectedItem;
                filteredProducts = filteredProducts.Where(p => p.CategoryID == selectedCategory.CategoryID).ToList();
            }
            lvProducts.ItemsSource = filteredProducts;
        }

        private void cmbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtProductSearch_TextChanged(null,null);
        }

        private void btnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            txtProductSearch.Text = "";
            cmbCategoryFilter.SelectedIndex = -1;
            LoadProducts();
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditProductWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadProducts();
            }
        }
        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem is Product selectedProduct)
            {
                var editWindow = new AddEditProductWindow(selectedProduct);
                if (editWindow.ShowDialog() == true)
                {
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Ошибка",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (lvProducts.SelectedItem is Product selectedProduct)
            {
                if (MessageBox.Show("Удалить выбранный товар?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DB.storeEntities.Product.Remove(selectedProduct);
                        DB.storeEntities.SaveChanges();
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",  MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderPage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Authoresation());
        }
    }
}

