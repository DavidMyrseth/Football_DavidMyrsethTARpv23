public class Stadium
{
    public int Width { get; }
    public int Height { get; }
    public (double, double) HomeGoal { get; }
    public (double, double) AwayGoal { get; }

    public Stadium(int width, int height)
    {
        Width = width;
        Height = height;
        // Define the goals on both sides of the field
        HomeGoal = (0, height / 2);
        AwayGoal = (width, height / 2);
    }

    public bool IsIn(double x, double y)
    {
        return x >= 0 && x <= Width && y >= 0 && y <= Height;
    }
}