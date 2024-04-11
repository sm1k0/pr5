using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class CashierPage : Page
    {
        private prEntities db;
        private List<Product> selectedProducts = new List<Product>();

        public CashierPage()
        {
            InitializeComponent();
            db = new prEntities();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsDataGrid.ItemsSource = db.Product.ToList();
        }

        private void AddToReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem != null)
            {
                Product selectedProduct = (Product)ProductsDataGrid.SelectedItem;
                selectedProducts.Add(selectedProduct);
                ReceiptListBox.Items.Add($"{selectedProduct.Product_Name} - {selectedProduct.Price}");
                CalculateTotalAmount();
            }
            else
            {
                MessageBox.Show("Выберите товар для добавления в чек.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CalculateTotalAmount()
        {
            decimal totalAmount = selectedProducts.Sum(p => p.Price);
            TotalAmountTextBlock.Text = totalAmount.ToString();
        }

        private void GenerateReceipt_Click(object sender, RoutedEventArgs e)
        {
            decimal totalAmount = selectedProducts.Sum(p => p.Price);
            decimal amountPaid;
            if (!decimal.TryParse(AmountPaidTextBox.Text, out amountPaid))
            {
                MessageBox.Show("Введите корректную сумму оплаты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal change = amountPaid - totalAmount;

            string receipt = $"Кассовый чек №{GetReceiptID()}\n\n";

            foreach (Product product in selectedProducts)
            {
                receipt += $"{product.Product_Name}\t-\t{product.Price}\n";
            }

            receipt += $"\nИтого к оплате: {totalAmount}\nВнесено: {amountPaid}\nСдача: {change}";

            SaveReceiptToFile(receipt);
            ClearReceipt();
        }

        private int GetReceiptID()
        {
            Random rand = new Random();
            return rand.Next(1000, 9999);
        }

        private void SaveReceiptToFile(string receipt)
        {
            string filePath = @"D:\Desktop\2 курс\практика\receipt.txt";
            File.WriteAllText(filePath, receipt);
            MessageBox.Show($"Чек сохранен в файл {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearReceipt()
        {
            selectedProducts.Clear();
            ReceiptListBox.Items.Clear();
            TotalAmountTextBlock.Text = "";
            AmountPaidTextBox.Text = "0";
        }
    }
}
