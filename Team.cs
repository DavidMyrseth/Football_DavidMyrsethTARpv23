namespace Football_DavidMyrsethTARpv23
{
    public class Team
    {
        public List<Player> Players { get; } = new List<Player>();
        public string Name { get; private set; }
        public Game Game { get; set; }
        public int Score { get; private set; } = 0;


        public Team(string name)
        {
            Name = name;
        }


        public void StartGame(int width, int height)
        {
            Random rnd = new Random();
            foreach (var player in Players)
            {
                player.SetPosition(
                    (int)( rnd.NextDouble() * width),
                    (int)( rnd.NextDouble() * height)
                );
            }
        }


        public void AddPlayer(Player player)
        {
            if (player.Team != null) return; // если у игрока нет команды, то не добавляю игрока
            Players.Add(player);
            player.Team = this;
        }


        public (int, int) GetBallPosition()
        {
            return Game.GetBallPositionForTeam(this);
        }


        public void SetBallSpeed(double vx, double vy)
        {
            Game.SetBallSpeedForTeam(this, vx, vy);
        }


        public Player GetClosestPlayerToBall()
        {
            Player closestPlayer = Players[0];
            int bestDistance = int.MaxValue;
            foreach (var player in Players)
            {
                var distance = player.GetDistanceToBall();
                if (distance < bestDistance)
                {
                    closestPlayer = player;
                    bestDistance = distance;
                }
            }

            return closestPlayer;
        }


        public void Move()
        {
            GetClosestPlayerToBall().MoveTowardsBall();
            Players.ForEach(player => player.Move());
        }

        public void ScoreGoal()
        {
            Score++;
        }
    }
}