namespace Football_DavidMyrsethTARpv23
{
    public class Game
    {
        public Team HomeTeam { get; }
        public Team AwayTeam { get; }
        public Stadium Stadium { get; }
        public Ball Ball { get; private set; }

        public Game(Team homeTeam, Team awayTeam, Stadium stadium)
        {
            HomeTeam = homeTeam;
            homeTeam.Game = this;
            AwayTeam = awayTeam;
            awayTeam.Game = this;
            Stadium = stadium;
        }


        public void Start()
        {
            Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this);
            HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height);
            AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height);
        }


        private (int, int) GetPositionForAwayTeam(int x, int y)
        {
            return (Stadium.Width - x, Stadium.Height - y);
        }


        public (int, int) GetPositionForTeam(Team team, int x, int y)
        {
            return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y);
        }


        public (int, int) GetBallPositionForTeam(Team team)
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
            AwayTeam.Move();
            Ball.Move();
            CheckGoal();
        }

        private void CheckGoal()
        {
            //проверка на голы
            if (Ball.X <= 0) //мяч попал в ворота домашней
            {
                AwayTeam.ScoreGoal();
                ResetBall();
            }
            else if (Ball.X >= Stadium.Width - 1) //мяч попал в ворота выездной
            {
                HomeTeam.ScoreGoal();
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