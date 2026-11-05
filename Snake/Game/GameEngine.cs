using SnakeGame;

public class GameEngine
{
    public Snake Snake { get; private set; }
    public Food Food { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsWin { get; private set; }
    public int GridSize {get; private set;}
    public GameStats Stats { get; private set; }
    private GameRenderer Renderer { get;}
    private readonly Random random;

    public GameEngine(int cellSize, int gridSize,
        Random? random = null)
    {
        this.random = random ?? Random.Shared;
        Renderer = new GameRenderer(cellSize, gridSize);
        Snake = new Snake(10, 10);
        Food = new Food(0, 0);
        GridSize = gridSize;

        IsGameOver = false;
        IsWin = false;
        Stats = new GameStats();
        Stats.NextEpisode();

        SpawnFood();
    }

    private void SpawnFood()
    {
        int x, y;

        do
        {
            x = random.Next(0, Renderer.GetGridSize);
            y = random.Next(0, Renderer.GetGridSize);
        }
        while (Snake.GetBody.Any(segment => segment.X == x && segment.Y == y));

        Food.NewFood(x, y);
    }

    private bool CheckWallCollision()
    {   
        

        return Snake.X < 0 || 
        Snake.X >= Renderer.GetGridSize || 
        Snake.Y < 0 ||
        Snake.Y >= Renderer.GetGridSize;
    }

    private bool CheckSelfCollision()
    {
        return Snake.GetBody.Skip(1)
            .Any(segment => segment.X == Snake.X && 
            segment.Y == Snake.Y);
    }

    public void Update()
    {
        if (IsGameOver) return;
        
        // используется Манхеттенское расстояние для расстоянии до еды
        int distanceBefore =
            Math.Abs(Snake.X - Food.X) +
            Math.Abs(Snake.Y - Food.Y);

        Snake.Move();


        int distanceAfter =             
            Math.Abs(Snake.X - Food.X) +
            Math.Abs(Snake.Y - Food.Y);

        int progress = distanceBefore - distanceAfter;

        Stats.AddStep();

        if (CheckWallCollision() || CheckSelfCollision())
        {
            IsGameOver = true;
            return;
        }

        Stats.AddFoodProgress(progress);

        if (Snake.X == Food.X && Snake.Y == Food.Y)
        {
            Snake.Eating(Food.X, Food.Y);

            Stats.AddScore();

            if (Snake.GetBody.Count == Renderer.GetGridSize*Renderer.GetGridSize)
            {
                IsWin = true;
                IsGameOver = true;
                return;
            }

            SpawnFood();
        }

        int maxStepsWithoutFood = 2 * (30 + 5 * Stats.Score);

        if (Stats.StepsSinceFoods >= maxStepsWithoutFood)
        {
            IsGameOver = true;
            return;
        }
    }

    public void Restart()
    {
        Snake = new Snake(10, 10);
        Food = new Food(0, 0);

        IsGameOver = false;
        IsWin = false;

        Stats.NextEpisode();

        SpawnFood();
    } 
}