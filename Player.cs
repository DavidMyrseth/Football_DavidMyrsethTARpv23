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

        public (int, int) GetAbsolutePosition()
        {
            return Team!.Game.GetPositionForTeam(Team, X, Y);
        }

        public int GetDistanceToBall()
        {
            (int, int) ballPosition = Team!.GetBallPosition();
            int dx = ballPosition.Item1 - X;
            int dy = ballPosition.Item2 - Y;
            return (int) Math.Sqrt((double)dx * (double)dx + (double)dy * (double)dy);
        }

        public void MoveTowardsBall()
        {
            var ballPosition = Team!.GetBallPosition();
            var dx = ballPosition.Item1 - X;
            var dy = ballPosition.Item2 - Y;
            var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;
            _vx = (dx == 0 ? 1 : dx) / ((int)ratio < 1 ? 1 : (int)ratio);
            _vy = (dy == 0 ? 1 : dy) / ((int)ratio < 1 ? 1 : (int)ratio);
        }

        public void Move()
        {
            if (Team.GetClosestPlayerToBall() != this)
            {
                _vx = 0;
                _vy = 0;
            }

            if (GetDistanceToBall() < BallKickDistance)
            {
                Team.SetBallSpeed(
                    MaxKickSpeed * _random.NextDouble(),
                    MaxKickSpeed * (_random.NextDouble() - 0.5)
                );
            }

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