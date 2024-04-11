using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class EmployerWindow : Window
    {
        public EmployerWindow()
        {
            InitializeComponent();
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedItem = e.AddedItems[0] as ListViewItem;

                if (selectedItem != null)
                {
                    string selectedContent = selectedItem.Content.ToString();

                    switch (selectedContent)
                    {
                        case "Training Data":
                            MainFrame.Navigate(new OrdersPage());
                            break;
                        case "Nationality":
                            MainFrame.Navigate(new NationalityPage());
                            break;
                        case "Employers":
                            MainFrame.Navigate(new EmployersPage());
                            break;
                        case "Position":
                            MainFrame.Navigate(new PositionPage());
                            break;
                        case "New Employees":
                            MainFrame.Navigate(new NewEmployeesPage());
                            break;
                        default:
                            MessageBox.Show("Unknown menu item selected.");
                            break;
                    }
                }
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
