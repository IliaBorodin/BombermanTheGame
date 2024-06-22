using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HomeLabWPF
{
    enum Arrows
    {
        left,
        right,
        up,
        down
    }
    class Hero
    {

        int step;
        MovingClass moving;
        public List<Bomb> bombs { get; private set; }
        int bomb_count;
        public int lenght_fire { get; private set; }
        public Hero(Image _hero, int x, int y, game_object[,] array, Condition[,] states)
        {
            
            step = 3;
            bomb_count = 3;
            lenght_fire = 3;
            bombs = new List<Bomb>();
            moving = new MovingClass(_hero, x, y, array, states);
        }

        public void MoveHero(Arrows arrow)
        {
            switch (arrow)
            {
                case Arrows.left:
                    moving.Move(-step, 0);
                    break;
                case Arrows.right:
                    moving.Move(step, 0);
                    break;
                case Arrows.up:
                    moving.Move(0, -step);
                    break;
                case Arrows.down:
                    moving.Move(0, step);
                    break;
                default:
                    break;

            }
        }
       
        public Point MyNowPoint()
        {
            return moving.MyNowPoint();
        }


        public bool PutBomb(game_object[,] map, Canvas canvas, detonation detonation)
        {
            if (bombs.Count >= bomb_count) return false;
            Bomb bomb = new Bomb(map, MyNowPoint(), canvas, detonation);
            bombs.Add(bomb);
            return true;
        }

        public void BombRemove(Bomb bomb)
        {
            bombs.Remove(bomb);
        }
        public int availableBombCount()
        {
            return bomb_count - bombs.Count;
        }

    }
}
