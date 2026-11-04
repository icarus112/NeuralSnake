using SnakeGame;

public class TrainingManager
{
    private readonly GeneticAlgorithm geneticAlgorithm = new();
    private readonly IndividEvaluator evaluator = new();

    public NeuralNetwork Train(
        int generations,
        int populationSize,
        int numOfSaves)
    {
        int generation = 0;
        int gap = generations / numOfSaves;
        int count = 0;

        Population population = new Population(populationSize);
        NeuralNetwork? bestNetwork = null;
       
        while(generation < generations)
        {
            evaluator.EvaluatePopulation(population);
            
            int bestFitness = 0;
            Individ bestIndivid = population.Individuals
            .MaxBy(individ => individ.Fitness)!;
            double avgFitness = population.Individuals
            .Average(individ => individ.Fitness);
            double bestFitnessEver = double.NegativeInfinity;
            
            if (bestIndivid.Fitness > bestFitnessEver)
            {
                bestFitnessEver = bestIndivid.Fitness;
                bestNetwork = new NeuralNetwork();
                bestNetwork.SetGenome(
                    bestIndivid.Network.GetGenome()
                );
            }
            
            Console.WriteLine($"GNR: {generation+1} |" +
            $"Best: {bestIndivid.Fitness} |" +
            $"Avg: {avgFitness}");

            if(generation < generations - 1)
            {
                population = geneticAlgorithm.NextGeneration(population, 5, 5, 0.1, 0.1);
            }
 
            if(count >= gap || generation == generations - 1)
            {
                ModelStorage.Save(
                    bestIndivid.Network,
                    $"Model_3/best_snake_{generation}.json"
                );
                count = 0;
            }

            generation++;
            count++;

        }   

        return bestNetwork;            
    }
}