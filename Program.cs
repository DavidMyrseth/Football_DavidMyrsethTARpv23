namespace Football_DavidMyrsethTARpv23
{
    class Program
    {
        static void Main(string[] args)
        {
            Stadium stadium = new Stadium(40, 20); // Увеличиваем размер стадиона


            Team homeTeam = new Team("Home");
            Team awayTeam = new Team("Away");


            for (int i = 0; i < 11; i++)// Добавляем игроков в команды
            {
                homeTeam.AddPlayer(new Player($"HomePlayer{i + 1}"));
                awayTeam.AddPlayer(new Player($"AwayPlayer{i + 1}"));
            }


            Game game = new Game(homeTeam, awayTeam, stadium);
            game.Start();


            while (true)// Бесконечный игровой цикл
            {
                game.Move();
                PrintGameState(game, stadium);
                System.Threading.Thread.Sleep(500); // Задержка для визуализации


                if (Console.KeyAvailable)// Проверка нажатия клавиши для выхода
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape) // Выход по нажатию ESC
                    {
                        break;
                    }
                }
            }
        }

        static void PrintGameState(Game game, Stadium stadium)
        {
            Console.Clear();

            int width = game.Stadium.Width;
            int height = game.Stadium.Height;
            char[,] field = new char[height, width];

            // Создаем пустое поле
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    field[y, x] = ' '; // Пустое пространство
                }
            }

            // Размещаем игроков домашней команды
            foreach (var player in game.HomeTeam.Players)
            {
                int playerX = (int)player.X;
                int playerY = (int)player.Y;
                if (stadium.IsIn(playerX, playerY)) // нужен для того чтобы игрок не выходил из поля
                {
                    field[playerY, playerX] = 'H'; // задают символ где находится игрок
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


            // Отображаем ворота
            for(int x = 0; x < 8; x++) // пока x меньше 8, он будет работать
            {
                field[height / 2 + 3 - x, 0] = 'X'; // Ворота домашней команды
                field[height / 2 + 3 - x, width - 1] = 'X'; // Ворота выездной команды
            }


            // Выводим счёт
            Console.WriteLine($"{new string (' ', (int)((float)width /  1.5f))}Score: {game.HomeTeam.Name} {game.HomeTeam.Score} - {game.AwayTeam.Score} {game.AwayTeam.Name}\n");
            // Преобразование флота в int

            // Выводим поле с рамкой
            Console.WriteLine(new string('#', width * 2 + 3)); // Верхняя рамка

            for (int y = 0; y < height; y++)
            {
                Console.Write("# "); // Левая рамка
                for (int x = 0; x < width; x++)
                {
                    Console.Write(field[y, x] + " ");
                }
                Console.WriteLine("#"); // Правая рамка
            }

            Console.WriteLine(new string('#', width * 2 + 3)); // Нижняя рамка
        }
    }
}