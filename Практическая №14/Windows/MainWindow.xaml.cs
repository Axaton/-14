using Microsoft.Win32;
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
using LibraryMatrix;
using System.IO;

namespace Практическая__14
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            loginWindow.ShowDialog();
            if (loginWindow.IsRightPassword == true)
            {
                if (File.Exists("config.ini"))
                {
                    using (StreamReader reader = new StreamReader("config.ini"))
                    {
                        _myMatrix = new MyMatrix(int.Parse(reader.ReadLine()), int.Parse(reader.ReadLine()));
                        InitializeComponent();
                        dataGrid.ItemsSource = _myMatrix.ToDataTable().DefaultView;
                    }
                }
                else MessageBox.Show("Файла конфигурации нет");
            }
            else
            {
                Close();
            }
        }

        private MyMatrix _myMatrix;

        LoginWindow loginWindow = new LoginWindow();
        
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            int resultMultiply = int.MaxValue;
            int resultindex = 0;

            for (int i = 0; i < _myMatrix.ColumnLength; i++)
            {
                int multiplication = 1;
                for (int j = 0; j < _myMatrix.RowLength; j++)
                {
                    multiplication *= _myMatrix[j, i];
                }
                if (resultMultiply > multiplication)
                {
                    resultMultiply = multiplication;
                    resultindex = i;
                }
            }
            Columnindex.Text = resultindex.ToString();
            minMultiplication.Text = resultMultiply.ToString();
        }

        private void FillMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(row.Text) || String.IsNullOrEmpty(column.Text))
            {
                MessageBox.Show("Введите размер матрицы");
            }
            else
            {
                _myMatrix = new MyMatrix(int.Parse(row.Text), int.Parse(column.Text));
                sizeRow.Text = $"Строк {row.Text}";
                sizeColumn.Text = $"Столбцов {column.Text}";
                _myMatrix.FillMatrix();
                dataGrid.ItemsSource = _myMatrix.ToDataTable().DefaultView;
            }
        }

        private void SaveArray_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource == null)
            {
                MessageBox.Show("Заполните матрицу", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Title = "Экспорт массива";

            if (saveFileDialog.ShowDialog() == true)
            {
                _myMatrix.Export(saveFileDialog.FileName);
            }
            Clear();
        }

        private void OpenArray_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Импорт массива";

            if (openFileDialog.ShowDialog() == true)
            {
                _myMatrix.Import(openFileDialog.FileName);
            }
            dataGrid.ItemsSource = _myMatrix.ToDataTable().DefaultView;
        }

        private void Clear()
        {
            dataGrid.ItemsSource = null;
            row.Clear();
            column.Clear();
            minMultiplication.Clear();
            Columnindex.Clear();
            sizeColumn.Clear();
            sizeRow.Clear();
            selectedCell.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void CellIndex_Click(object sender, MouseEventArgs e)
        {
            selectedCell.Text = $"[{dataGrid.Items.IndexOf(dataGrid.CurrentItem)};" +
                $"{dataGrid.CurrentColumn.DisplayIndex}]";
        }

        private void ChangeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            minMultiplication.Clear();
            Columnindex.Clear();
        }

        private void TaskInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Добавить форму «Пароль» и организовать авторизацию в программе.  " +
               "Создать форму «Настройка». Назначение формы – изменение размера таблицы." +
               "При установке размера таблицы сохранять настройки в файл конфигурации «config.ini».", "Задание");
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void DeveloperInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" Харенко Кирилл  ИСП-34", "Разработчик", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Завершить работу программы?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) Close();
        }
    }
}
