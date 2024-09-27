using System;

namespace Football;

// Класс для представления игрока
public class Player
{
    public string Name { get; } // Имя игрока
    public double X { get; private set; } // Позиция игрока по оси X
    public double Y { get; private set; } // Позиция игрока по оси Y
    private double _vx, _vy; // Скорость игрока по осям X и Y
    public Team? Team { get; set; } = null; // Команда игрока

    private const double MaxSpeed = 5; // Максимальная скорость игрока
    private const double MaxKickSpeed = 25; // Максимальная скорость удара по мячу
    private const double BallKickDistance = 10; // Дистанция для удара по мячу

    private Random _random = new Random(); // Генератор случайных чисел

    // Конструктор для создания игрока с именем
    public Player(string name)
    {
        Name = name;
    }

    // Конструктор для создания игрока с заданной позицией и командой
    public Player(string name, double x, double y, Team team)
    {
        Name = name;
        X = x;
        Y = y;
        Team = team;
    }

    // Метод для установки позиции игрока
    public void SetPosition(double x, double y)
    {
        X = x; // Устанавливаем позицию по оси X
        Y = y; // Устанавливаем позицию по оси Y
    }

    // Метод для получения абсолютной позиции игрока
    public (double, double) GetAbsolutePosition()
    {
        return Team!.Game.GetPositionForTeam(Team, X, Y); // Получаем позицию с учетом команды
    }

    // Метод для расчета расстояния до мяча
    public double GetDistanceToBall()
    {
        var ballPosition = Team!.GetBallPosition(); // Получаем позицию мяча
        var dx = ballPosition.Item1 - X; // Разность по оси X
        var dy = ballPosition.Item2 - Y; // Разность по оси Y
        return Math.Sqrt(dx * dx + dy * dy); // Возвращаем расстояние до мяча
    }

    // Метод для движения игрока к мячу
    public void MoveTowardsBall()
    {
        var ballPosition = Team!.GetBallPosition(); // Получаем позицию мяча
        var dx = ballPosition.Item1 - X; // Разность по оси X
        var dy = ballPosition.Item2 - Y; // Разность по оси Y
        var ratio = Math.Sqrt(dx * dx + dy * dy) / MaxSpeed; // Рассчитываем коэффициент
        _vx = dx / ratio; // Устанавливаем скорость по оси X
        _vy = dy / ratio; // Устанавливаем скорость по оси Y
    }

    // Метод для обновления позиции игрока
    public void Move()
    {
        if (Team.GetClosestPlayerToBall() != this) // Если игрок не ближайший к мячу
        {
            _vx = 0; // Обнуляем скорость по оси X
            _vy = 0; // Обнуляем скорость по оси Y
        }

        if (GetDistanceToBall() < BallKickDistance) // Если игрок находится в пределах удара по мячу
        {
            Team.SetBallSpeed(
                MaxKickSpeed * _random.NextDouble(), // Задаем скорость мяча по оси X
                MaxKickSpeed * (_random.NextDouble() - 0.5) // Задаем скорость мяча по оси Y
            );
        }

        var newX = X + _vx; // Обновляем позицию по оси X
        var newY = Y + _vy; // Обновляем позицию по оси Y
        var newAbsolutePosition = Team.Game.GetPositionForTeam(Team, newX, newY); // Получаем новую абсолютную позицию

        // Проверяем, находится ли новая позиция в пределах стадиона
        if (Team.Game.Stadium.IsIn(newAbsolutePosition.Item1, newAbsolutePosition.Item2))
        {
            X = newX; // Обновляем позицию по оси X
            Y = newY; // Обновляем позицию по оси Y
        }
        else // Если позиция выходит за пределы стадиона
        {
            _vx = _vy = 0; // Обнуляем скорость
        }
    }
}