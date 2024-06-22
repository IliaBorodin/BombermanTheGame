using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TextFileOperations;

namespace HomeLabWPF
{

    public delegate void updateInfo(int param1, int param2);
   
    struct TimerGameOver
    {
        public DispatcherTimer timer;
        public bool flag;
    }
    public delegate void deClear();
    public partial class Game : Page
    {
       private BattleField board;
       private DispatcherTimer timer;
       private TimerGameOver timerGameOver;
        private string path;
        private Window window;
        private GameMenu game_menu;
        private TextBlock label1;
        private TextBlock label2;
        private MapInput modal;
        private string mapName;

        public Game(Window _window, GameMenu menu, string _path, string _mapName = "random")
        {
            path = _path;
            mapName = _mapName;
            game_menu = menu;
            InitializeComponent();
             window = _window;
            window.KeyDown += HandleKeyPress;
            
            NewGame();
            
            
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (timerGameOver.flag == true)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        board.MoveHero(Arrows.left); break;
                    case Key.Right:
                        board.MoveHero(Arrows.right); break;
                    case Key.Up:
                        board.MoveHero(Arrows.up); break;
                    case Key.Down:
                        board.MoveHero(Arrows.down); break;
                    case Key.Space:
                        board.PutBomb(); break;

                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            board.ClearFire();
            timer.Stop();
        }
        private void timerGameOver_Tick(object sender, EventArgs e)
        {
            if (board.GameOver())
            {
                timerGameOver.timer.Stop();
                timerGameOver.flag = false;
                MessageBoxResult dialog = MessageBox.Show("Игра окончена", "Конец игры!!!", MessageBoxButton.OK, MessageBoxImage.Information);
                if (dialog == MessageBoxResult.OK)
                    CloseGame();
                


            }
        }

        private void CloseGame()
        {
            
            Application.Current.Shutdown();



        }

        private void NewGame()
        {
            timer = new DispatcherTimer();
            timerGameOver.flag = false;
            timerGameOver.timer = new DispatcherTimer();
            timerGameOver.timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timerGameOver.timer.Tick += timerGameOver_Tick;
            drawingInfoBoard();
            CreateBattleField();
           
            
            timerGameOver.timer.Start();
            timerGameOver.flag = true;
            
        }

        private void CreateBattleField()
        {

            int[] array = HelperClass.ProcessLineByLabel(path, mapName);
            if(mapName == "random" || array.Length==0)
            board = new BattleField(Canvas, StartClear, game_menu.Dif_menu.level, UpdateInfo);
            else
                board = new BattleField(Canvas, StartClear, game_menu.Dif_menu.level, UpdateInfo,array);
        }

        private void StartClear()
        {
            timer.Start();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseGame();
           
        }

        private void GamePage_KeyDown(object sender, KeyEventArgs e)
        {
            if (timerGameOver.flag == true)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        board.MoveHero(Arrows.left); break;
                    case Key.Right:
                        board.MoveHero(Arrows.right); break;
                    case Key.Up:
                        board.MoveHero(Arrows.up); break;
                    case Key.Down:
                        board.MoveHero(Arrows.down); break;
                    case Key.Space:
                        board.PutBomb(); break;

                }
            }
        }


        private void UpdateInfo(int bomb_count, int enemy_count)
        {
            
            label1.Text = bomb_count.ToString();
            label2.Text = enemy_count.ToString();
        }

        private void drawingInfoBoard()
        {
            label1 = new TextBlock();
            label2 = new TextBlock();
            Image picture1 = new Image();
            Image picture2 = new Image();
            /////////////////////////////
            Canvas.SetLeft(label1, 32);
            label1.Width = 61; label1.Height = 55;
            label1.FontSize = 20; label1.FontWeight = FontWeights.Bold;
            InfoBoard.Children.Add(label1);
            /////////////////////////////
            Canvas.SetLeft(picture1, 33);
            picture1.Width = 62; picture1.Height = 55/2;
            picture1.Source = (BitmapImage)Application.Current.FindResource("bomb");
            InfoBoard.Children.Add(picture1);
            /////////////////////////////
            Canvas.SetLeft(label2, 125);
            label2.Width = 61; label2.Height = 55;
            label2.FontSize = 20; label2.FontWeight = FontWeights.Bold;
            InfoBoard.Children.Add(label2);
            /////////////////////////////
            Canvas.SetLeft(picture2, 126);
            picture2.Width = 62; picture2.Height = 55/2;
            picture2.Source = (BitmapImage)Application.Current.FindResource("monster");
            InfoBoard.Children.Add(picture2);

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            modal = new MapInput();
            modal.Closed += MapInput_Closed;
            modal.ShowDialog();
            int[,] map = board.GetMap();
            HelperClass.WriteTextFileContents(mapName, map, path);
            
        }

        private void MapInput_Closed(object sender, EventArgs e)
        {
            MapInput customModalWindow = (MapInput)sender;
            mapName = customModalWindow.UserInput;
            path = customModalWindow.path;
            customModalWindow.Closed -= MapInput_Closed;
        }

       

      

        
    }
}
