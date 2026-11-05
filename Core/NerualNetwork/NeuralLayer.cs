namespace SnakeGame;

public class NeuralLayer
{
    private readonly Neuron[] neurons;
    private readonly int InputCount;

    public int ParameterCount => neurons.Length * (InputCount + 1);

    public NeuralLayer(int inputCount, int neuronCount)
    {
        InputCount = inputCount;
        neurons = new Neuron[neuronCount];
        for(int i = 0; i < neuronCount; i++)
        {
            neurons[i] = new Neuron(inputCount);
        }
    }

    public double[] CalculateLayer(double[] inputs)
    {

        if (inputs.Length != InputCount)
        {
            throw new ArgumentException("Неверное количество входных значений.");
        }

        double[] outputs = new double[neurons.Length];

        for(int i = 0; i < neurons.Length; i++)
        {
            outputs[i] = neurons[i].Forward(inputs);
        }

        return outputs;
    }

    public double[] GetGenome()
    {
        double[] genome = new double[neurons.Length*(InputCount + 1)];
        int index = 0;

        foreach(Neuron neuron in neurons)
        {
            double[] neuronGenome = neuron.GetGenome();

            for(int i = 0; i < neuronGenome.Length; i++)
            {
                genome[index++] = neuronGenome[i];
            }
        }

        return genome;
    }

    public void SetGenome(double[] genome)
    {
        if (genome.Length != neurons.Length * (InputCount + 1))
        {
            throw new ArgumentException("Неверный размер генома слоя.");
        }

        int index = 0;

        foreach (Neuron neuron in neurons)
        {
            double[] neuronGenome = new double[InputCount + 1];

            Array.Copy(genome, index, neuronGenome, 0, neuronGenome.Length);

            neuron.SetGenome(neuronGenome);

            index += neuronGenome.Length;
        }
    }
}