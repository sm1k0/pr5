using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class TrainingDataPage : Page
    {
        private prEntities db;

        public TrainingDataPage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadTrainingData();
        }

        private void LoadTrainingData()
        {
            try
            {
                TrainingDataGrid.ItemsSource = db.Training_Data.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string trainingDetails = trainingDetailsTextBox.Text.Trim();

                if (string.IsNullOrEmpty(trainingDetails))
                {
                    MessageBox.Show("Детали обучения не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Training_Data newTrainingData = new Training_Data()
                {
                    Training_Details = trainingDetails
                };

                db.Training_Data.Add(newTrainingData);
                db.SaveChanges();

                LoadTrainingData();
                trainingDetailsTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении новых данных обучения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(trainingDetailsTextBox.Text) || trainingDetailsTextBox.Text == "Training Details")
            {
                MessageBox.Show("Детали обучения не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                if (TrainingDataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите данные обучения для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Training_Data selectedTrainingData = (Training_Data)TrainingDataGrid.SelectedItem;
                selectedTrainingData.Training_Details = trainingDetailsTextBox.Text;

                db.SaveChanges();
                LoadTrainingData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании данных обучения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TrainingDataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Пожалуйста, выберите данные обучения для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Training_Data selectedTrainingData = (Training_Data)TrainingDataGrid.SelectedItem;

                db.Training_Data.Remove(selectedTrainingData);
                db.SaveChanges();

                LoadTrainingData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении данных обучения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ClearInputFields()
        {
            trainingDetailsTextBox.Text = "Training Details";
        }
    }
}
