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
    /// Логика взаимодействия для ProductPage1.xaml
    /// </summary>
    public partial class ProductPage1 : Page
    {
        private prEntities db;
        public ProductPage1()
        {
            InitializeComponent();

            db = new prEntities();
            LoadProductData();
        }

        private void LoadProductData()
        {
            try
            {
                ProductData.ItemsSource = db.Product.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}