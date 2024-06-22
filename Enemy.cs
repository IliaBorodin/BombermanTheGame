
using MyLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;


namespace HomeLabWPF
{
    class Enemy
    {
      public  Image texture { get; private set; }
      private int level;
      private Timer timer;
      private  int paths;
      private Point[] path; 
      private Point destinePlace;
      private  Point mobPlace;
      private MovingClass moving;
      private  Condition[,] states_map;
      private int step;
      private   int[,] fmap;
      private int pathStep;
      private Random random = new Random();
      private MyClass help = new MyClass();
      private Hero hero;
         
        public Enemy(Image image, int x, int y, game_object[,] array, Condition[,] states, int _level, Hero _hero)
        {
            texture = image;
            level = _level;
            hero = _hero;
            step = 3;
            CreateTimer();
            states_map = states;
            fmap = new int[states_map.GetLength(0), states_map.GetLength(1)];
            path = new Point[states_map.GetLength(0) * states_map.GetLength(1)];
            moving = new MovingClass(image, x, y, array, states);
            mobPlace = moving.MyNowPoint();
            destinePlace = mobPlace;
            timer.Enabled = true;
            


        }
        
        private void CreateTimer()
        {
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += timer_Tick; 
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mobPlace == destinePlace)
                GetNewPlace();
            if (path[0].X == 0 & path[0].Y == 0)
                if (!help.FindPath(fmap, BattleField.getMaskOfMap(states_map), ref paths,ref pathStep, ref path , mobPlace , destinePlace)) return;
            if (pathStep > paths) return;
            if (path[pathStep] == mobPlace)
                pathStep++;
            else
                MoveMob(path[pathStep]);
        }
       


        private void MoveMob(Point newPlace)
        {
            int sx = 0; int sy = 0;
            if (mobPlace.X < newPlace.X)
                sx = (int)newPlace.X - (int)mobPlace.X > step ? step : (int)newPlace.X - (int)mobPlace.X;
            else
                sx = (int)mobPlace.X - (int)newPlace.X < step ? (int)newPlace.X - (int)mobPlace.X : -step;
            if (mobPlace.Y < newPlace.Y)
                sy = (int)newPlace.Y - (int)mobPlace.Y > step ? step : (int)newPlace.Y - (int)mobPlace.Y;
            else
                sy = (int)mobPlace.Y - (int)newPlace.Y < step ? (int)newPlace.Y - (int)mobPlace.Y : -step;

            moving.Move(sx, sy);
            mobPlace = moving.MyNowPoint();
            if (level >= 2 && states_map[(int)newPlace.X, (int)newPlace.Y]==Condition.bomb || states_map[(int)newPlace.X, (int)newPlace.Y] == Condition.fire)
            {
                GetNewPlace();
            }

        }
        private void GetNewPlace()
        {
            if (level >= 3)
            {
                destinePlace = hero.MyNowPoint();
                if (help.FindPath(fmap, BattleField.getMaskOfMap(states_map), ref paths, ref pathStep, ref path, mobPlace, destinePlace))
                    return;
            }
            int loop = 0;
            do
            {
                destinePlace.X = random.Next(1, states_map.GetLength(0) - 1);
                destinePlace.Y = random.Next(1, states_map.GetLength(1) - 1);



            } while (!help.FindPath(fmap, BattleField.getMaskOfMap(states_map), ref paths, ref pathStep, ref path, mobPlace, destinePlace) && loop++<100);
            if (loop >= 100)
            {
                destinePlace = mobPlace;
            }
        }

        public Point MyNowPoint()
        {
            return moving.MyNowPoint();
        }

    }
}
