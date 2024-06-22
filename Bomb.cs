
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace HomeLabWPF
{
    public class Bomb
    {
       private Timer timer;
       private Canvas canvas;
       private int second_count;
       private game_object[,] map;
       public Point bombPlace { get; private set; }
       private TextBlock label;
       private detonation detonation;
        public Bomb(game_object[,] array, Point point, Canvas _canvas, detonation _detonation)
        {
            
            map = array;
            second_count = 4;
            bombPlace = point;
            canvas = _canvas;
            detonation = _detonation;
            CreateTime();
            timer.Enabled = true;
            InitLabel();
        }
        public Bomb()
        {

        }
        private void InitLabel()
        {
            label = new TextBlock();
            Canvas.SetLeft(label, map[(int)bombPlace.X, (int)bombPlace.Y].texture.Width * (int)bombPlace.X + 7);
            Canvas.SetTop(label, map[(int)bombPlace.X, (int)bombPlace.Y].texture.Height * (int)bombPlace.Y + 7);
            label.Foreground = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(label);
        }

        private void CreateTime()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (second_count <= 0)
            {
                label.Text = "";
                timer.Enabled = false;
                detonation(this);
                return;
            }
            WriteTimer(--second_count);
        }

        private void WriteTimer(int number)
        {
            label.Text = number.ToString();

        }

        public void Reaction()
        {
            second_count = 0;
        }
        
        
    }
}
