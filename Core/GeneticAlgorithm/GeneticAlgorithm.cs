namespace Core.GeneticAlgorithm;

public class GeneticAlgorithm
{
    private readonly Random random = new Random();

    // Мутация генома
    public void Mutate(
        double[] genome,
        double mutationRate,
        double mutationStrength)
    {
        for (int i = 0; i < genome.Length; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                double v = (random.NextDouble() * 2.0 - 1.0) * mutationStrength;

                genome[i] += v;
            }
        }
    }

    // Скрещивание двух родителей
    public double[] Crossover(double[] parent1, double[] parent2)
    {
        if (parent1.Length != parent2.Length)
        {
            throw new ArgumentException(
                "Количество параметров родителей не одинаково!"
            );
        }

        double[] child = new double[parent1.Length];

        for (int i = 0; i < parent1.Length; i++)
        {
            child[i] = random.NextDouble() > 0.5
                ? parent1[i]
                : parent2[i];
        }

        return child;
    }

    // Турнирный отбор
    public Individ TournamentSelection(
        Population population,
        int tournamentSize)
    {
        int populationLen = population.Count;

        if (populationLen == 0)
            throw new ArgumentException("Популяция пуста.");

        if (tournamentSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(tournamentSize));

        int bestIndex = random.Next(0, populationLen);

        for (int i = 1; i < tournamentSize; i++)
        {
            int index = random.Next(0, populationLen);

            if (population.Individuals[index].Fitness >
                population.Individuals[bestIndex].Fitness)
            {
                bestIndex = index;
            }
        }

        return population.Individuals[bestIndex];
    }

    // Получение элитных особей
    public List<Individ> GetElites(
        Population population,
        int eliteCount)
    {
        if (eliteCount < 0 || eliteCount > population.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(eliteCount));
        }

        List<Individ> elites = population.Individuals
            .OrderByDescending(individ => individ.Fitness)
            .Take(eliteCount)
            .ToList();

        List<Individ> eliteCopies = new();

        foreach (var origin in elites)
        {
            Individ copy = new Individ(
                origin.Network.GetArchitecture()
            );

            double[] genome = origin.Network.GetGenome();

            copy.Network.SetGenome(genome);
            copy.Fitness = origin.Fitness;

            eliteCopies.Add(copy);
        }

        return eliteCopies;
    }

    // Создание следующего поколения
    public Population NextGeneration(
        Population population,
        int eliteCount,
        int tournamentSize,
        double mutationRate,
        double mutationStrength)
    {
        if (population.Count == 0)
            throw new ArgumentException("Популяция пуста.");

        if (mutationStrength < 0)
            throw new ArgumentOutOfRangeException(nameof(mutationStrength));

        if (tournamentSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(tournamentSize));

        if (mutationRate < 0 || mutationRate > 1)
            throw new ArgumentOutOfRangeException(nameof(mutationRate));

        if (eliteCount < 0 || eliteCount > population.Count)
            throw new ArgumentOutOfRangeException(nameof(eliteCount));

        List<Individ> newGeneration = GetElites(population, eliteCount);

        while (newGeneration.Count < population.Count)
        {
            // Выбор родителей
            Individ parent1 = TournamentSelection(population, tournamentSize);
            Individ parent2 = TournamentSelection(population, tournamentSize);

            // Получение геномов
            double[] genome1 = parent1.Network.GetGenome();
            double[] genome2 = parent2.Network.GetGenome();

            // Скрещивание
            double[] childGenome = Crossover(genome1, genome2);

            // Мутация
            Mutate(childGenome, mutationRate, mutationStrength);

            // Создание потомка
            Individ child = new Individ(
                parent1.Network.GetArchitecture()
            );

            child.Network.SetGenome(childGenome);

            newGeneration.Add(child);
        }

        return new Population(newGeneration);
    }
}