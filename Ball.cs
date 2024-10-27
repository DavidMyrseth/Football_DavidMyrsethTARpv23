namespace Football_DavidMyrsethTARpv23
{
    public class Ball
    {
        // Позиция мяча на поле
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
          
        // скорости мяча по осям X и Y
        public void SetSpeed(double vx, double vy)
        {
            _vx = vx;
            _vy = vy;
        }

        public void Move()
        {
            // Рассчитываем новую позицию мяча с учетом скорости
            int newX = X + (int)_vx;
            int newY = Y + (int)_vy;

            // Проверка остается ли мяч в пределах стадиона
            if (_game.Stadium.IsIn(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            else
            {
                // Рикошет от стены
                if (newX < 0 || newX >= _game.Stadium.Width)
                {
                    _vx = -_vx; // Смена направление по оси X
                    newX = X + (int)_vx; // Перемещение мяч на 1 пиксель
                    X = newX < 0 ? 1 : newX >= _game.Stadium.Width ? _game.Stadium.Width - 1 : newX;
                }

                if (newY < 0 || newY >= _game.Stadium.Height)
                {
                    // Сменна направление по оси Y
                    _vy = -_vy;

                    // Новая позиция мяча с учетом рикошета
                    newY = Y + (int)_vy;

                    // Если мяч вылетел за границы поля, корректируем его положение
                    Y = newY < 0 ? 1 : newY >= _game.Stadium.Height ? _game.Stadium.Height - 1 : newY;
                }
            }
        }
    }
}