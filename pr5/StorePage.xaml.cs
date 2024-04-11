using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class StorePage : Page
    {
        private prEntities db;
        private Random random;

        public StorePage()
        {
            InitializeComponent();

            db = new prEntities();
            random = new Random();
            LoadStoreData();
        }

        private void LoadStoreData()
        {
            try
            {
                StoreDataGrid.ItemsSource = db.Store.ToList();
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
                string address = addressTextBox.Text.Trim();

                if (string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Адрес не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Генерация случайного Warehouse_ID
                int randomWarehouseID = random.Next(1, 5);

                Store newStore = new Store()
                {
                    Warehouse_ID = randomWarehouseID,
                    S_Address = address
                };

                db.Store.Add(newStore);
                db.SaveChanges();

                LoadStoreData();
                addressTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении нового магазина: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Store selectedStore = (Store)StoreDataGrid.SelectedItem;

                if (selectedStore == null)
                {
                    MessageBox.Show("Пожалуйста, выберите магазин для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string address = addressTextBox.Text.Trim();

                if (string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Адрес не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedStore.S_Address = address;
                db.SaveChanges();

                LoadStoreData();
                addressTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании магазина: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Store selectedStore = (Store)StoreDataGrid.SelectedItem;

                if (selectedStore == null)
                {
                    MessageBox.Show("Пожалуйста, выберите магазин для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                db.Store.Remove(selectedStore);
                db.SaveChanges();

                LoadStoreData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении магазина: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
