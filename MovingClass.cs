using System.Windows;
using System.Windows.Controls;

namespace HomeLabWPF
{
    class MovingClass
    {
       private Image hero;
       private game_object[,] map;
       private Condition[,] states_map;
       private  int x_cur;
       private  int y_cur;

        public MovingClass(Image image, int x, int y, game_object[,] array, Condition[,] states)
        {
            hero = image;
            map = array;
            x_cur = x;
            y_cur = y;
            states_map = states;
        }
        public void Move(int offset_X, int offset_Y)
        {
            if (isEmpty(ref offset_X, ref offset_Y))
            {
                x_cur += offset_X;
                y_cur += offset_Y;
                Canvas.SetLeft(hero, x_cur);
                Canvas.SetTop(hero, y_cur);
            }
        }

        private bool isEmpty(ref int x, ref int y)
        {
            Point point = MyNowPoint();

            int hero_Right = (int)(x_cur + hero.Width);
            int hero_Left = x_cur;
            int hero_Down = (int)(y_cur + hero.Height);
            int hero_Up = y_cur;

            int rightWallLeft = (int)map[(int)point.X + 1, (int)point.Y].coordinates.X;
            int leftWallRight = (int)map[(int)point.X - 1, (int)point.Y].coordinates.X + (int)map[(int)point.X - 1, (int)point.Y].texture.Width;
            int downWallUp = (int)map[(int)point.X, (int)point.Y + 1].coordinates.Y;
            int upWallDown = (int)map[(int)point.X, (int)point.Y - 1].coordinates.Y + (int)map[(int)point.X, (int)point.Y - 1].texture.Height;

            int rightUpWallDown = (int)map[(int)point.X + 1, (int)point.Y - 1].coordinates.Y + (int)map[(int)point.X + 1, (int)point.Y - 1].texture.Height;
            int rightDownWallUp = (int)map[(int)point.X + 1, (int)point.Y + 1].coordinates.Y;
            int leftUpWallDown = (int)map[(int)point.X - 1, (int)point.Y - 1].coordinates.Y + (int)map[(int)point.X - 1, (int)point.Y - 1].texture.Height;
            int leftDownWallUp = (int)map[(int)point.X - 1, (int)point.Y + 1].coordinates.Y;

            int rightUpWallLeft = (int)map[(int)point.X + 1, (int)point.Y - 1].coordinates.X;
            int leftUpWallRight = (int)map[(int)point.X - 1, (int)point.Y - 1].coordinates.X + (int)map[(int)point.X - 1, (int)point.Y - 1].texture.Width;
            int rightDownWallLeft = (int)map[(int)point.X + 1, (int)point.Y + 1].coordinates.X;
            int leftDownWallRight = (int)map[(int)point.X - 1, (int)point.Y + 1].coordinates.X + (int)map[(int)point.X - 1, (int)point.Y + 1].texture.Width;

            int offset = 3;


            if (x > 0 && (states_map[(int)point.X + 1, (int)point.Y] == Condition.blank || states_map[(int)point.X + 1, (int)point.Y] == Condition.fire))
            {
                if (hero_Up < rightUpWallDown)
                    if (rightUpWallDown - hero_Up > offset)
                        y = offset;
                    else
                        y = rightUpWallDown - hero_Up;
                if (hero_Down > rightDownWallUp)
                    if (rightDownWallUp - hero_Down < -offset)
                        y = -offset;
                    else
                        y = rightDownWallUp - hero_Down;
                return true;
            }

            if (x < 0 && (states_map[(int)point.X - 1, (int)point.Y] == Condition.blank || states_map[(int)point.X - 1, (int)point.Y] == Condition.fire))
            {
                if (hero_Up < leftUpWallDown)
                    if (leftUpWallDown - hero_Up > offset)
                        y = offset;
                    else
                        y = leftUpWallDown - hero_Up;
                if (hero_Down > leftDownWallUp)
                    if (leftDownWallUp - hero_Down < -offset)
                        y = -offset;
                    else
                        y = leftDownWallUp - hero_Down;
                return true;
            }

            if (y > 0 && (states_map[(int)point.X, (int)point.Y + 1] == Condition.blank || states_map[(int)point.X, (int)point.Y + 1] == Condition.fire))
            {
                if (hero_Right > rightDownWallLeft)
                    if (rightDownWallLeft - hero_Right < -offset)
                        x = -offset;
                    else
                        x = rightDownWallLeft - hero_Right;
                if (hero_Left < leftDownWallRight)
                    if (leftDownWallRight - hero_Left > offset)
                        x = offset;
                    else
                        x = leftDownWallRight - hero_Left;
                return true;
            }

            if (y < 0 && (states_map[(int)point.X, (int)point.Y - 1] == Condition.blank || states_map[(int)point.X, (int)point.Y - 1] == Condition.fire))
            {
                if (hero_Right > rightUpWallLeft)
                    if (rightUpWallLeft - hero_Right < -offset)
                        x = -offset;
                    else
                        x = rightUpWallLeft - hero_Right;
                if (hero_Left < leftUpWallRight)
                    if (leftUpWallRight - hero_Left > offset)
                        x = offset;
                    else
                        x = leftUpWallRight - hero_Left;
                return true;
            }



            if (x > 0 && hero_Right + x > rightWallLeft)
                x = rightWallLeft - hero_Right;
            if (x < 0 && hero_Left + x < leftWallRight)
                x = leftWallRight - hero_Left;
            if (y > 0 && hero_Down + y > downWallUp)
                y = downWallUp - hero_Down;
            if (y < 0 && hero_Up + y < upWallDown)
                y = upWallDown - hero_Up;



            return true;
        }
        public Point MyNowPoint()
        {
            Point point = new Point();
            {
                point.X = x_cur + hero.Width / 2;
                point.Y = y_cur + hero.Height / 2;
            }
            for (int x = 0; x < map.GetLength(0); ++x)
            {
                for (int y = 0; y < map.GetLength(1); ++y)
                {
                    if (
                        map[x, y].coordinates.X <= point.X &&
                        map[x, y].coordinates.Y <= point.Y &&
                        map[x, y].coordinates.X + map[x, y].texture.Width >= point.X &&
                        map[x, y].coordinates.Y + map[x, y].texture.Height >= point.Y)
                        return new Point(x, y);
                }
            }


            return point;
        }
    }
}
