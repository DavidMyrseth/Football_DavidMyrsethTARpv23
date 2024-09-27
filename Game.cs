using Football;

public class Game
{
    public Team HomeTeam { get; } // Домашняя команда
    public Team AwayTeam { get; } // Гостевая команда
    public Stadium Stadium { get; } // Стадион
    public Ball Ball { get; private set; } // Мяч

    // Конструктор для инициализации игры
    public Game(Team homeTeam, Team awayTeam, Stadium stadium)
    {
        HomeTeam = homeTeam; // Устанавливаем домашнюю команду
        homeTeam.Game = this; // Привязываем игру к домашней команде
        AwayTeam = awayTeam; // Устанавливаем гостевую команду
        awayTeam.Game = this; // Привязываем игру к гостевой команде
        Stadium = stadium; // Устанавливаем стадион
    }

    // Метод для старта игры
    public void Start()
    {
        Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this); // Размещаем мяч в центре поля
        HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height); // Запускаем игроков домашней команды
        AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height); // Запускаем игроков гостевой команды
    }

    // Метод для получения позиции для гостей
    private (double, double) GetPositionForAwayTeam(double x, double y)
    {
        return (Stadium.Width - x, Stadium.Height - y); // Отражаем позицию для гостей
    }

    // Метод для получения позиции для команды
    public (double, double) GetPositionForTeam(Team team, double x, double y)
    {
        return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y); // Возвращаем позицию в зависимости от команды
    }

    // Метод для получения позиции мяча для команды
    public (double, double) GetBallPositionForTeam(Team team)
    {
        return GetPositionForTeam(team, Ball.X, Ball.Y); // Возвращаем позицию мяча с учетом команды
    }

    // Метод для установки скорости мяча для команды
    public void SetBallSpeedForTeam(Team team, double vx, double vy)
    {
        if (team == HomeTeam) // Если команда домашняя
        {
            Ball.SetSpeed(vx, vy); // Устанавливаем скорость мяча
        }
        else // Если команда гостевая
        {
            Ball.SetSpeed(-vx, -vy); // Устанавливаем скорость мяча в противоположном направлении
        }
    }

    // Метод для обновления состояний в игре
    public void Move()
    {
        HomeTeam.Move(); // Двигаем игроков домашней команды
        AwayTeam.Move(); // Двигаем игроков гостевой команды
        Ball.Move(); // Двигаем мяч
    }

    // Метод для сброса позиций игроков и мяча после забитого гола
    public void ResetPositions()
    {
        Ball.SetPosition(Stadium.Width / 2, Stadium.Height / 2); // Ставим мяч в центр
        HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height); // Запускаем игроков домашней команды
        AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height); // Запускаем игроков гостевой команды
    }
}