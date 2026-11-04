namespace SnakeGame;

public class FitnessEvaluator
{
    // для вычисление успешности нейронки по статистике
    public double Calculate(GameStats stats)
    {
        int score = stats.Score;
        int steps = stats.Steps;

        // double fitness = 10*score + 0.00008 * steps;

        // double fitness = 10*score + 0.00008 * steps + 0.01 * stats.FoodProgress;

        double fitness = 10 * score 
        + 0.00008 * steps 
        + 0.1 * stats.FoodProgress
        - 0.005 * stats.Turns;
        return fitness;
    }
}