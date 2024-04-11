using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class UserAuthenticationPage : Page
    {
        private prEntities db;

        public UserAuthenticationPage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadComboBoxData();
            LoadUserData();
        }

        private void LoadComboBoxData()
        {
            try
            {
                roleComboBox.ItemsSource = db.Roles.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUserData()
        {
            try
            {
                UserAuthenticationDataGrid.ItemsSource = db.User_Authentication.ToList();
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
                string username = usernameTextBox.Text.Trim();
                string password = passwordBox.Password.Trim();
                Roles selectedRole = (Roles)roleComboBox.SelectedItem;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || selectedRole == null)
                {
                    MessageBox.Show("Имя пользователя, пароль и роль не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (db.User_Authentication.Any(u => u.Username == username))
                {
                    MessageBox.Show("Имя пользователя уже существует. Пожалуйста, выберите другое.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                User_Authentication newUser = new User_Authentication()
                {
                    Username = username,
                    Password_user = password,
                    ID_Roles = selectedRole.Roles_ID
                };

                db.User_Authentication.Add(newUser);
                db.SaveChanges();

                LoadUserData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении нового пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User_Authentication selectedUser = (User_Authentication)UserAuthenticationDataGrid.SelectedItem;

                if (selectedUser == null)
                {
                    MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string username = usernameTextBox.Text.Trim();
                string password = passwordBox.Password.Trim();
                Roles selectedRole = (Roles)roleComboBox.SelectedItem;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || selectedRole == null)
                {
                    MessageBox.Show("Имя пользователя, пароль и роль не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (db.User_Authentication.Any(u => u.Username == username && u.Authorization_ID != selectedUser.Authorization_ID))
                {
                    MessageBox.Show("Имя пользователя уже существует. Пожалуйста, выберите другое.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                selectedUser.Username = username;
                selectedUser.Password_user = password;
                selectedUser.ID_Roles = selectedRole.Roles_ID;

                db.SaveChanges();

                LoadUserData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User_Authentication selectedUser = (User_Authentication)UserAuthenticationDataGrid.SelectedItem;

                if (selectedUser == null)
                {
                    MessageBox.Show("Пожалуйста, выберите пользователя для удаления.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    db.User_Authentication.Remove(selectedUser);
                    db.SaveChanges();

                    LoadUserData();
                    ClearInputFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Authenticate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = usernameTextBox.Text.Trim();

                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Имя пользователя не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (db.User_Authentication.Any(u => u.Username == username))
                {
                    MessageBox.Show("Имя пользователя доступно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Имя пользователя недоступно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при аутентификации пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ClearInputFields()
        {
            usernameTextBox.Text = "Username";
            passwordBox.Password = "Password";
            roleComboBox.SelectedIndex = -1;
        }
    }
}
