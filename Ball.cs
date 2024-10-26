namespace Football_DavidMyrsethTARpv23
{
    public class Ball
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private double _vx, _vy;

        private Game _game;

        public Ball(int x, int y, Game game)
        {
            _game = game;
            X = x;
            Y = y;
        }

        public void SetPosition(int x, int y)
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
            int newX = X + (int)_vx;
            int newY = Y + (int)_vy;

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
                    newX = X + (int)_vx; //перемещение мяч на 1 пиксель
                    X = newX < 0 ? 1 : newX >= _game.Stadium.Width ? _game.Stadium.Width - 1 : newX;
                }

                if (newY < 0 || newY >= _game.Stadium.Height)
                {
                    _vy = -_vy; //сменна направление по оси Y
                    newY = Y + (int)_vy; //перимещаем мяч на 1 пиксель
                    Y = newY < 0 ? 1 : newY >= _game.Stadium.Height ? _game.Stadium.Height - 1 : newY;
                }
            }
        }
    }
}