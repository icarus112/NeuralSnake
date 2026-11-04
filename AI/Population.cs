namespace SnakeGame;

public class Population
{
    private readonly List<Individ> individs = new();
    public IReadOnlyList<Individ> Individuals => individs;

    public int Count => individs.Count;

    public Population(int PopulationSize)
    {
        for (int i = 0; i < PopulationSize; i++)
        {
            individs.Add(new Individ());
        }
    }

    public Population(List<Individ> newIndividuals)
    {
        individs = new List<Individ>(newIndividuals);
    }
}