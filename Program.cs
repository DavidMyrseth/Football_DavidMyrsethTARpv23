using System;

namespace Football;

public class Player
{
    // Имя игрока (свойство доступно только для чтения).
    public string Name { get; }

    // Координаты игрока на поле по X и Y (свойства доступны только для изменения внутри класса).
    public double X { get; private set; }
    public double Y { get; private set; }

    // Шаг движения игрока по X и Y.
    private double _vx, _vy;

    // Команда игрока (свойство может быть установлено или изменено извне)
    public Team? Team { get; set; } = null;

    // Константа максимальной скорости игрока.
    private const double MaxSpeed = 5;

    // Константа максимальной скорости удара по мячу.
    private const double MaxKickSpeed = 25;

    // Максимальная дистанция от игрока до мяча, на которой возможен удар по мячу.
    private const double BallKickDistance = 10;

    // Экземпляр генератора случайных чисел.
    private Random _random = new Random();

    // Конструктор, принимающий только имя игрока.
    public Player(string name)
    {
        // Устанавливаем имя игрока.
        Name = name;
    }

    // Конструктор, принимающий имя игрока, начальные координаты и команду.
    public Player(string name, double x, double y, Team team)
    {
        // Устанавливаем имя, начальные координаты и команду игрока.
        Name = name;
        X = x;
        Y = y;
        Team = team;
    }

    // Метод для установки новой позиции игрока по координатам X и Y.
    public void SetPosition(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Метод для получения абсолютной позиции игрока на поле (с учётом команды).
    public (double, double) GetAbsolutePosition()
    {
        // Используем метод GetPositionForTeam из класса Game, чтобы получить позицию относительно команды.
        return Team!.Game.GetPositionForTeam(Team, X, Y);
    }

    // Метод для вычисления расстояния от игрока до мяча.
    public double GetDistanceToBall()
    {
        // Получаем позицию мяча у команды игрока.
        var ballPosition = Team!.GetBallPosition();

        // Вычисляем расстояние по осям X и Y между игроком и мячом.
        var dx = ballPosition.Item1 - X;
        var dy = ballPosition.Item2 - Y;

        // Возвращаем расстояние до мяча с использованием теоремы Пифагора.
        return Math.Sqrt(dx * dx + dy * dy);
    }

    // Метод для перемещения игрока в сторону мяча.
    public void MoveTowardsBall()
    {
        // Получаем позицию мяча.
        var ballPosition = Team!.GetBallPosition();

        // Вычисляем разницу по координатам X и Y между игроком и мячом.
        var dx = ballPosition.Item1 - X;
        var dy = ballPosition.Item2 - Y;

        // Рассчитываем коэффициент для скорости с учетом максимальной скорости игрока.
        var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed;

        // Определяем скорость по осям X и Y, чтобы двигаться к мячу.
        _vx = dx / ratio;
        _vy = dy / ratio;
    }

    // Метод для перемещения игрока.
    public void Move()
    {
        // Если этот игрок не является ближайшим к мячу, он не движется.
        if (Team.GetClosestPlayerToBall() != this)
        {
            _vx = 0;
            _vy = 0;
        }

        // Если игрок достаточно близко к мячу, он может ударить по нему.
        if (GetDistanceToBall() < BallKickDistance)
        {
            // Устанавливаем случайную скорость удара по мячу (скорость и направление).
            Team.SetBallSpeed(
                MaxKickSpeed * _random.NextDouble(),                   // Случайная скорость по X
                MaxKickSpeed * (_random.NextDouble() - 0.5)            // Случайная скорость по Y с отклонением
            );
        }

        // Вычисляем новые координаты игрока.
        var newX = X + _vx;
        var newY = Y + _vy;

        // Получаем абсолютную позицию игрока на поле (с учётом команды).
        var newAbsolutePosition = Team.Game.GetPositionForTeam(Team, newX, newY);

        // Проверяем, находится ли новая позиция игрока внутри стадиона.
        if (Team.Game.Stadium.IsIn(newAbsolutePosition.Item1, newAbsolutePosition.Item2))
        {
            // Если да, обновляем позицию игрока.
            X = newX;
            Y = newY;
        }
        else
        {
            // Если нет, игрок перестает двигаться.
            _vx = _vy = 0;
        }
    }
}