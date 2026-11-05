using Core.Neural;
using Core.GeneticAlgorithm;
using Core.ModelStorage;

namespace SnakeTasks.AI;

public class SnakeTrain
{
    private readonly GeneticAlgorithm geneticAlgorithm = new();
    private readonly IndividEvaluator evaluator = new();

    public NeuralNetwork Train(
        int generations,
        int populationSize,
        int numOfSaves,
        int[] layers)
    {
        if (generations <= 0)
            throw new ArgumentException("Количество поколений должно быть положительным.");

        if (numOfSaves <= 0)
            throw new ArgumentException("Количество сохранений должно быть положительным.");

        int generation = 0;

        double bestFitnessEver = double.NegativeInfinity;

        NeuralNetwork? bestNetwork = null;

        Population population = new Population(populationSize, layers);

        Directory.CreateDirectory("/SnakeModels/Model_5");

        int saveTarget = Math.Min(numOfSaves, generations);
        int savesDone = 0;

        while (generation < generations)
        {
            evaluator.EvaluatePopulation(population);

            Individ bestIndivid = population.Individuals
                .MaxBy(individ => individ.Fitness)!;

            double avgFitness = population.Individuals
                .Average(individ => individ.Fitness);

            // Проверяем лучший результат за всё время обучения.
            if (bestIndivid.Fitness > bestFitnessEver)
            {
                bestFitnessEver = bestIndivid.Fitness;

                bestNetwork = new NeuralNetwork(layers);

                bestNetwork.SetGenome(
                    bestIndivid.Network.GetGenome()
                );
            }

            Console.WriteLine(
                $"GNR: {generation + 1} | " +
                $"Best: {bestIndivid.Fitness:F2} | " +
                $"Best Ever: {bestFitnessEver:F2} | " +
                $"Avg: {avgFitness:F2}"
            );

            // Создаём следующее поколение.
            if (generation < generations - 1)
            {
                population = geneticAlgorithm.NextGeneration(
                    population, 5, 5, 0.3, 0.1
                );
            }

            // Определяем, пора ли сохранять модель.
            int nextSaveGeneration = (int)Math.Ceiling(
                (savesDone + 1.0) * generations / saveTarget
            );

            if (generation + 1 >= nextSaveGeneration)
            {
                ModelStorage.Save(
                    bestNetwork!,
                    $"/SnakeModels/Model_5/best_snake_{generation + 1}.json"
                );

                savesDone++;
            }

            generation++;
        }

        return bestNetwork
            ?? throw new InvalidOperationException("Обучение не завершилось успешно.");
    }
}