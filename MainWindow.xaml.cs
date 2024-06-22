using System.Windows;
using System.Windows.Input;


namespace HomeLabWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GameMenu gameMenu;
             gameMenu = new GameMenu(this);
            
            MainFrame.Content =gameMenu;
        }

        private void MainWindow1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
