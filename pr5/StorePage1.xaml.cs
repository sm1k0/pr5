using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr5
{
    /// <summary>
    /// Логика взаимодействия для StorePage1.xaml
    /// </summary>
    public partial class StorePage1 : Page
    {
        private prEntities db;
        private Random random;

        public StorePage1()
        {
            InitializeComponent();

            db = new prEntities();
            random = new Random();
            LoadStoreData();
        }

        private void LoadStoreData()
        {
            try
            {
                StoreDataGrid.ItemsSource = db.Store.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}