using Core.Neural;
using SnakeTasks.Game;
using Core.GeneticAlgorithm;

namespace SnakeTasks.AI;


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
    private readonly SnakeController controller = new();
    private readonly SnakeEvaluator fitnessEvaluator = new();

    // запуск игровой сессии с нейронкой и вычисление Fitness
    public void Evaluate(Individ individ)
    {
        const int episodes = 5;

        double totalFitness = 0;

        for (int episode = 0; episode < episodes; episode++)
        {
            GameEngine game = new GameEngine(
                25,
                20,
                new Random(1000 + episode)
            );

            while (!game.IsGameOver)
            {
                double[] inputs = observation.GetState(game);

                double[] outputs = individ.Network.Forward(inputs);

                int action = individ.Network.GetAction(outputs);

                Direction direction = controller.GetDirection(
                    game.Snake.CurrentDirection,
                    action
                );

                Direction previousDirection = game.Snake.CurrentDirection;

                game.Snake.ChangeDirection(direction);

                if (game.Snake.CurrentDirection != previousDirection)
                {
                    game.Stats.AddTurn();
                }

                game.Update();
            }

            totalFitness += fitnessEvaluator.Calculate(game.Stats);
        }

        individ.Fitness = totalFitness / episodes;       // Console.WriteLine(
            //     $"Score: {game.Stats.Score}, " +
            //     $"Steps: {game.Stats.Steps}, " +
            //     $"StepsSinceFood: {game.Stats.StepsSinceFoods}"
            // );
}

    // Evaluate для всей популяции
    public void EvaluatePopulation(Population population)
    {
        IndividEvaluator evaluator = new IndividEvaluator();
        
        // foreach (Individ individ in population.Individuals)
        // {
        //     evaluator.Evaluate(individ);
        // }
        Parallel.ForEach(
            population.Individuals,
            individ =>
            {
                evaluator.Evaluate(individ);
            }
        );
    }
}