using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pr5
{
    public partial class EmployeesPage : Page
    {
        private prEntities db;
        private ObservableCollection<Employees> employeesList;

        public EmployeesPage()
        {
            InitializeComponent();
            db = new prEntities();
            LoadComboBoxData();
            LoadEmployeesData();
        }

        private void LoadComboBoxData()
        {
            positionComboBox.ItemsSource = db.Position.ToList();

            addressComboBox.ItemsSource = db.Store.ToList();
        }

        private void LoadEmployeesData()
        {
            employeesList = new ObservableCollection<Employees>(db.Employees.ToList());
            EmployeesDataGrid.ItemsSource = employeesList;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (db.User_Authentication.Any(u => u.Username == text9.Text))
                {
                    MessageBox.Show("Имя пользователя уже существует. Пожалуйста, выберите другое.");
                    return;
                }

                if (!IsPhoneNumberValid(text3.Text))
                {
                    MessageBox.Show("Некорректный формат номера телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                

                int employeeRoleId = 2;

                User_Authentication newUserAuth = new User_Authentication()
                {
                    Username = text9.Text,
                    Password_user = passwordBox.Text,
                    ID_Roles = employeeRoleId
                };

                db.User_Authentication.Add(newUserAuth);
                db.SaveChanges();

                Employees newEmployee = new Employees()
                {
                    Last_Name = text1.Text,
                    First_Name = text2.Text,
                    Middle_Name = text3.Text,
                    Phone_Number = text4.Text,
                    Position_ID = ((Position)positionComboBox.SelectedItem).Position_ID,
                    Store_ID = ((Store)addressComboBox.SelectedItem).Store_ID,
                    Authorization_ID = newUserAuth.Authorization_ID,
                    ID_Roles = employeeRoleId 
                };

                db.Employees.Add(newEmployee);
                db.SaveChanges();

                LoadEmployeesData();
                ClearTextBoxes();
            }
            catch (DbUpdateException ex)
            {
                Exception rootCause = ex;
                while (rootCause.InnerException != null)
                {
                    rootCause = rootCause.InnerException;
                }

                MessageBox.Show("Ошибка добавления сотрудника: " + rootCause.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                Employees selectedEmployee = (Employees)EmployeesDataGrid.SelectedItem;
                selectedEmployee.Last_Name = text1.Text;
                selectedEmployee.First_Name = text2.Text;
                selectedEmployee.Middle_Name = text3.Text;
                selectedEmployee.Phone_Number = text4.Text;
                selectedEmployee.Position_ID = ((Position)positionComboBox.SelectedItem).Position_ID;
                selectedEmployee.Store_ID = ((Store)addressComboBox.SelectedItem).Store_ID;

                db.SaveChanges();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedItem != null)
            {
                Employees selectedEmployee = (Employees)EmployeesDataGrid.SelectedItem;

                var relatedOrders = db.Orders.Where(o => o.Employee_ID == selectedEmployee.Employee_ID).ToList();
                if (relatedOrders.Count > 0)
                {
                    MessageBox.Show("Невозможно удалить сотрудника, так как есть связанные заказы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Employees.Remove(selectedEmployee);
                db.SaveChanges();
                employeesList.Remove(selectedEmployee);
            }
        }

        private void ClearTextBoxes()
        {
            text1.Text = "";
            text2.Text = "";
            text3.Text = "";
            text4.Text = "";
            positionComboBox.SelectedItem = null;
            addressComboBox.SelectedItem = null;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                ClearTextBoxes();
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

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Password == passwordBox.Tag.ToString())
                {
                    passwordBox.Password = "";
                }
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    passwordBox.Password = passwordBox.Tag.ToString();
                }
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\d+$");
        }
    }
}
