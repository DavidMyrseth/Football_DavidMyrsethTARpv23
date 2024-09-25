using System;

namespace Football;

// Класс Player представляет игрока в команде и его поведение на поле.
public class Player
{
    // Имя игрока, доступно только для чтения.
    public string Name { get; }

    // Координаты игрока на поле (X и Y), доступные только для чтения извне.
    public double X { get; private set; }
    public double Y { get; private set; }

    // Переменные для хранения скорости игрока по осям X и Y.
    private double _vx, _vy;

    // Ссылка на команду, в которой находится игрок (может быть null).
    public Team? Team { get; set; } = null;

    // Константы: максимальная скорость игрока, максимальная скорость удара по мячу и расстояние, на котором можно ударить по мячу.
    private const double MaxSpeed = 5;
    private const double MaxKickSpeed = 25;
    private const double BallKickDistance = 10;

    // Экземпляр класса Random для генерации случайных чисел, используется для случайного направления удара по мячу.
    private Random _random = new Random();

    // Конструктор, который инициализирует игрока только с именем (без позиции и команды).
    public Player(string name)
    {
        Name = name;  // Устанавливаем имя игрока.
    }

    // Конструктор, который инициализирует игрока с именем, позицией и командой.
    public Player(string name, double x, double y, Team team)
    {
        Name = name;  // Устанавливаем имя игрока.
        X = x;        // Устанавливаем начальную позицию игрока по X.
        Y = y;        // Устанавливаем начальную позицию игрока по Y.
        Team = team;  // Устанавливаем команду, в которой находится игрок.
    }

    // Метод для установки позиции игрока.
    public void SetPosition(double x, double y)
    {
        X = x;  // Обновляем позицию игрока по X.
        Y = y;  // Обновляем позицию игрока по Y.
    }

    // Метод, который возвращает абсолютную позицию игрока на поле (учитывая команду).
    public (double, double) GetAbsolutePosition()
    {
        // Получаем позицию игрока для его команды через игру.
        return Team!.Game.GetPositionForTeam(Team, X, Y);
    }

    // Метод для вычисления расстояния до мяча.
    public double GetDistanceToBall()
    {
        // Получаем текущую позицию мяча относительно команды.
        var ballPosition = Team!.GetBallPosition();

        // Рассчитываем разности координат между игроком и мячом.
        var dx = ballPosition.Item1 - X;
        var dy = ballPosition.Item2 - Y;

        // Возвращаем расстояние до мяча по формуле Евклидова расстояния.
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // Метод для перемещения игрока в направлении мяча.
    public void MoveTowardsBall()
    {
        // Получаем позицию мяча относительно команды.
        var ballPosition = Team!.GetBallPosition();

        // Рассчитываем разности координат между игроком и мячом.
        var dx = ballPosition.Item1 - X;
        var dy = ballPosition.Item2 - Y;

        // Вычисляем соотношение расстояния до максимальной скорости.
        var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;

        // Устанавливаем скорость игрока по осям X и Y так, чтобы двигаться в направлении мяча.
        _vx = dx / ratio;
        _vy = dy / ratio;
    }

    // Метод, который перемещает игрока на поле.
    public void Move()
    {
        // Если этот игрок не является ближайшим к мячу, скорость обнуляется.
        if (Team.GetClosestPlayerToBall() != this)
        {
            _vx = 0;
            _vy = 0;
        }

        // Если игрок находится достаточно близко к мячу, он ударяет его.
        if (GetDistanceToBall() < BallKickDistance)
        {
            // Устанавливаем случайную скорость для мяча (в пределах максимальной скорости удара).
            Team.SetBallSpeed(
                MaxKickSpeed * _random.NextDouble(),
                MaxKickSpeed * (_random.NextDouble() - 0.5)
            );
        }

        // Рассчитываем новые координаты игрока с учетом его скорости.
        var newX = X + _vx;
        var newY = Y + _vy;

        // Получаем новую абсолютную позицию игрока на поле.
        var newAbsolutePosition = Team.Game.GetPositionForTeam(Team, newX, newY);

        // Если новая позиция игрока находится в пределах стадиона, обновляем координаты.
        if (Team.Game.Stadium.IsIn(newAbsolutePosition.Item1, newAbsolutePosition.Item2))
        {
            X = newX;  // Обновляем координату X.
            Y = newY;  // Обновляем координату Y.
        }
        else
        {
            // Если игрок выходит за пределы стадиона, его скорость обнуляется.
            _vx = _vy = 0;
        }
    }
}