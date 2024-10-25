namespace Football_DavidMyrsethTARpv23
{
    public class Game
    {
        public Team HomeTeam { get; }
        public Team GuestTeam { get; }
        public Stadium Stadium { get; }
        public Ball Ball { get; private set; }


        public Game(Team homeTeam, Team guestTeam, Stadium stadium)
        {
            HomeTeam = homeTeam;
            homeTeam.Game = this;
            GuestTeam = guestTeam;
            guestTeam.Game = this;
            Stadium = stadium;
        }


        public void Start()
        {
            Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this);
            HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height);
            GuestTeam.StartGame(Stadium.Width / 2, Stadium.Height);
        }


        private (double, double) GetPositionForAwayTeam(double x, double y)
        {
            return (Stadium.Width - x, Stadium.Height - y);
        }


        public (double, double) GetPositionForTeam(Team team, double x, double y)
        {
            return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y);
        }


        public (double, double) GetBallPositionForTeam(Team team)
        {
            return GetPositionForTeam(team, Ball.X, Ball.Y);
        }


        public void SetBallSpeedForTeam(Team team, double vx, double vy)
        {
            if (team == HomeTeam)
            {
                Ball.SetSpeed(vx, vy);
            }
            else
            {
                Ball.SetSpeed(-vx, -vy);
            }
        }


        public void Move()
        {
            HomeTeam.Move();
            GuestTeam.Move();
            Ball.Move();
            CheckGoal();
        }
        private void CheckGoal()
        {
            // Проверка на голы
            if (Ball.X <= 0) // Мяч попал в ворота домашней команды
            {
                GuestTeam.ScoreGoal(); // Увеличиваем счет для Guest
                Console.WriteLine("Guest забил гол!"); // Логирование
                ResetBall();
            }
            else if (Ball.X >= Stadium.Width - 1) // Мяч попал в ворота выездной команды
            {
                HomeTeam.ScoreGoal(); // Увеличиваем счет для Home
                Console.WriteLine("Home забил гол!"); // Логирование
                ResetBall();
            }
        }


        private void ResetBall()
        {
            Ball.SetPosition(Stadium.Width / 2, Stadium.Height / 2);
            Ball.SetSpeed(0, 0);
        }
    }
}