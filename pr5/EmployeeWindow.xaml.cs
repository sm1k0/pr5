using System;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem selectedItem = (ListViewItem)MenuListView.SelectedItem;
            string selectedMenuItem = selectedItem.Content.ToString();

            switch (selectedMenuItem)
            {
                case "Product":
                    MainFrame.Content = new ProductPage();
                    break;
                case "Warehouse":
                    MainFrame.Content = new WarehousePage();
                    break;
                case "Store":
                    MainFrame.Content = new StorePage();
                    break;
                case "Statuses":
                    MainFrame.Content = new StatusesPage();
                    break;
                case "Orders":
                    MainFrame.Content = new OrdersPage();
                    break;
                case "Client":
                    MainFrame.Content = new ClientPage();
                    break;
                case "Employees":
                    MainFrame.Content = new EmployeesPage();
                    break;
                case "New_Employees":
                    MainFrame.Content = new NewEmployeesPage();
                    break;
                case "Position":
                    MainFrame.Content = new PositionPage();
                    break;
                case "Employers":
                    MainFrame.Content = new EmployersPage();
                    break;
                case "Nationality":
                    MainFrame.Content = new NationalityPage();
                    break;
                case "Training_Data":
                    MainFrame.Content = new TrainingDataPage();
                    break;
                case "User_Authentication":
                    MainFrame.Content = new UserAuthenticationPage();
                    break;
                case "Roles":
                    MainFrame.Content = new RolesPage();
                    break;
                case "Backup":
                    MainFrame.Content = new BackupPage();
                    break;
                case "Кассир":
                    MainFrame.Content = new CashierPage();
                    break;
                default:
                    MessageBox.Show("Unknown menu item selected.");
                    break;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
