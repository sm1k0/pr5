using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class EmployersPage : Page
    {
        private prEntities db;
        private Employers selectedEditingEmployer;

        public EmployersPage()
        {
            InitializeComponent();
            db = new prEntities();
            LoadComboBoxData();
            LoadEmployersData();
        }

        private void LoadComboBoxData()
        {
            positionComboBox.ItemsSource = db.Position.ToList();
        }

        private void LoadEmployersData()
        {
            EmployersDataGrid.ItemsSource = db.Employers.ToList();
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

                int employerRoleId = 3;

                User_Authentication newUserAuth = new User_Authentication()
                {
                    Username = text9.Text,
                    Password_user = passwordBox.Text,
                    ID_Roles = employerRoleId
                };

                db.User_Authentication.Add(newUserAuth);
                db.SaveChanges();

                Employers newEmployer = new Employers()
                {
                    Employer_Name = text1.Text,
                    Employee_Last_Name = text2.Text,
                    Employee_Middle_Name = text3.Text,
                    Position_ID = ((Position)positionComboBox.SelectedItem).Position_ID,
                    Authorization_ID = newUserAuth.Authorization_ID,
                    ID_Roles = employerRoleId
                };

                db.Employers.Add(newEmployer);
                db.SaveChanges();

                LoadEmployersData();
                ClearTextBoxes();
            }
            catch (DbUpdateException ex)
            {
                Exception rootCause = ex;
                while (rootCause.InnerException != null)
                {
                    rootCause = rootCause.InnerException;
                }

                MessageBox.Show("Ошибка добавления работника: " + rootCause.Message);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (EmployersDataGrid.SelectedItem != null)
            {
                Employers selectedEmployer = (Employers)EmployersDataGrid.SelectedItem;

                selectedEmployer.Employer_Name = text1.Text;
                selectedEmployer.Employee_Last_Name = text2.Text;
                selectedEmployer.Employee_Middle_Name = text3.Text;

                selectedEmployer.User_Authentication.Username = text9.Text;
                selectedEmployer.User_Authentication.Password_user = passwordBox.Text;

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены.");
                    LoadEmployersData(); 
                    ClearTextBoxes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении изменений: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите работника для редактирования.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (EmployersDataGrid.SelectedItem != null)
            {
                Employers selectedEmployer = (Employers)EmployersDataGrid.SelectedItem;


                db.Employers.Remove(selectedEmployer);
                db.SaveChanges();

                LoadEmployersData();
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

        private void EmployersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployersDataGrid.SelectedItem != null)
            {
                Employers selectedEmployer = (Employers)EmployersDataGrid.SelectedItem;

                text1.Text = selectedEmployer.Employer_Name;
                text2.Text = selectedEmployer.Employee_Last_Name;
                text3.Text = selectedEmployer.Employee_Middle_Name;

                positionComboBox.SelectedItem = selectedEmployer.Position;

                passwordBox.Text = "";
            }
        }

        private void ClearTextBoxes()
        {
            text1.Text = "";
            text2.Text = "";
            text3.Text = "";
            text9.Text = "";
            passwordBox.Text = "";
        }
    }
}
