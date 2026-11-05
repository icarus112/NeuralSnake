namespace SnakeGame;

public class Individ
{
    public NeuralNetwork Network {get;}
    public double Fitness {get; set;}

    public Individ(int[] layers)
    {
        Network = new NeuralNetwork(layers);
        Fitness = 0;
    }
}

class IndividEvaluator
{
    private readonly GameObservation observation = new();
    private readonly AIController controller = new();
    private readonly FitnessEvaluator fitnessEvaluator = new();

    // запуск игровой сессии с нейронкой и вычисление Fitness
    public void Evaluate(Individ individ)
    {
        GameEngine game = new GameEngine(25, 20);

        while (!game.IsGameOver)
        {
            double[] inputs = observation.GetState(game);
            double[] outputs = individ.Network.Forward(inputs);
            int action = individ.Network.GetAction(outputs);

            if (action != 0)
            {
                game.Stats.AddTurn();
            }

            Direction aidir = controller.GetDirection(game.Snake.CurrentDirection, action);

            game.Snake.ChangeDirection(aidir);

            game.Update();
        }

        individ.Fitness = fitnessEvaluator.Calculate(game.Stats);
        // Console.WriteLine(
        //     $"Score: {game.Stats.Score}, " +
        //     $"Steps: {game.Stats.Steps}, " +
        //     $"StepsSinceFood: {game.Stats.StepsSinceFoods}"
        // );
    }

    // Evaluate для всей популяции
    public void EvaluatePopulation(Population population)
    {
        IndividEvaluator evaluator = new IndividEvaluator();
        foreach(Individ individ in population.Individuals)
        {
            evaluator.Evaluate(individ);
        }
    }
}