namespace Football_DavidMyrsethTARpv23
{
    public class Player
    {
        public string Name { get; }
        public int X { get; private set; }
        public int Y { get; private set; }
        private int _vx, _vy;
        public Team? Team { get; set; } = null;

        private const int MaxSpeed = 5;
        private const int MaxKickSpeed = 25;
        private const int BallKickDistance = 10;

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

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int GetDistanceToBall()
        {
            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;
            // Приводим результат к int
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }

        public void MoveTowardsBall()
        {
            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;
            var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;

            // Приводим к int
            _vx = (int)(dx / ratio);
            _vy = (int)(dy / ratio);
        }

        public void Move()
        {
            if (Team.GetClosestPlayerToBall() != this)
            {
                _vx = 0;
                _vy = 0;
            }

            // Удар по мячу, если он рядом
            if (GetDistanceToBall() < BallKickDistance)
            {
                int kickStrength = (int)(MaxKickSpeed * _random.NextDouble()); // Случайная сила удара
                Team.SetBallSpeed(kickStrength * (1 + _random.Next(0, 2)), kickStrength * (_random.Next(0, 2) - 1)); // Используйте метод Next для получения случайных значений
            }

            // Обновляем позицию игрока
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
                _vx = _vy = 0; // Остановка игрока, если он выходит за пределы
            }
        }
    }
}