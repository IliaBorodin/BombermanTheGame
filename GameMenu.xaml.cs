using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace HomeLabWPF
{

    /// <summary>
    /// Логика взаимодействия для GameMenu.xaml
    /// </summary>
    public partial class GameMenu : Page
    {
        private Window window;
        public Difficulty Dif_menu { get; private set; }
        public string mapName { get; private set; }
        public string path { get; private set; }
        private ModalWindow modal;
        public GameMenu(Window _window)
        {
            
            Dif_menu = new Difficulty(this);
            window = _window;
            InitializeComponent();
             
        }

        private void CreateGameBtn_Click(object sender, RoutedEventArgs e)
        {
            modal = new ModalWindow();
            modal.Closed += ModalWindow_Closed;
            modal.ShowDialog();
            NavigationService.Navigate(new Game(window, this, path, mapName));
          
        }

        

        private void ModalWindow_Closed(object sender, EventArgs e)
        {
            ModalWindow customModalWindow = (ModalWindow)sender;
            mapName = customModalWindow.mapName;
            path = customModalWindow.path;
            customModalWindow.Closed -= ModalWindow_Closed;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DifBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Dif_menu);
        }
    }
}
