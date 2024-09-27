using System;

public class Ball
{
    public double X { get; private set; } // Позиция мяча по оси X
    public double Y { get; private set; } // Позиция мяча по оси Y

    private double _vx, _vy; // Скорость мяча по осям X и Y

    private Game _game; // Игра, в которой находится мяч


// Конструктор для инициализации мяча
public Ball(double x, double y, Game game)
    {
        _game = game; // Привязываем игру к мячу
        X = x; // Устанавливаем позицию мяча по оси X
        Y = y; // Устанавливаем позицию мяча по оси Y
    }

    // Метод для установки скорости мяча
    public void SetSpeed(double vx, double vy)
    {
        _vx = vx; // Устанавливаем скорость мяча по оси X
        _vy = vy; // Устанавливаем скорость мяча по оси Y
    }

    // Метод для обновления позиции мяча
    public void Move()
    {
        double newX = X + _vx; // Вычисляем новую позицию по оси X
        double newY = Y + _vy; // Вычисляем новую позицию по оси Y

        // Проверяем, находится ли новая позиция мяча в пределах стадиона
        if (_game.Stadium.IsIn(newX, newY))
        {
            X = newX; // Обновляем позицию мяча по оси X
            Y = newY; // Обновляем позицию мяча по оси Y
        }
        else // Если позиция мяча выходит за пределы стадиона
        {
            _vx = 0; // Обнуляем скорость мяча по оси X
            _vy = 0; // Обнуляем скорость мяча по оси Y
        }
    }