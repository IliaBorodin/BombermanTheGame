
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HomeLabWPF
{
    public delegate void detonation(Bomb bomb);
    
    enum Condition
    {
        blank,
        wall,
        brick,
        bomb,
        fire,
        prize
    }
    public struct game_object
    {
        public Image texture;
        public Point coordinates;
    }



    class BattleField
    {
        private Canvas canvasGame;
        private updateInfo updateInfo;
        private Hero hero;
        private Enemy monster;
        private game_object[,] _map;
        private int sizeX = 17; //Размер поля по коор. Х
        private int sizeY = 11; //Размер поля по коор. Y
        private Condition[,] states_map;
        private Random random = new Random();
        private List<Enemy> mobs;
        private deClear need_clear;
        public BattleField(Canvas canvas, deClear clear, int level_enemy, updateInfo _updateInfo)
        {
            updateInfo = _updateInfo;
            canvasGame = canvas;
            mobs = new List<Enemy>();
            need_clear = clear;
            ImageBrush backgroundBrush = new ImageBrush((BitmapImage)Application.Current.FindResource("grass"));
            
            canvasGame.Background = backgroundBrush;
            int imageSize = getImageSize();
            canvasGame.Width = sizeX * imageSize;
            canvasGame.Height = sizeY * imageSize;
            InitStartMap(imageSize);
            InitStartPlayer( imageSize);
            for(int i = 0; i < 3; ++i)
            {
                InitEnemy(imageSize, level_enemy);
            }
            updateInfo(hero.availableBombCount(), mobs.Count);
            
        }
        public BattleField(Canvas canvas, deClear clear, int level_enemy, updateInfo _updateInfo, int[] array)
        {
            updateInfo = _updateInfo;
            canvasGame = canvas;
            mobs = new List<Enemy>();
            need_clear = clear;
            ImageBrush backgroundBrush = new ImageBrush((BitmapImage)Application.Current.FindResource("grass"));

            canvasGame.Background = backgroundBrush;
            int imageSize = getImageSize();
            canvasGame.Width = sizeX * imageSize;
            canvasGame.Height = sizeY * imageSize;
            InitStartMap(imageSize, sequence_to_matrix(array));
            InitStartPlayer(imageSize);
            for (int i = 0; i < 3; ++i)
            {
                InitEnemy(imageSize, level_enemy);
            }
            updateInfo(hero.availableBombCount(), mobs.Count);

        }

        private int[,] sequence_to_matrix(int[] array)
        {
            int[,] matrix = new int[sizeX, sizeY];
            for(int x =0; x < sizeX; ++x)           
                for(int y = 0; y < sizeY; ++y)      
                    matrix[x, y] = array[x * sizeY + y];

            return matrix;

        }

        private void InitStartMap(int imageSize)
        {
            
            _map = new game_object[sizeX, sizeY];
            states_map = new Condition[sizeX, sizeY];
            canvasGame.Children.Clear();
            for (int x = 0; x < sizeX; ++x)
            {
                for (int y = 0; y < sizeY; ++y)
                {
                    if (x == 0 || y == 0 || x == sizeX - 1 || y == sizeY - 1)
                        CreatePlace( x,  y, imageSize, Condition.wall);
                    else if (x % 2 == 0 && y % 2 == 0)
                    {
                        CreatePlace( x,  y, imageSize, Condition.wall);
                    }
                    else if (random.Next(3) == 0)
                    {
                        CreatePlace( x,  y, imageSize, Condition.brick);
                    }
                    else
                        CreatePlace( x,  y, imageSize, Condition.blank);
                }
            }
            ChangeCondition(1,1, Condition.blank);
            ChangeCondition(2,1, Condition.blank);
            ChangeCondition(1,2, Condition.blank);
            ChangeCondition(15, 9, Condition.blank);
            ChangeCondition(15, 8, Condition.blank);
            ChangeCondition(14, 9, Condition.blank);
        }

        private void InitStartMap(int imageSize,int[,] array)
        {
            _map = new game_object[sizeX, sizeY];
            states_map = new Condition[sizeX, sizeY];
            canvasGame.Children.Clear();
            for(int x = 0; x < sizeX; ++x)
            {
                for(int y = 0; y < sizeY; ++y)
                {
                    
                   CreatePlace(x, y, imageSize, (Condition)array[x,y]);
                }
            }

        }

        private int getImageSize()
        {
            int sizeFromX = (int)(canvasGame.Width / sizeX);
            int sizeFromY = (int)(canvasGame.Height / sizeY);
            return sizeFromX < sizeFromY ? sizeFromX : sizeFromY;
        }

        private void CreatePlace(int x, int y, int imageSize, Condition condition)
        {
            int x_cur = x * imageSize;
            int y_cur = y * imageSize;
            Image picture = new Image();
            Canvas.SetLeft(picture, x_cur);
            Canvas.SetTop(picture, y_cur);
            picture.Width = imageSize;
            picture.Height = imageSize;
           picture.Source = (BitmapImage)Application.Current.FindResource("grass");
            canvasGame.Children.Add(picture);

           
            _map[x, y].texture = picture;
            _map[x, y].coordinates = new Point(x_cur, y_cur);
            ChangeCondition(x, y, condition);

        }

        private void ChangeCondition(int x, int y, Condition condition)
        {
            switch (condition)
            {
                case Condition.wall:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("wall");
                    break;
                case Condition.brick:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("brick");
                    break;
                case Condition.bomb:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("bomb");
                    break;
                case Condition.fire:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("fire");
                    break;
                case Condition.prize:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("gold");
                    break;

                default:
                    _map[x, y].texture.Source = (BitmapImage)Application.Current.FindResource("grass");
                    break;
            }
            states_map[x, y] = condition;
        }

        private void InitStartPlayer(int imageSize)
        {
            int x = 1, y = 1;
            int x_cur = x * imageSize + 7;
            int y_cur = y * imageSize + 3;
            Image picture = new Image();
            Canvas.SetLeft(picture, x_cur);
            Canvas.SetTop(picture, y_cur);
            picture.Width = imageSize - 14;
            picture.Height = imageSize - 6;        
            picture.Source = (BitmapImage)Application.Current.FindResource("hero");
            canvasGame.Children.Add(picture);
            hero = new Hero(picture, x_cur, y_cur, _map, states_map);
           
        }

        private void InitEnemy(int imageSize, int level_enemy)
        {
            int _x = 15, _y = 8;
            FindEmptyPlace(ref _x, ref _y);
            int x_cur = _x * imageSize + 7;
            int y_cur = _y * imageSize + 3;
            Image picture = new Image();
            Canvas.SetLeft(picture, x_cur);
            Canvas.SetTop(picture, y_cur);
            picture.Width = imageSize - 14;
            picture.Height = imageSize - 5;
            picture.Source = (BitmapImage)Application.Current.FindResource("monster");
            canvasGame.Children.Add(picture);
            monster = new Enemy(picture, x_cur, y_cur, _map, states_map, level_enemy, hero) ;
            mobs.Add(monster);
        }

        public void MoveHero(Arrows arrow)
        {
            if (hero == null) return;
            hero.MoveHero(arrow);
        }

        public void PutBomb()
        {
            Point player_point = hero.MyNowPoint();
            if (states_map[(int)player_point.X, (int)player_point.Y] == Condition.bomb) return;
            if(hero.PutBomb(_map, canvasGame, Detonation))
                ChangeCondition((int)hero.MyNowPoint().X, (int)hero.MyNowPoint().Y, Condition.bomb);
            updateInfo(hero.availableBombCount(), mobs.Count);


        }


        private void FindEmptyPlace(ref int x, ref int y)
        {
            int loop = 0;
            do
            {
                x = random.Next(states_map.GetLength(0)/2, states_map.GetLength(0));
                y = random.Next(1, states_map.GetLength(1));
            } while (states_map[x, y] != Condition.blank && loop++ < 100);
            if (loop >= 100)
            {
                x = 15;
                y = 9;
            }
                
            
        }

        private void Detonation(Bomb bomb)
        {
            ChangeCondition((int)bomb.bombPlace.X, (int)bomb.bombPlace.Y, Condition.fire);
            Flame(bomb.bombPlace, Arrows.left);
            Flame(bomb.bombPlace, Arrows.right);
            Flame(bomb.bombPlace, Arrows.up);
            Flame(bomb.bombPlace, Arrows.down);

            hero.BombRemove(bomb);
            Blaze();
            need_clear();
            updateInfo(hero.availableBombCount(), mobs.Count);
        }

        private void Blaze()
        {
            List<Enemy> delMobs = new List<Enemy>();
            foreach(Enemy mob in mobs)
            {
                Point mobPoint = mob.MyNowPoint();
                if (states_map[(int)mobPoint.X, (int)mobPoint.Y] == Condition.fire)
                    delMobs.Add(mob);
            }

            for(int x = 0; x < delMobs.Count; ++x)
            {
                mobs.Remove(delMobs[x]);
                canvasGame.Children.Remove(delMobs[x].texture);
                delMobs[x] = null;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            updateInfo(hero.availableBombCount(), mobs.Count);
        }

        private void Flame(Point point, Arrows arrow)
        {
            int sx = 0, sy = 0;
            switch (arrow)
            {
                case Arrows.left:
                    sx = -1;
                    break;
                case Arrows.right:
                    sx = 1;
                    break;
                case Arrows.up:
                    sy = -1;
                    break;
                case Arrows.down:
                    sy = 1;
                    break;
                default:
                    break;
            }

            bool isNotDone = true;
            int x = 0, y = 0;

            do
            {
                x += sx;
                y += sy;
                if (Math.Abs(x) > hero.lenght_fire|| Math.Abs(y) > hero.lenght_fire) break;
                if (isFire(point, -x, y))
                    ChangeCondition((int)point.X - x, (int)point.Y + y, Condition.fire);
                else
                    isNotDone = false;

            } while (isNotDone);
        }

        private bool isFire(Point place, int sx, int sy)
        {
            switch (states_map[(int)place.X+sx,(int)place.Y+sy])
            {
                case Condition.blank:
                    return true;
                   
                case Condition.wall:
                    return false;
                    
                case Condition.brick:
                    ChangeCondition((int)place.X + sx, (int)place.Y + sy, Condition.fire);
                    return false;
                case Condition.bomb:
                    foreach(Bomb bomb in hero.bombs)
                    {
                        if(bomb.bombPlace.X == (int)place.X && bomb.bombPlace.Y == (int)place.Y)
                        {
                            bomb.Reaction();
                        }
                    }
                    return false;                   
                default:
                    return true;
            }
        }

        public void ClearFire()
        {
            for(int x=0;x<states_map.GetLength(0);++x)
                for(int y = 0; y < states_map.GetLength(1); ++y)
                {
                    if (states_map[x, y] == Condition.fire)
                        ChangeCondition(x, y, Condition.blank);
                }
        }


        public bool GameOver()
        {
            Point point = hero.MyNowPoint();
            if (states_map[(int)point.X, (int)point.Y] == Condition.fire)
                return true;
            if (mobs.Count == 0) return true;

            foreach (Enemy mob in mobs)
            {
                if (point == mob.MyNowPoint()) return true;
            }
            return false;
        }


        static public int[,] getMaskOfMap(Condition[,] states_map)
        {
            int[,] mask = new int[states_map.GetLength(0), states_map.GetLength(1)];
            for(int x=0;x<states_map.GetLength(0);++x)
                for(int y=0; y < states_map.GetLength(1); ++y)
                {
                    if (states_map[x, y] == Condition.blank)
                        mask[x, y] = 0;
                    else
                        mask[x, y] = -1;
                }
            return mask;
        }
        public  int[,] GetMap()
        {
            int[,] map = new int[sizeX, sizeY];
            for(int x = 0; x < sizeX; ++x)
                for(int y=0; y < sizeY; ++y)
                {
                    map[x, y] = (int)states_map[x, y];
                }
            return map;

        }
    }

    
}
