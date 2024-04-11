using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace pr5
{
    public partial class PositionPage : Page
    {
        private prEntities db;

        public PositionPage()
        {
            InitializeComponent();

            db = new prEntities();
            LoadPositionsData();
        }

        private void LoadPositionsData()
        {
            PositionDataGrid.ItemsSource = db.Position.ToList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(positionTitleTextBox.Text) || scheduleComboBox.Text == null)
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Position newPosition = new Position()
                {
                    Position_Title = positionTitleTextBox.Text,
                    Schedule = scheduleComboBox.Text
                };

                db.Position.Add(newPosition);
                db.SaveChanges();

                LoadPositionsData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.AppendLine("Ошибка при добавлении новой должности:");
                sb.AppendLine(ex.Message);

                if (ex is DbEntityValidationException validationException)
                {
                    foreach (var eve in validationException.EntityValidationErrors)
                    {
                        sb.AppendLine($"Сущность типа \"{eve.Entry.Entity.GetType().Name}\" в состоянии \"{eve.Entry.State}\" имеет следующие ошибки валидации:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine($"- Свойство: \"{ve.PropertyName}\", Ошибка: \"{ve.ErrorMessage}\"");
                        }
                    }
                }

                MessageBox.Show(sb.ToString());
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PositionDataGrid.SelectedItem != null)
                {
                    Position selectedPosition = (Position)PositionDataGrid.SelectedItem;

                    if (string.IsNullOrWhiteSpace(positionTitleTextBox.Text) || scheduleComboBox.Text == null)
                    {
                        MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    selectedPosition.Position_Title = positionTitleTextBox.Text;
                    selectedPosition.Schedule = scheduleComboBox.Text;

                    db.SaveChanges();

                    LoadPositionsData();
                    ClearInputFields();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите должность для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании должности: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PositionDataGrid.SelectedItem != null)
                {
                    Position selectedPosition = (Position)PositionDataGrid.SelectedItem;

                    MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту должность?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        db.Position.Remove(selectedPosition);
                        db.SaveChanges();

                        LoadPositionsData();
                        ClearInputFields();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите должность для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении должности: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag.ToString();
            }
        }

        private void ClearInputFields()
        {
            positionTitleTextBox.Text = "Position Title";
            scheduleComboBox.Text = "";
        }
    }
}
