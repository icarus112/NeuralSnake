using System.Text.Json;
namespace SnakeGame;

public class ModelData
{
    public int[] Layers { get; set; } = Array.Empty<int>();
    public double[] Genome { get; set; } = Array.Empty<double>();
}
public static class ModelStorage
{
    public static void Save(NeuralNetwork network, string path)
    {
        ModelData model = new ModelData
        {
            Layers = network.GetArchitecture(),
            Genome = network.GetGenome()
        };

        string json = JsonSerializer.Serialize(
            model,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    public static NeuralNetwork Load(string path)
    {
        string json = File.ReadAllText(path);

        ModelData model = JsonSerializer.Deserialize<ModelData>(json)
            ?? throw new InvalidDataException("Invalid model file");

        NeuralNetwork network = new NeuralNetwork(model.Layers);

        network.SetGenome(model.Genome);

        return network;
    }
}