using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class NewEmployeesPage : Page
    {
        private prEntities db;

        public NewEmployeesPage()
        {
            InitializeComponent();
            db = new prEntities();
            LoadComboBoxData();
            LoadNewEmployeesData();
        }

        private void LoadComboBoxData()
        {
            employerComboBox.ItemsSource = db.Employers.ToList();
            positionComboBox.ItemsSource = db.Position.ToList();
            nationalityComboBox.ItemsSource = db.Nationality.ToList();
            trainingDataComboBox.ItemsSource = db.Training_Data.ToList();
        }

        private void LoadNewEmployeesData()
        {
            NewEmployeesDataGrid.ItemsSource = db.New_Employees.ToList();
        }

        private bool IsEmployeeDataValid()
        {
            if (string.IsNullOrWhiteSpace(text1.Text) || text1.Text == "Last Name" ||
                string.IsNullOrWhiteSpace(text2.Text) || text2.Text == "First Name" ||
                string.IsNullOrWhiteSpace(text3.Text) || text3.Text == "Middle Name" ||
                datePicker.SelectedDate == null ||
                employerComboBox.SelectedItem == null ||
                positionComboBox.SelectedItem == null ||
                nationalityComboBox.SelectedItem == null ||
                trainingDataComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmployeeDataValid())
            {
                return;
            }

            try
            {
                New_Employees newEmployee = new New_Employees()
                {
                    Employee_Last_Name = text1.Text,
                    Employee_First_Name = text2.Text,
                    Employee_Middle_Name = text3.Text,
                    Date_of_Birth = datePicker.SelectedDate ?? DateTime.Now,
                    Employer_ID = (int)employerComboBox.SelectedValue,
                    Position_ID = (int)positionComboBox.SelectedValue,
                    Nationality_ID = (int)nationalityComboBox.SelectedValue,
                    Training_Data_ID = (int)trainingDataComboBox.SelectedValue
                };

                db.New_Employees.Add(newEmployee);
                db.SaveChanges();

                LoadNewEmployeesData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmployeeDataValid() || NewEmployeesDataGrid.SelectedItem == null)
            {
                return;
            }

            try
            {
                New_Employees selectedEmployee = (New_Employees)NewEmployeesDataGrid.SelectedItem;

                selectedEmployee.Employee_Last_Name = text1.Text;
                selectedEmployee.Employee_First_Name = text2.Text;
                selectedEmployee.Employee_Middle_Name = text3.Text;
                selectedEmployee.Date_of_Birth = datePicker.SelectedDate ?? DateTime.Now;
                selectedEmployee.Employer_ID = (int)employerComboBox.SelectedValue;
                selectedEmployee.Position_ID = (int)positionComboBox.SelectedValue;
                selectedEmployee.Nationality_ID = (int)nationalityComboBox.SelectedValue;
                selectedEmployee.Training_Data_ID = (int)trainingDataComboBox.SelectedValue;

                db.SaveChanges();

                LoadNewEmployeesData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (NewEmployeesDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.", "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                New_Employees selectedEmployee = (New_Employees)NewEmployeesDataGrid.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.New_Employees.Remove(selectedEmployee);
                    db.SaveChanges();

                    LoadNewEmployeesData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении сотрудника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            text1.Text = "Last Name";
            text2.Text = "First Name";
            text3.Text = "Middle Name";
            datePicker.SelectedDate = DateTime.Now;
        }
    }
}
