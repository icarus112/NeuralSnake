using SnakeTasks.Game;

namespace SnakeTasks.AI;


public class SnakeController
{

    public Direction GetDirection(Direction curdir, int action)
    {
        switch (curdir)
        {
            case Direction.Up:
                if(action == 0)
                    return Direction.Up;
                if(action == 1)
                    return Direction.Right;
                if(action == 2)
                    return Direction.Left;
                break;
            case Direction.Down:
                if(action == 0)
                    return Direction.Down;
                if(action == 1)
                    return Direction.Left;
                if(action == 2)
                    return Direction.Right;
                break;
            case Direction.Right:
                if(action == 0)
                    return Direction.Right;
                if(action == 1)
                    return Direction.Down;
                if(action == 2)
                    return Direction.Up;
                break;
            case Direction.Left:
                if(action == 0)
                    return Direction.Left;
                if(action == 1)
                    return Direction.Up;
                if(action == 2)
                    return Direction.Down;
                break;
        };

        throw new ArgumentOutOfRangeException(
            nameof(action),
            "Invalid direction or action"
        );
    }
}