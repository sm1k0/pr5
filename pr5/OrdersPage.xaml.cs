using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class OrdersPage : Page
    {
        private prEntities db;

        public OrdersPage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadComboBoxData();
            LoadOrdersData();
        }

        private void LoadComboBoxData()
        {
            clientNameComboBox.ItemsSource = db.Client.ToList();
            clientAddressComboBox.ItemsSource = db.Store.ToList();
            employeeAddressComboBox.ItemsSource = db.Employees.ToList();
            statusComboBox.ItemsSource = db.Statuses.ToList();
        }

        private void LoadOrdersData()
        {
            OrdersDataGrid.ItemsSource = db.Orders.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    Orders newOrder = new Orders()
                    {
                        Order_Number = orderNumberTextBox.Text,
                        Order_Date = orderDatePicker.SelectedDate ?? DateTime.Now,
                        Client_ID = (int)clientNameComboBox.SelectedValue,
                        Employee_ID = (int)employeeAddressComboBox.SelectedValue,
                        Store_ID = (int)clientAddressComboBox.SelectedValue,
                        Status_ID = (int)statusComboBox.SelectedValue
                    };

                    db.Orders.Add(newOrder);
                    db.SaveChanges();

                    LoadOrdersData();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении нового заказа: " + ex.Message);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersDataGrid.SelectedItem != null)
                {
                    if (ValidateInputs())
                    {
                        Orders selectedOrder = (Orders)OrdersDataGrid.SelectedItem;

                        selectedOrder.Order_Number = orderNumberTextBox.Text;
                        selectedOrder.Order_Date = orderDatePicker.SelectedDate ?? DateTime.Now;
                        selectedOrder.Client_ID = (int)clientNameComboBox.SelectedValue;
                        selectedOrder.Employee_ID = (int)employeeAddressComboBox.SelectedValue;
                        selectedOrder.Store_ID = (int)clientAddressComboBox.SelectedValue;
                        selectedOrder.Status_ID = (int)statusComboBox.SelectedValue;

                        db.SaveChanges();

                        LoadOrdersData();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании заказа: " + ex.Message);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OrdersDataGrid.SelectedItem != null)
                {
                    Orders selectedOrder = (Orders)OrdersDataGrid.SelectedItem;

                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этот заказ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Orders.Remove(selectedOrder);
                        db.SaveChanges();

                        LoadOrdersData();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении заказа: " + ex.Message);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(orderNumberTextBox.Text) ||
                clientNameComboBox.SelectedIndex == -1 ||
                clientAddressComboBox.SelectedIndex == -1 ||
                employeeAddressComboBox.SelectedIndex == -1 ||
                statusComboBox.SelectedIndex == -1)
            {
                return false;
            }
            return true;
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
            orderNumberTextBox.Text = "Order Number";
            orderDatePicker.SelectedDate = DateTime.Now;
            clientNameComboBox.SelectedIndex = -1;
            clientAddressComboBox.SelectedIndex = -1;
            employeeAddressComboBox.SelectedIndex = -1;
            statusComboBox.SelectedIndex = -1;
        }
    }
}
