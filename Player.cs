namespace Football_DavidMyrsethTARpv23
{
    public class Player
    {   
        // Свойства игрока имя и его координаты на поле (X, Y)
        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }

        // Скорости игрока по осям X и Y
        private int _vx, _vy;

        // Команда, к которой принадлежит игрок
        public Team? Team { get; set; } = null;

        private const int MaxSpeed = 5;
        private const int MaxKickSpeed = 25;
        private const int BallKickDistance = 10;

        // Генератор случайных чисел для расчета силы удара
        private Random _random = new Random();

        public Player(string name)
        {
            Name = name;
        }

        public Player(string name, int x, int y, Team team)
        {
            Name = name;
            X = x;
            Y = y;
            Team = team;
        }

        // Установка позиции игрока на поле
        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public (int, int) GetAbsolutePosition()
        {
            return Team!.Game.GetPositionForTeam(Team, X, Y);
        }

        public int GetDistanceToBall()
        {
            // Получаем текущую позицию мяча
            (int, int) ballPosition = Team!.GetBallPosition();
            int dx = ballPosition.Item1 - X;
            int dy = ballPosition.Item2 - Y;

            // Возвращаем расстояние до мяча с использованием теоремы Пифагора
            return (int) Math.Sqrt((double)dx * (double)dx + (double)dy * (double)dy);
        }

        // Метод для движения игрока к мячу
        public void MoveTowardsBall()
        {

            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;

            // Рассчитывает отношение дистанции до мяча к максимальной скорости игрока
            var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;
            
            // Устанавливаем скорость игрока по X и Y
            _vx = (dx == 0 ? 1 : dx) / ((int)ratio < 1 ? 1 : (int)ratio);
            _vy = (dy == 0 ? 1 : dy) / ((int)ratio < 1 ? 1 : (int)ratio);
        }

        public void Move()
        {
            // Игрок не является ближайшим к мячу, то его скорость обнуляется
            if (Team.GetClosestPlayerToBall() != this)
            {
                _vx = 0;
                _vy = 0;
            }


            // Близко к мячу, он выполняет удар по мячу
            if (GetDistanceToBall() < BallKickDistance)
            {
                Team.SetBallSpeed(
                    // Скорость удара по X
                    MaxKickSpeed * _random.NextDouble(),
                    // Скорость удара по Y
                    MaxKickSpeed * (_random.NextDouble() - 0.5)
                );
            }

            // Обновляем позицию игрока с учетом его скорости
            var newX = X + _vx;
            var newY = Y + _vy;

            var newAbsolutePosition = Team.Game.GetPositionForTeam(Team, newX, newY);
            if (Team.Game.Stadium.IsIn(newAbsolutePosition.Item1, newAbsolutePosition.Item2))
            {
                X = newX;
                Y = newY;
            }
            else
            {
                _vx = _vy = 0; //остановка игрока, если он выходит за пределы
            }
        }
    }
}