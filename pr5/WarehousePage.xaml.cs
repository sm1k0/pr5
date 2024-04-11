using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class WarehousePage : Page
    {
        private prEntities db;

        public WarehousePage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadProductData();
            LoadWarehouseData();
        }

        private void LoadProductData()
        {
            productComboBox.ItemsSource = db.Product.ToList();
        }

        private void LoadWarehouseData()
        {
            WarehouseDataGrid.ItemsSource = db.Warehouse.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int quantity;
                if (!int.TryParse(quantityTextBox.Text, out quantity))
                {
                    MessageBox.Show("Количество должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (quantity <= 0)
                {
                    MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Warehouse newWarehouse = new Warehouse()
                {
                    Product_ID = (int)productComboBox.SelectedValue,
                    Quantity = quantity,
                    W_Availability = "В наличии"
                };

                db.Warehouse.Add(newWarehouse);
                db.SaveChanges();

                LoadWarehouseData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных на склад: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WarehouseDataGrid.SelectedItem != null)
                {
                    Warehouse selectedWarehouse = (Warehouse)WarehouseDataGrid.SelectedItem;
                    int newQuantity;

                    if (!int.TryParse(quantityTextBox.Text, out newQuantity))
                    {
                        MessageBox.Show("Количество должно быть целым числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (newQuantity <= 0)
                    {
                        MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    selectedWarehouse.Quantity = newQuantity;
                    db.SaveChanges();

                    LoadWarehouseData();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент склада для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании данных на складе: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WarehouseDataGrid.SelectedItem != null)
                {
                    Warehouse selectedWarehouse = (Warehouse)WarehouseDataGrid.SelectedItem;

                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот элемент со склада?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Warehouse.Remove(selectedWarehouse);
                        db.SaveChanges();

                        LoadWarehouseData();
                        ClearInputFields();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите элемент склада для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении данных со склада: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            quantityTextBox.Text = "Quantity";
            productComboBox.SelectedIndex = -1;
        }
    }
}
