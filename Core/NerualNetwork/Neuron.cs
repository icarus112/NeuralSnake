namespace Core.Neural;

public class Neuron
{
    private readonly double[] weights;
    private double bias;
    public Neuron(int inputCount)
    {
        weights = new double[inputCount];
        for (int i = 0; i < inputCount; i++)
        {
            weights[i] = (Random.Shared.NextDouble() * 2.0) - 1.0;
        }
        bias = (Random.Shared.NextDouble() * 2.0) - 1.0;    
    }

    public double Forward(double[] inputs)
    {

        if (inputs.Length != weights.Length)
        {
            throw new ArgumentException("Количество входов не соответствует количеству весов.");
        }
        double z = 0;
        for(int input = 0; input < inputs.Length; input++)
        {
            z += inputs[input] * weights[input];
        }
        z += bias;
        double output = Math.Tanh(z);

        return output;
    }

    public double[] GetGenome()
    {
        double[] genome = new double[weights.Length + 1];
        for(int i = 0; i < weights.Length; i++)
        {
            genome[i] = weights[i];
        }

        genome[weights.Length] = bias;

        return genome;
    }

    public void SetGenome(double[] genome)
    {
        if (genome.Length != weights.Length + 1)
        {
            throw new ArgumentException("Неверный размер генома.");
        }

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = genome[i];
        }

        bias = genome[weights.Length];
    }
}