using System;
using System.Linq;
using System.Windows;

namespace pr5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            try
            {
                using (var context = new prEntities())
                {
                    var user = context.User_Authentication
                        .FirstOrDefault(u => u.Username == username && u.Password_user == password);

                    if (user != null)
                    {
                        int userRole = user.ID_Roles;

                        switch (userRole)
                        {
                            case 1:
                                ClientWindow clientWindow = new ClientWindow();
                                clientWindow.Show();
                                break;
                            case 2:
                                EmployeeWindow employeeWindow = new EmployeeWindow();
                                employeeWindow.Show();
                                break;
                            case 3:
                                EmployerWindow employerWindow = new EmployerWindow();
                                employerWindow.Show();
                                break;
                            default:
                                MessageBox.Show("Unknown role.");
                                break;
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
