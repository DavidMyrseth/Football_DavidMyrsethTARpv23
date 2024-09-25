using System;
using System.Collections.Generic;

namespace Football;

// Класс Team представляет команду, которая состоит из игроков и участвует в игре.
public class Team
{
    // Список игроков команды, инициализируется пустым списком.
    public List<Player> Players { get; } = new List<Player>();

    // Название команды, доступно только для чтения (устанавливается в конструкторе).
    public string Name { get; private set; }

    // Ссылка на игру, в которой участвует команда. Это свойство может быть изменено.
    public Game Game { get; set; }

    // Конструктор, который инициализирует команду с указанным именем.
    public Team(string name)
    {
        Name = name; // Устанавливаем имя команды.
    }

    // Метод, который расставляет игроков команды на случайные позиции внутри заданных границ поля.
    public void StartGame(int width, int height)
    {
        Random rnd = new Random(); // Создаем генератор случайных чисел.

        // Для каждого игрока в команде задаем случайную позицию.
        foreach (var player in Players)
        {
            player.SetPosition(
                rnd.NextDouble() * width,  // Случайная позиция по ширине поля.
                rnd.NextDouble() * height  // Случайная позиция по высоте поля.
            );
        }
    }

    // Метод добавления игрока в команду.
    public void AddPlayer(Player player)
    {
        // Если игрок уже состоит в другой команде, мы ничего не делаем.
        if (player.Team != null) return;

        // Добавляем игрока в команду.
        Players.Add(player);

        // Устанавливаем ссылку на команду для игрока.
        player.Team = this;
    }

    // Метод получения позиции мяча относительно команды.
    public (double, double) GetBallPosition()
    {
        // Получаем позицию мяча для данной команды через игру.
        return Game.GetBallPositionForTeam(this);
    }

    // Метод, который устанавливает скорость мяча относительно команды.
    public void SetBallSpeed(double vx, double vy)
    {
        // Устанавливаем скорость мяча через метод игры.
        Game.SetBallSpeedForTeam(this, vx, vy);
    }

    // Метод, который возвращает ближайшего к мячу игрока.
    public Player GetClosestPlayerToBall()
    {
        // Инициализируем ближайшего игрока как первого в списке.
        Player closestPlayer = Players[0];

        // Изначально устанавливаем минимальную дистанцию до мяча как максимально возможную.
        double bestDistance = Double.MaxValue;

        // Проходим по каждому игроку в команде.
        foreach (var player in Players)
        {
            // Вычисляем расстояние от игрока до мяча.
            var distance = player.GetDistanceToBall();

            // Если это расстояние меньше текущего минимального, обновляем ближайшего игрока.
            if (distance < bestDistance)
            {
                closestPlayer = player;
                bestDistance = distance;
            }
        }

        // Возвращаем ближайшего к мячу игрока.
        return closestPlayer;
    }

    // Метод, который двигает всех игроков команды.
    public void Move()
    {
        // Ближайший к мячу игрок двигается в направлении мяча.
        GetClosestPlayerToBall().MoveTowardsBall();

        // Для каждого игрока выполняем его движение.
        Players.ForEach(player => player.Move());
    }
}

