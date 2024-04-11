using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class ProductPage : Page
    {
        private prEntities db;

        public ProductPage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadProductData();
        }

        private void LoadProductData()
        {
            try
            {
                ProductDataGrid.ItemsSource = db.Product.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productNameTextBox.Text) || string.IsNullOrWhiteSpace(priceTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal price;
                if (!decimal.TryParse(priceTextBox.Text, out price))
                {
                    MessageBox.Show("Пожалуйста, введите допустимую цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Product newProduct = new Product()
                {
                    Product_Name = productNameTextBox.Text,
                    Price = price
                };

                db.Product.Add(newProduct);
                db.SaveChanges();

                LoadProductData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении нового продукта: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductDataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите продукт для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrWhiteSpace(productNameTextBox.Text) || string.IsNullOrWhiteSpace(priceTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal price;
                if (!decimal.TryParse(priceTextBox.Text, out price))
                {
                    MessageBox.Show("Пожалуйста, введите допустимую цену.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Product selectedProduct = (Product)ProductDataGrid.SelectedItem;
                selectedProduct.Product_Name = productNameTextBox.Text;
                selectedProduct.Price = price;

                db.SaveChanges();

                LoadProductData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании продукта: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductDataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите продукт для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Product selectedProduct = (Product)ProductDataGrid.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.Product.Remove(selectedProduct);
                    db.SaveChanges();

                    LoadProductData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении продукта: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == textBox.Tag.ToString())
                {
                    textBox.Text = "";
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = textBox.Tag.ToString();
                }
            }
        }

        private void ClearInputFields()
        {
            productNameTextBox.Text = "Product Name";
            priceTextBox.Text = "Price";
        }
    }
}
