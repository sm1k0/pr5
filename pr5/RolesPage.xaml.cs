using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class RolesPage : Page
    {
        private prEntities db;

        public RolesPage()
        {
            InitializeComponent();

            db = new prEntities();

            LoadRolesData();
        }

        private void LoadRolesData()
        {
            RolesDataGrid.ItemsSource = db.Roles.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(RoleNameTextBox.Text))
                {
                    var newRole = new Roles()
                    {
                        Roless = RoleNameTextBox.Text
                    };

                    db.Roles.Add(newRole);
                    db.SaveChanges();

                    LoadRolesData();
                }
                else
                {
                    MessageBox.Show("Введите название роли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении роли: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RolesDataGrid.SelectedItem != null)
            {
                try
                {
                    Roles selectedRole = (Roles)RolesDataGrid.SelectedItem;
                    selectedRole.Roless = RoleNameTextBox.Text;

                    db.SaveChanges();

                    LoadRolesData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при редактировании роли: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите роль для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RolesDataGrid.SelectedItem != null)
            {
                try
                {
                    Roles selectedRole = (Roles)RolesDataGrid.SelectedItem;

                    db.Roles.Remove(selectedRole);
                    db.SaveChanges();

                    LoadRolesData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении роли: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите роль для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
