using Raylib_cs;

namespace SnakeTasks.Game;

public class GameRenderer
{
    private readonly int cellSize;
    private readonly int gridSize;

    public GameRenderer (int cellSize, int gridSize)
    {
        this.cellSize = cellSize;
        this.gridSize = gridSize;
    }

    public int GetGridSize => gridSize;
    public int GetCellSize => cellSize;

    public void DrawGrid()
    {
        // генерация поля
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Raylib.DrawRectangle(
                    i * cellSize, 
                    j * cellSize, 
                    cellSize, 
                    cellSize, 
                    Color.DarkGray);

                Raylib.DrawRectangleLines(
                    i * cellSize,
                    j * cellSize,
                    cellSize,
                    cellSize,
                    new Color(60, 10, 90, 255));
            }
        }
    }

    public void DrawSnake(Snake snake)
    {
        foreach (var segment in snake.GetBody)
        {
            Raylib.DrawRectangle(
                segment.X * cellSize, 
                segment.Y * cellSize, 
                cellSize, 
                cellSize, 
                Color.Green);

            Raylib.DrawRectangleLines(
                segment.X * cellSize, 
                segment.Y * cellSize, 
                cellSize,
                cellSize,
                new Color(60, 100, 90, 255));
        }
    }

    public void DrawFood(Food food)
    {
        Raylib.DrawRectangle(
            food.X * cellSize, 
            food.Y * cellSize, 
            cellSize, 
            cellSize, 
            Color.Red);
    }

    public void Draw(GameEngine game)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);

        DrawGrid();
        DrawSnake(game.Snake);
        DrawFood(game.Food);
        DrawStats(game.Stats);

        if (game.IsGameOver)
        {
            string message = game.IsWin ? "YOU WIN!" : "GAME OVER";
            Raylib.DrawText(message, 150, 220, 30, Color.White);
            Raylib.DrawText("Press ESC to exit", 150, 260, 20, Color.LightGray);
            Raylib.DrawText("Press r to restart", 150, 280, 20, Color.LightGray);

        }
        
        Raylib.EndDrawing();
    }

    public void DrawStats(GameStats stats)
    {
        Raylib.DrawText(
            $"Score: {stats.Score}",
            10, 10,
            20,
            Color.White
        );

        Raylib.DrawText(
            $"Steps: {stats.Steps}",
            10, 35,
            20,
            Color.White
        );
    }

}