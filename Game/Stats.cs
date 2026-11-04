public class GameStats
{
    public int Score { get; private set; }
    public int Steps { get; private set; }
    public int StepsSinceFoods { get; private set; }
    public int Episode { get; private set; }
    public double FoodProgress{get; private set;}

    public int Turns {get; private set;}

    public GameStats()
    {
        Score = 0;
        Steps = 0;
        StepsSinceFoods = 0;
        Episode = 0;
    }

    public void AddStep()
    {
        Steps++;
        StepsSinceFoods++;
    }

    public void AddScore()
    {
        Score++;
        StepsSinceFoods = 0;
    }
    public void NextEpisode()
    {
        Episode++;
        Score = 0;
        Steps = 0;
        StepsSinceFoods = 0;
    }

    // прогресс движение в сторону еды
    public void AddFoodProgress(double progress)
    {
        FoodProgress += progress;
    }

    public void AddTurn()
    {
        Turns++;
    }
} 