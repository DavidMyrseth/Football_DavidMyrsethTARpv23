namespace Football;



public class Game
{
    // Домашняя команда (только для чтения, после установки через конструктор)
    public Team HomeTeam { get; }

    // Гостевая команда (только для чтения)
    public Team AwayTeam { get; }

    // Стадион, на котором проходит игра (только для чтения)
    public Stadium Stadium { get; }

    // Мяч в игре, его можно изменять только через приватные методы этого класса
    public Ball Ball { get; private set; }

    // Конструктор, который инициализирует игру с домашней и гостевой командами и стадионом
    public Game(Team homeTeam, Team awayTeam, Stadium stadium)
    {
        // Инициализация домашней команды.
        HomeTeam = homeTeam;

        // Связываем домашнюю команду с этой игрой, чтобы она знала, в какой игре участвует
        homeTeam.Game = this;

        // Инициализация гостевой команды
        AwayTeam = awayTeam;

        // Связываем гостевую команду с этой игрой
        awayTeam.Game = this;

        // Устанавливаем стадион для текущей игры
        Stadium = stadium;
    }

    // Метод для начала игры.
    public void Start()
    {
        // Инициализируем мяч в центре поля (ширина пополам, высота пополам)
        Ball = new Ball(Stadium.Width / 2, Stadium.Height / 2, this);

        // Устанавливаем начальные позиции игроков домашней команды на их половине поля
        HomeTeam.StartGame(Stadium.Width / 2, Stadium.Height);

        // Устанавливаем начальные позиции игроков гостевой команды на их половине поля
        AwayTeam.StartGame(Stadium.Width / 2, Stadium.Height);
    }

    // Приватный метод для расчета позиции игрока гостевой команды с зеркальной симметрией по полю
    private (double, double) GetPositionForAwayTeam(double x, double y)
    {
        // Зеркально отражаем координаты относительно оси стадиона (по ширине и высоте)
        return (Stadium.Width - x, Stadium.Height - y);
    }

    // Метод для получения позиции игрока с учётом того, за какую команду он играет (домашняя или гостевая)
    public (double, double) GetPositionForTeam(Team team, double x, double y)
    {
        // Если это домашняя команда, возвращаем исходные координаты, если гостевая — зеркально отражаем координаты
        return team == HomeTeam ? (x, y) : GetPositionForAwayTeam(x, y);
    }

    // Метод для получения позиции мяча относительно команды
    public (double, double) GetBallPositionForTeam(Team team)
    {
        // Возвращаем текущую позицию мяча с учётом команды (домашняя или гостевая)
        return GetPositionForTeam(team, Ball.X, Ball.Y);
    }

    // Метод для установки скорости мяча в зависимости от команды
    public void SetBallSpeedForTeam(Team team, double vx, double vy)
    {
        // Если это домашняя команда, устанавливаем заданную скорость мяча
        if (team == HomeTeam)
        {
            Ball.SetSpeed(vx, vy);
        }
        // Если это гостевая команда, инвертируем направление скорости мяча
        else
        {
            Ball.SetSpeed(-vx, -vy);
        }
    }

    // Метод для обновления состояний всех объектов игры (игроков и мяча)
    public void Move()
    {
        // Двигаем игроков домашней команды
        HomeTeam.Move();

        // Двигаем игроков гостевой команды
        AwayTeam.Move();

        // Двигаем мяч по полю в зависимости от его скорости
        Ball.Move();
    }
}
