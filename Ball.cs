using System;

namespace Football_DavidMyrsethTARpv23
{
    public class Ball
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        private double _vx, _vy;

        private Game _game;



        public Ball(double x, double y, Game game)
        {
            _game = game;
            X = x;
            Y = y;
        }

        public void SetPosition(double x, double y)
        {
            X = x;
            Y = y;
        }


        public void SetSpeed(double vx, double vy)
        {
            _vx = vx;
            _vy = vy;
        }


        public void Move()
        {
            double newX = X + _vx;
            double newY = Y + _vy;

            if (_game.Stadium.IsIn(newX, newY))//проверка на столкновение с стеной
            {
                X = newX;
                Y = newY;
            }
            else
            {

                if (newX < 0 || newX >= _game.Stadium.Width)//рикошет от стены
                {
                    _vx = -_vx; //смена направление по оси X
                    newX = X + _vx; //перемещение мяч на 1 пиксель
                    X = newX < 0 ? 1 : newX >= _game.Stadium.Width ? _game.Stadium.Width - 1 : newX;
                }

                if (newY < 0 || newY >= _game.Stadium.Height)
                {
                    _vy = -_vy; //сменна направление по оси Y
                    newY = Y + _vy; //перимещаем мяч на 1 пиксель
                    Y = newY < 0 ? 1 : newY >= _game.Stadium.Height ? _game.Stadium.Height - 1 : newY;
                }
            }
        }
    }
}