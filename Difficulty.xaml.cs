using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace HomeLabWPF
{
    /// <summary>
    /// Логика взаимодействия для Difficulty.xaml
    /// </summary>
    public partial class Difficulty : Page
    {
        private GameMenu game_menu;
        public int level { get; private set; }
        public Difficulty(GameMenu menu)
        {
            game_menu = menu;
            InitializeComponent();
            lightLevelBtn.Background = new SolidColorBrush(Colors.Green);
            mediumLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            hardLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            level = 1;
           
        }

        private void lightLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(game_menu);
            lightLevelBtn.Background = new SolidColorBrush(Colors.Green);
            mediumLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            hardLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            level = 1;
        }

        private void mediumLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(game_menu);
            lightLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            mediumLevelBtn.Background = new SolidColorBrush(Colors.Green);
            hardLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            level = 2;
        }

        private void hardLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(game_menu);
            lightLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            mediumLevelBtn.Background = new SolidColorBrush(Colors.Gray);
            hardLevelBtn.Background = new SolidColorBrush(Colors.Green);
            level = 3;
        }
    }
}
