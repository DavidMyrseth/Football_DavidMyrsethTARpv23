namespace Football_DavidMyrsethTARpv23
{
    public class Team
    {
        // Список игроков команды
        public List<Player> Players { get; } = new List<Player>();
        public string Name { get; private set; }
        public Game Game { get; set; }
        public int Score { get; private set; } = 0;


        // Конструктор команды, который принимает название команды
        public Team(string name)
        {
            Name = name;
        }

        // Начальная расстановка игроков на поле
        public void StartGame(int width, int height)
        {
            Random rnd = new Random(); // генератор случайных чисел
            foreach (var player in Players)
            {
                // случайные позиции X и Y
                player.SetPosition(
                    (int)( rnd.NextDouble() * width),
                    (int)( rnd.NextDouble() * height)
                );
            }
        }


        public void AddPlayer(Player player)
        {
            // Проверка
            // если у игрока нет команды, то не добавляю игрока
            // Если игрок уже в команде, выходим из метода
            if (player.Team != null) return;
            // Добавляем в список команды
            Players.Add(player); 
            player.Team = this;
        }

        // Текущая позиция мяча
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

            // Перебираем всех игроков команды
            foreach (var player in Players)
            {
                var distance = player.GetDistanceToBall();

                // Если расстояние меньше, чем текущего игрока
                // то обновляем ближайшего игрока
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
            // Ближайший к мячу игрок начинает двигаться к мячу
            GetClosestPlayerToBall().MoveTowardsBall();
            Players.ForEach(player => player.Move());
        }

        public void ScoreGoal()
        {
            Score++; // +1
        }
    }
}