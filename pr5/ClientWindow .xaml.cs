using System;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = MenuListView.SelectedItem as ListViewItem;

            if (selectedItem != null)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Orders":
                        int currentUserId = GetCurrentUserId(); 
                        MainFrame.Navigate(new OrdersPage1(currentUserId));
                        break;
                    case "Store":
                        MainFrame.Navigate(new StorePage1());
                        break;
                    case "Product":
                        MainFrame.Navigate(new ProductPage1());
                        break;
                    default:
                        MessageBox.Show("Unknown menu item selected.");
                        break;
                }
            }
        }

        private int GetCurrentUserId()
        {
            return 123; 
        }



        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
