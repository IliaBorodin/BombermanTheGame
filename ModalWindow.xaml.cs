using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TextFileOperations;

namespace HomeLabWPF
{
    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        public string mapName { get; private set; }
        public string path{ get; private set; }
        public ModalWindow()
        {
            InitializeComponent();
            DeleteMap.IsEnabled = false;
            LoadMapBtn.IsEnabled = false;
        }

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadMapBtn_Click(object sender, RoutedEventArgs e)
        {
             mapName = Storage.SelectedItem.ToString();
            Close();
        }

        private void Storage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Storage.SelectedItem != null)
            {
                
                LoadMapBtn.IsEnabled = true;
                DeleteMap.IsEnabled = true;
            }
            else
            {
                
                LoadMapBtn.IsEnabled = false;
                DeleteMap.IsEnabled = false;
            }
        }


        private void PopulateListBox(string filePath)
        {
            try
            {
                
                string[] allLines = File.ReadAllLines(filePath);

                
                var labels = from line in allLines
                             let startIndex = line.IndexOf("{") + 1
                             let endIndex = line.IndexOf("}")
                             where startIndex > 0 && endIndex > startIndex
                             select line.Substring(startIndex, endIndex - startIndex);

                
                foreach (var label in labels)
                {
                    Storage.Items.Add(label);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void DeleteMap_Click(object sender, RoutedEventArgs e)
        {
            HelperClass.RemoveLineByLabel(path, Storage.SelectedItem.ToString());
            Storage.Items.Clear();
            PopulateListBox(path);
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            path = HelperClass.GetTxtFilePath();
            if (path == null) return;
            Storage.Items.Clear();
            PopulateListBox(path);
        }
    }
}
