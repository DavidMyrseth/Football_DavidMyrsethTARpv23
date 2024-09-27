using System;
using System.Collections.Generic;

namespace Football;

public class Team
{
    public List<Player> Players { get; } = new List<Player>(); // Список игроков команды
    public string Name { get; private set; } // Имя команды
    public Game Game { get; set; } // Игра, в которой находится команда

    // Конструктор для создания команды
    public Team(string name)
    {
        Name = name; // Устанавливаем имя команды
    }

    // Метод для старта игры для команды
    public void StartGame(int width, int height)
    {
        Random rnd = new Random(); // Создаем генератор случайных чисел
        foreach (var player in Players) // Для каждого игрока в команде
        {
            player.SetPosition(
                rnd.NextDouble() * width, // Устанавливаем случайную позицию по оси X
                rnd.NextDouble() * height // Устанавливаем случайную позицию по оси Y
            );
        }
    }

    // Метод для добавления игрока в команду
    public void AddPlayer(Player player)
    {
        if (player.Team != null) return; // Если игрок уже в команде, ничего не делаем
        Players.Add(player); // Добавляем игрока в список
        player.Team = this; // Привязываем игрока к команде
    }

    // Метод для получения позиции мяча для команды
    public (double, double) GetBallPosition()
    {
        return Game.GetBallPositionForTeam(this); // Получаем позицию мяча с учетом команды
    }

    // Метод для установки скорости мяча для команды
    public void SetBallSpeed(double vx, double vy)
    {
        Game.SetBallSpeedForTeam(this, vx, vy); // Устанавливаем скорость мяча
    }

    // Метод для получения ближайшего игрока к мячу
    public Player GetClosestPlayerToBall()
    {
        Player closestPlayer = Players[0]; // Начинаем с первого игрока
        double bestDistance = Double.MaxValue; // Начинаем с бесконечного расстояния

        foreach (var player in Players) // Для каждого игрока в команде
        {
            var distance = player.GetDistanceToBall(); // Получаем расстояние до мяча
            if (distance < bestDistance) // Если это ближайший игрок
            {
                closestPlayer = player; // Обновляем ближайшего игрока
                bestDistance = distance; // Обновляем лучшее расстояние
            }
        }

        return closestPlayer; // Возвращаем ближайшего игрока
    }

    // Метод для обновления состояний в команде
    public void Move()
    {
        GetClosestPlayerToBall().MoveTowardsBall(); // Двигаем ближайшего игрока к мячу
        Players.ForEach(player => player.Move()); // Двигаем всех игроков команды
    }
}