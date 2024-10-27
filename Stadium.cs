namespace Football_DavidMyrsethTARpv23
{
    public class Stadium
    {
        public Stadium(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        // Проверяет, находится ли точка с координатами (x, y) внутри стадиона
        public bool IsIn(int x, int y)
        {
            // Возвращает true, если координаты x и y лежат в пределах стадиона,
            // иначе false
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
}
