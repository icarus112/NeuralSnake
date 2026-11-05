namespace SnakeTasks.Game;

public class Food
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Food(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public void NewFood(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}