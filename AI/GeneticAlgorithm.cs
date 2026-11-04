using System.Net.NetworkInformation;
using SnakeGame;

public class GeneticAlgorithm
{
    private readonly Random random = new Random();

    // мутирует геном для поучения изменений нейронки, может стать лучше или хуже 
    public void Mutate(
        double[] genome,
        double mutationRate,
        double mutationStrength
    )
    {
        for(int i = 0; i < genome.Length; i++)
        {
            if (random.NextDouble() < mutationRate)
            {
                double v = (random.NextDouble() * 2.0 - 1.0) * mutationStrength;

                genome[i] += v; 
            }   
        }
    }

    // скрещивает геном 2 нейронок, поучение одного гена от одного из родителей 50%
    public double[] Crossover(double[] parent1, double[] parent2)
    {        
        if(parent1.Length != parent2.Length)
        {
            throw new ArgumentException("число параметров родителей не одинаково!");
        }
        double[] child = new double[parent1.Length];

        for(int i = 0; i < parent1.Length; i++)
        {
            double v = random.NextDouble();
            if(v > 0.5)
            {
                child[i] = parent1[i];
            }
            else
            {
                child[i] = parent2[i];
            }
        }

        return child;
    }

    // отбор для 3 случайных особей популяции
    public Individ TournamentSelection(
        Population population,
        int tournamentSize
    )
    {
        int populationLen = population.Individuals.Count();
        int bestIndivid = random.Next(0, populationLen);

        for(int i = 0; i < tournamentSize; i++)
        {
            int inx = random.Next(1, populationLen); 
            if(population.Individuals[inx].Fitness > population.Individuals[bestIndivid].Fitness)
            {
                bestIndivid = inx;
            }
        }

        return population.Individuals[bestIndivid];
    }

    public List<Individ> GetElites(
        Population population,
        int eliteCount
    )
    {
        List<Individ> elites = population.Individuals
        .OrderByDescending(individ => individ.Fitness)
        .Take(eliteCount)
        .ToList();
        List<Individ> eliteCopies = new();

        foreach (var origin in elites)
        {
            Individ copy = new Individ();

            double[] genome = origin.Network.GetGenome();
            copy.Network.SetGenome(genome);
            eliteCopies.Add(copy);
        }

        return eliteCopies;
    }

    // Создание новой генерации(популяции) с помощью Elitism, tournamentSelection
    public Population NextGeneration(
        Population population,
        int eliteCount,
        int tournamentSize,
        double mutationRate,
        double mutationStrength
    )
    {
        if(population.Count == 0)
        {
            throw new ArgumentException(nameof(population));
        }

        if (mutationStrength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mutationStrength));
        }

        if(tournamentSize <= 0)
        {
            throw new ArgumentException(nameof(tournamentSize));
        }

        if(mutationRate < 0 || mutationRate >1)
        {
            throw new ArgumentException(nameof(mutationRate));
        }

        if (eliteCount < 0 || eliteCount > population.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(eliteCount));
        }

        List<Individ> newGeneration = GetElites(population, eliteCount);
        while(newGeneration.Count < population.Count)
        {
            Individ parent1 = TournamentSelection(population, tournamentSize);
            Individ parent2 = TournamentSelection(population, tournamentSize);
            
            double[] genome1 = parent1.Network.GetGenome();
            double[] genome2 = parent2.Network.GetGenome();

            double[] crossover = Crossover(genome1, genome2);
            Mutate(crossover, mutationRate, mutationStrength);
            Individ child = new Individ();
            child.Network.SetGenome(crossover);

            newGeneration.Add(child);
        }

        Population newPopulation = new Population(newGeneration); 

        return newPopulation;
    }
}