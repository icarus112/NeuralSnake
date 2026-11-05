using SnakeTasks.Game;


namespace SnakeTasks.AI;

public class GameObservation
{
    public double[] GetState(GameEngine game)
    {
        Snake snake = game.Snake;
        Food food = game.Food;

        double[] state = new double[11];

        // для того что бы определить какая координата 
        // следующей клетки перемещения по направлению движения
        // относительно плоскости
        (int dx, int dy) = snake.CurrentDirection switch
        {
            Direction.Up => (0, -1),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Right => (1, 0),
            _ => (0,0)
        };

        // опасности
        // используется мат. преобразование вектора при поворотах
        // относительно змее
        state[0] = isDanger(game, snake.X + dx, snake.Y + dy) ? 1.0 : 0.0; // спереди
        state[1] = isDanger(game, snake.X - dy, snake.Y + dx)? 1.0 : 0.0; // справа
        state[2] = isDanger(game, snake.X + dy, snake.Y - dx)? 1.0 : 0.0; // слева

        // направление движения
        state[3] = snake.CurrentDirection == Direction.Up ? 1.0 : 0.0;
        state[4] = snake.CurrentDirection == Direction.Down ? 1.0 : 0.0;
        state[5] = snake.CurrentDirection == Direction.Left ? 1.0 : 0.0;
        state[6] = snake.CurrentDirection == Direction.Right ? 1.0 : 0.0;
        
        // положение еды
        state[7] = food.Y < snake.Y ? 1.0 : 0.0; //еда сверху
        state[8] = food.Y > snake.Y ? 1.0 : 0.0; // еда снизу
        state[9] = food.X < snake.X ? 1.0 : 0.0; // еда слева
        state[10] = food.X > snake.X ? 1.0 : 0.0; // еда справа

        return state;
    }

    public bool isDanger(
        GameEngine game,
        int x,
        int y
    )
    {
        if (x < 0 || x >= game.GridSize ||
            y < 0 || y >= game.GridSize)
        {
            return true;
        }

        var body = game.Snake.GetBody;

        for (int i = 1; i < body.Count - 1; i++)
        {
            if (body[i].X == x && body[i].Y == y)
                return true;
        }

        return false;
    }
}