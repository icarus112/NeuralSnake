namespace SnakeGame;

// Для запоминания направления движения змейки
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Snake
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Direction CurrentDirection {get; private set; } // for automatic move per sec
    private List<(int X, int Y)> body;
    private (int X, int Y) previousTail;
    public List<(int X, int Y)> GetBody => body;

    public Snake(int x, int y)
    {
        X = x;
        Y = y;

        CurrentDirection = Direction.Right;
        body = new List<(int X, int Y)>
        {
            (x, y),
            (x -1, y),
            (x - 2, y)
        };
    }

    public void MoveRight() => X++;
    public void MoveLeft() => X--;
    public void MoveUp() => Y--;
    public void MoveDown() => Y++;

    public void ChangeDirection(Direction direction)
    {
    if (CurrentDirection == Direction.Up && direction == Direction.Down ||
        CurrentDirection == Direction.Down && direction == Direction.Up ||
        CurrentDirection == Direction.Left && direction == Direction.Right ||
        CurrentDirection == Direction.Right && direction == Direction.Left)
    {
        return;
    }
        CurrentDirection = direction;
    }

    public void Move()
    {   
        previousTail = body[^1];
        switch (CurrentDirection)
        {
            case Direction.Right:
                MoveRight();
                break;

            case Direction.Left:
                MoveLeft();
                break;

            case Direction.Up:
                MoveUp();
                break;

            case Direction.Down:
                MoveDown();
                break;
        }

        body.Insert(0, (X, Y));
        body.RemoveAt(body.Count - 1);
    }

    public void Eating(int x, int y)
    {
        body.Add(previousTail);
    }
}