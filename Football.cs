using System;
using System.Collections.Generic;

namespace Football;

// Класс Team представляет футбольную команду.
public class Team
{
    // Список игроков в команде (свойство доступно только для чтения).
    public List<Player> Players { get; } = new List<Player>();

    // Название команды (может быть установлено только внутри класса).
    public string Name { get; private set; }

    // Ссылка на игру, в которой участвует команда (свойство может быть изменено извне).
    public Game Game { get; set; }

    // Конструктор, принимающий название команды.
    public Team(string name)
    {
        // Устанавливаем название команды.
        Name = name;
    }

    // Метод для начала игры: размещает игроков на случайные позиции на поле.
    public void StartGame(int width, int height)
    {
        // Генератор случайных чисел.
        Random rnd = new Random();

        // Для каждого игрока в команде устанавливаем случайную позицию.
        foreach (var player in Players)
        {
            player.SetPosition(
                rnd.NextDouble() * width,    // Случайная X ширины поля.
                rnd.NextDouble() * height    // Случайная Y высоты поля.
            );
        }
    }

    // Метод для добавления игрока в команду.
    public void AddPlayer(Player player)
    {
        // Если у игрока уже есть команда, то ничего не делаем.
        if (player.Team != null) return;

        // Добавляем игрока в список игроков команды.
        Players.Add(player);

        // Устанавливаем команду для игрока.
        player.Team = this;
    }

    // Метод для получения позиции мяча относительно команды.
    public (double, double) GetBallPosition()
    {
        // Получаем позицию мяча из игры для этой команды.
        return Game.GetBallPositionForTeam(this);
    }

    // Метод для установки скорости мяча.
    public void SetBallSpeed(double vx, double vy)
    {
        // Устанавливаем скорость мяча для команды в игре.
        Game.SetBallSpeedForTeam(this, vx, vy);
    }

    // Метод для получения игрока, который находится ближе всего к мячу.
    public Player GetClosestPlayerToBall()
    {
        // Предполагаем, что первый игрок в списке — ближайший.
        Player closestPlayer = Players[0];

        // Изначально расстояние до мяча задается максимально возможным значением.
        double bestDistance = Double.MaxValue;

        // Проходим по всем игрокам в команде.
        foreach (var player in Players)
        {
            // Получаем расстояние игрока до мяча.
            var distance = player.GetDistanceToBall();

            // Если текущее расстояние меньше лучшего, обновляем ближайшего игрока.
            if (distance < bestDistance)
            {
                closestPlayer = player;
                bestDistance = distance;
            }
        }

        // Возвращаем игрока, который ближе всего к мячу.
        return closestPlayer;
    }

    // Метод для движения всех игроков в команде.
    public void Move()
    {
        // Игрок, ближайший к мячу, двигается к нему.
        GetClosestPlayerToBall().MoveTowardsBall();

        // Каждый игрок совершает движение (даже если он не двигается к мячу).
        Players.ForEach(player => player.Move());
    }
}
