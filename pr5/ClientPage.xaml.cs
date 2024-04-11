using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pr5
{
    public partial class ClientPage : Page
    {
        private prEntities db;

        public ClientPage()
        {
            InitializeComponent();
            db = new prEntities();

            LoadClientData();
            ClientDataGrid.SelectionChanged += ClientDataGrid_SelectionChanged;
            text1.KeyUp += TextBox_KeyUp;
            text2.KeyUp += TextBox_KeyUp;
            text3.KeyUp += TextBox_KeyUp;
            text4.KeyUp += TextBox_KeyUp;
            text5.KeyUp += TextBox_KeyUp;
            text6.KeyUp += TextBox_KeyUp;
            text7.KeyUp += TextBox_KeyUp;
            passwordBox.KeyUp += TextBox_KeyUp;

            text4.PreviewTextInput += Text4_PreviewTextInput;
            text5.LostFocus += Text5_LostFocus;
        }

        private void Text5_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(text5.Text, @"^.+@.+\..+$"))
            {
                MessageBox.Show("Неверный формат электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                text5.Focus();
            }
        }

        private void Text4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, @"^\d+$");
        }

        private void LoadClientData()
        {
            var clients = db.Client.ToList();
            ObservableCollection<Client> clientList = new ObservableCollection<Client>(clients);
            ClientDataGrid.ItemsSource = clientList;
        }

        private void ClientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client selectedClient = (Client)ClientDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                text1.Text = selectedClient.Last_Name;
                text2.Text = selectedClient.First_Name;
                text3.Text = selectedClient.Middle_Name;
                text4.Text = selectedClient.Phone_Number;
                text5.Text = selectedClient.Email;
                text6.Text = selectedClient.Address_Client;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (db.User_Authentication.Any(u => u.Username == text7.Text))
                {
                    MessageBox.Show("Имя пользователя уже существует. Пожалуйста, выберите другое.");
                    return;
                }

                int clientId = 1;

                User_Authentication newUserAuth = new User_Authentication()
                {
                    Username = text7.Text,
                    Password_user = passwordBox.Text,
                    ID_Roles = clientId
                };

                db.User_Authentication.Add(newUserAuth);
                db.SaveChanges();

                Client newClient = new Client()
                {
                    Last_Name = text1.Text,
                    First_Name = text2.Text,
                    Middle_Name = text3.Text,
                    Phone_Number = text4.Text,
                    Email = text5.Text,
                    Address_Client = text6.Text,
                    Authorization_ID = newUserAuth.Authorization_ID,
                    ID_Roles = clientId
                };

                db.Client.Add(newClient);
                db.SaveChanges();

                LoadClientData();
                ClearTextBoxes();
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

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Client selectedClient = (Client)ClientDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                try
                {
                    selectedClient.Last_Name = text1.Text;
                    selectedClient.First_Name = text2.Text;
                    selectedClient.Middle_Name = text3.Text;
                    selectedClient.Phone_Number = text4.Text;
                    selectedClient.Email = text5.Text;
                    selectedClient.Address_Client = text6.Text;

                    db.SaveChanges();

                    LoadClientData();
                    ClearTextBoxes();
                }
                catch (DbUpdateException ex)
                {
                    Exception rootCause = ex;
                    while (rootCause.InnerException != null)
                    {
                        rootCause = rootCause.InnerException;
                    }

                    MessageBox.Show("Ошибка редактирования клиента: " + rootCause.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                DeleteSelectedClient();
            }
        }

        private void DeleteSelectedClient()
        {
            Client selectedClient = (Client)ClientDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                try
                {
                    db.Client.Remove(selectedClient);
                    db.SaveChanges();
                    LoadClientData();
                    ClearTextBoxes();
                }
                catch (DbUpdateException ex)
                {
                    Exception rootCause = ex;
                    while (rootCause.InnerException != null)
                    {
                        rootCause = rootCause.InnerException;
                    }

                    MessageBox.Show("Ошибка удаления клиента: " + rootCause.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Client selectedClient = (Client)ClientDataGrid.SelectedItem;

            if (selectedClient != null)
            {
                if (db.Orders.Any(o => o.Client_ID == selectedClient.Client_ID))
                {
                    var result = MessageBox.Show("У этого клиента есть связанные заказы. Хотите также удалить их?", "Подтверждение", MessageBoxButton.YesNoCancel);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            var ordersToDelete = db.Orders.Where(o => o.Client_ID == selectedClient.Client_ID).ToList();
                            foreach (var order in ordersToDelete)
                            {
                                db.Orders.Remove(order);
                            }
                            break;
                        case MessageBoxResult.No:
                        case MessageBoxResult.Cancel:
                            return;
                    }
                }

                try
                {
                    db.Client.Remove(selectedClient);
                    db.SaveChanges();
                    LoadClientData();
                    ClearTextBoxes();
                }
                catch (DbUpdateException ex)
                {
                    Exception rootCause = ex;
                    while (rootCause.InnerException != null)
                    {
                        rootCause = rootCause.InnerException;
                    }

                    MessageBox.Show("Ошибка удаления клиента: " + rootCause.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
        private void ClearTextBoxes()
        {
            text1.Text = string.Empty;
            text2.Text = string.Empty;
            text3.Text = string.Empty;
            text4.Text = string.Empty;
            text5.Text = string.Empty;
            text6.Text = string.Empty;
            text7.Text = string.Empty;
            passwordBox.Text = string.Empty;
        }
    }
}
