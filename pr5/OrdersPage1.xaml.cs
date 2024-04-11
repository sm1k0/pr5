using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class OrdersPage1 : Page
    {
        private prEntities db;
        private int currentUserId;

        public OrdersPage1(int userId)
        {
            InitializeComponent();

            db = new prEntities();
            currentUserId = userId;

            LoadOrdersData();
        }

        private void LoadOrdersData()
        {
            try
            {
                OrdersDataGrid.ItemsSource = db.Orders.Where(o => o.Client_ID == currentUserId).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заказов: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetRandomStoreId()
        {
            var stores = db.Store.ToList();
            Random rand = new Random();
            return stores[rand.Next(0, stores.Count)].Store_ID;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {
                    Random rand = new Random();
                    var newOrder = new Orders()
                    {
                        Order_Number = rand.Next(10000, 99999).ToString(),
                        Order_Date = orderDatePicker.SelectedDate ?? DateTime.Now,
                        Client_ID = currentUserId,
                        Employee_ID = GetRandomEmployeeId(),
                        Store_ID = GetRandomStoreId(),
                        Status_ID = 1
                    };

                    db.Orders.Add(newOrder);
                    db.SaveChanges();

                    LoadOrdersData();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (DbUpdateException ex)
            {
                Exception rootCause = ex;
                while (rootCause.InnerException != null)
                {
                    rootCause = rootCause.InnerException;
                }

                MessageBox.Show("Ошибка добавления клиента: " + rootCause.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetRandomEmployeeId()
        {
            var employees = db.Employees.ToList();
            Random rand = new Random();
            return employees[rand.Next(0, employees.Count)].Employee_ID;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem != null)
            {
                Orders selectedOrder = (Orders)OrdersDataGrid.SelectedItem;
                selectedOrder.Order_Date = orderDatePicker.SelectedDate ?? DateTime.Now;

                db.SaveChanges();

                LoadOrdersData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
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

        private bool ValidateInputs()
        {
            return orderDatePicker.SelectedDate.HasValue;
        }

    }
}
