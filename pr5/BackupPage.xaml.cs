using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class BackupPage : Page
    {
        private prEntities db;

        public BackupPage()
        {
            InitializeComponent();

            db = new prEntities();
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string backupPath = "D:\\Desktop\\2 курс\\практикаdatabase_backup.bak";

                using (SqlConnection connection = new SqlConnection(db.Database.Connection.ConnectionString))
                {
                    connection.Open();

                    string query = $"BACKUP DATABASE {connection.Database} TO DISK = '{backupPath}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Бэкап базы данных успешно создан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании бэкапа базы данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string jsonFilePath = "D:\\Desktop\\2 курс\\практика\\data.json";

                using (StreamReader file = File.OpenText(jsonFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var clientList = (List<Client>)serializer.Deserialize(file, typeof(List<Client>));

                    foreach (var client in clientList)
                    {
                        db.Client.Add(client);
                    }

                    var employeeList = (List<Employees>)serializer.Deserialize(file, typeof(List<Employees>));

                    foreach (var employee in employeeList)
                    {
                        db.Employees.Add(employee);
                    }

                    db.SaveChanges();
                }

                MessageBox.Show("Данные успешно импортированы из JSON файла.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при импорте данных из JSON файла: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
