namespace Football_DavidMyrsethTARpv23
{
    class Program
    {
        static void Main(string[] args)
        {
            Stadium stadium = new Stadium(40, 20); // Увеличиваем размер стадиона

            Team homeTeam = new Team("Home");
            Team GuestTeam = new Team("Guest");

            for (int i = 0; i < 11; i++)// Добавляем игроков в команды
            {
                homeTeam.AddPlayer(new Player($"HomePlayer{i + 1}"));
                GuestTeam.AddPlayer(new Player($"GuestPlayer{i + 1}"));
            }

            Game game = new Game(homeTeam, GuestTeam, stadium);
            game.Start();

            while (true)
            {
                game.Move();
                PrintGameState(game);
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

        static void PrintGameState(Game game)
        {
            Console.Clear();

            int width = game.Stadium.Width;
            int height = game.Stadium.Height;
            char[,] field = new char[height, width];

            // Fill the field with spaces (invisible background)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    field[y, x] = ' '; // Empty background
                }
            }

            // Add players from the home team
            for (int i = 0; i < game.HomeTeam.Players.Count; i++)
            {
                var player = game.HomeTeam.Players[i];
                int playerX = (int)player.X;
                int playerY = (int)player.Y;
                if (playerX >= 0 && playerX < width && playerY >= 0 && playerY < height)
                {
                    // Use a unique character for each home player
                    field[playerY, playerX] = (char)('H' + i); // Unique character for each home player
                }
            }

            // Add players from the guest team
            for (int i = 0; i < game.GuestTeam.Players.Count; i++)
            {
                var player = game.GuestTeam.Players[i];
                int playerX = (int)player.X;
                int playerY = (int)player.Y;
                if (playerX >= 0 && playerX < width && playerY >= 0 && playerY < height)
                {
                    // Use a unique character for each guest player
                    field[playerY, playerX] = (char)('G' + i); // Unique character for each guest player
                }
            }

            // Add the ball
            int ballX = (int)game.Ball.X;
            int ballY = (int)game.Ball.Y;
            if (ballX >= 0 && ballX < width && ballY >= 0 && ballY < height)
            {
                field[ballY, ballX] = 'O'; // Represent the ball with 'O'
            }

            // Display extended goals (5 cells wide)
            for (int i = -2; i <= 2; i++)
            {
                if (height / 2 + i >= 0 && height / 2 + i < height)
                {
                    field[height / 2 + i, 0] = 'X'; // Goal for the home team
                    field[height / 2 + i, width - 1] = 'X'; // Goal for the guest team
                }
            }

            // Print the score
            Console.WriteLine($"Score: {game.HomeTeam.Name} {game.HomeTeam.Score} - {game.GuestTeam.Score} {game.GuestTeam.Name}\n");

            // Print the upper boundary of the field
            Console.WriteLine(new string('#', (width + 2) * 2));

            // Print the playing field with side borders
            for (int y = 0; y < height; y++)
            {
                Console.Write("# "); // Left border
                for (int x = 0; x < width; x++)
                {
                    Console.Write(field[y, x] + " "); // Print field contents
                }
                Console.WriteLine("#"); // Right border
            }

            // Print the lower boundary of the field
            Console.WriteLine(new string('#', (width + 2) * 2));
        }
    }
}