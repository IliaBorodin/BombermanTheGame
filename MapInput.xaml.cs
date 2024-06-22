using System.Windows;
using TextFileOperations;

namespace HomeLabWPF
{
    /// <summary>
    /// Логика взаимодействия для MapInput.xaml
    /// </summary>
    public partial class MapInput : Window
    {
        
        public string UserInput { get; private set; }
        public string path { get; private set; }
        public MapInput()
        {
            InitializeComponent();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            UserInput = cardNameTextBox.Text;
            path = HelperClass.GetTxtFilePath();
            if (path == null) return;
            Close();
        }
    }
}
