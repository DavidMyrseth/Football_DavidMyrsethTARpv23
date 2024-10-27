namespace Football_DavidMyrsethTARpv23
{
    public class Game
    {
        // свойства
        public Team HomeTeam { get; }
        public Team AwayTeam { get; }
        public Stadium Stadium { get; }
        public Ball Ball { get; private set; }

        // конструктор игры задает
        public Game(Team homeTeam, Team awayTeam, Stadium stadium)
        {
            HomeTeam = homeTeam;
            homeTeam.Game = this;
            AwayTeam = awayTeam;
            awayTeam.Game = this;
            Stadium = stadium;
        }

        // старт игры
        public void Start()
        {
            Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this);
            HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height);
            AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height);
        }

        // Зеркальная позиция для выездной команды
        private (int, int) GetPositionForAwayTeam(int x, int y)
        {
            return (Stadium.Width - x, Stadium.Height - y);
        }

        // Возвращает позицию для команды, учитывая, домашняя это команда или выездная
        public (int, int) GetPositionForTeam(Team team, int x, int y)
        {
            return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y);
        }


        public (int, int) GetBallPositionForTeam(Team team)
        {
            return GetPositionForTeam(team, Ball.X, Ball.Y);
        }

        // Устанавливает скорость мяча для команды
        public void SetBallSpeedForTeam(Team team, double vx, double vy)
        {
            if (team == HomeTeam)
            {
                // Д
                Ball.SetSpeed(vx, vy);
            }
            else
            {   // В
                Ball.SetSpeed(-vx, -vy);
            }
        }

        // Движения команд и мяча в процессе игры
        public void Move()
        {
            // Обновляем
            HomeTeam.Move();
            AwayTeam.Move();
            Ball.Move();
            CheckGoal();
        }

        private void CheckGoal()
        {
            //проверка на голы
            //мяч попал в ворота домашней
            if (Ball.X <= 0)
            {
                AwayTeam.ScoreGoal();
                ResetBall();
            }
            //мяч попал в ворота выездной
            else if (Ball.X >= Stadium.Width - 1) 
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