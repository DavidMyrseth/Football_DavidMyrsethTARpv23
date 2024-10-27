namespace Football_DavidMyrsethTARpv23
{
    class Program
    {
        // Размер стадиона
        static void Main(string[] args)
        {

            Stadium stadium = new Stadium(40, 20); 

            Team homeTeam = new Team("Home");
            Team awayTeam = new Team("Away");

            // Добавляем игроков в команды
            for (int i = 0; i < 11; i++)
            {
                homeTeam.AddPlayer(new Player($"HomePlayer{i + 1}"));
                awayTeam.AddPlayer(new Player($"AwayPlayer{i + 1}"));
            }

            // создаем
            Game game = new Game(homeTeam, awayTeam, stadium);
            game.Start();


            while (true)
            {
                // Обновляем состояние игры
                game.Move();

                PrintGameState(game, stadium);

                // Задержка для визуализации
                System.Threading.Thread.Sleep(500);

                // Проверка нажатия клавиши для выхода
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    // Выход по нажатию Spacebar
                    if (key.Key == ConsoleKey.Spacebar) 
                    {
                        break;
                    }
                }
            }
        }
        // Вывод текущего состояния игры
        static void PrintGameState(Game game, Stadium stadium)
        {
            Console.Clear();

            int width = game.Stadium.Width;
            int height = game.Stadium.Height;

            // Двумерный массив, представляющий игровое поле
            char[,] field = new char[height, width]; 

            // Создаем пустое поле
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    field[y, x] = ' ';
                }
            }

            // Размещаем игроков домашней команды
            foreach (var player in game.HomeTeam.Players)
            {
                int playerX = (int)player.X;
                int playerY = (int)player.Y;

                // нужен для того чтобы игрок не выходил из поля
                if (stadium.IsIn(playerX, playerY)) 
                {
                    // задают символ где находится игрок
                    field[playerY, playerX] = 'H'; 
                }
            }

            // Размещаем игроков выездной команды
            foreach (var player in game.AwayTeam.Players)
            {
                int playerX = (int)player.X;
                int playerY = (int)player.Y;
                if (stadium.IsIn(playerX, playerY))
                {
                    field[playerY, playerX] = 'A';
                }
            }

            // Размещаем мяч
            int ballX = (int)game.Ball.X;
            int ballY = (int)game.Ball.Y;
            if (stadium.IsIn(ballX, ballY)) 
            {
                field[ballY, ballX] = 'O';
            }

            // Выводим поле на экран
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Печатаем символы игрового поля
                    Console.Write(field[y, x]); 
                }
                // Переход на новую строку после каждой строки поля
                Console.WriteLine(); 
            }
        }
    }
}