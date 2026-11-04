using System.Text.Json;
namespace SnakeGame;
public static class ModelStorage
{
    public static void Save(
        NeuralNetwork network,
        string path
    )
    {
        double[] genome = network.GetGenome();

        string json = JsonSerializer.Serialize(genome);

        File.WriteAllText(path, json);
    }

    public static NeuralNetwork Load(string path)
    {
        string json = File.ReadAllText(path);

        double[] genome = JsonSerializer.Deserialize<double[]>(json)
            ?? throw new InvalidDataException("Invalid genome file");

        NeuralNetwork network = new NeuralNetwork();

        network.SetGenome(genome);

        return network;
    }
}