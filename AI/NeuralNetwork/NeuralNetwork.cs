namespace SnakeGame;

public class NeuralNetwork
{
    private readonly double[,] weightsIH = new double[11,16]; 
    private readonly double[,] weightsHH = new double[16,16]; 
    private readonly double[,] weightsHO = new double[16,3]; 

    private readonly double[] biasH1 = new double[16]; 
    private readonly double[] biasH2 = new double[16]; 
    private readonly double[] biasOutPut = new double[3]; 
    private readonly Random random = new Random();

    // при инициализации слои нейронки заполняется рандомными числами 
    public NeuralNetwork()
    {
        FillWeights(weightsIH);
        FillWeights(weightsHH);
        FillWeights(weightsHO);

        FillBias(biasH1);
        FillBias(biasH2);
        FillBias(biasOutPut);
    }
    
    // заполнение рандомными числами -1:1 весов
    private void FillWeights(double[,] weights)
    {
        for (int i = 0; i < weights.GetLength(0); i++)
        {          
            for(int j = 0; j < weights.GetLength(1); j++)
            {
                weights[i, j] = (random.NextDouble() * 2.0) - 1.0;
            }
        }
    }

    // заполнение рандомными числами -1:1 смещений
    private void FillBias(double[] bias)
    {
        for(int j = 0; j < bias.Length; j++)
        {
            bias[j] = (random.NextDouble() * 2.0) - 1.0;
        }
    }
    
    // вычисление выходных параметров
    private double[] CalculateLayer(
        double[] inputs,
        double[,] weights,
        double[] bias
    )
    {
        double[] output = new double[weights.GetLength(1)];
        for(int neuron = 0; neuron < weights.GetLength(1); neuron++)
        {
            double sum = 0;

            for(int input = 0; input < inputs.Length; input++)
            {
                sum += inputs[input] * weights[input, neuron];
            }

            sum += bias[neuron];

            output[neuron] = Math.Tanh(sum);
        }

        return output;
    }
    
    // получение выходных значений output от input
    public double[] Forward(double[] inputs)
    {
        double[] Out1 = CalculateLayer(inputs, weightsIH, biasH1);
        double[] Out2 = CalculateLayer(Out1, weightsHH, biasH2);
        double[] Out3 = CalculateLayer(Out2, weightsHO, biasOutPut);

        return Out3;
    }

    // получение оптимального от output
    public int GetAction(double[] outputs)
    {
        int index = 0;
        for (int i = 1; i < outputs.Length; i++)
        {
            if (outputs[i] > outputs[index])
            {
                index = i;
            }
        }

        return index;
    }

    // для получения генома нейронки
    public double[] GetGenome()
    {
        double[] genome = new double[515];
        int index = 0;

        foreach (double weight in weightsIH)
        {
            genome[index++] = weight;
        }

        foreach (double weight in weightsHH)
        {
            genome[index++] = weight;
        }

        foreach (double weight in weightsHO)
        {
            genome[index++] = weight;
        }

        foreach (double bias in biasH1)
        {
            genome[index++] = bias;
        }

        foreach (double bias in biasH2)
        {
            genome[index++] = bias;
        }

        foreach (double bias in biasOutPut)
        {
            genome[index++] = bias;
        }
        return genome;
    }

    private void LoadWeights(
        double[,] weights, double[] genome, ref int index
    )
    {
        for(int i = 0; i < weights.GetLength(0); i++)
        {
            for(int j = 0; j < weights.GetLength(1); j++)
            {
                weights[i, j] = genome[index++];
            }
        }
    }
    private void LoadBias(double[] bias, double[] genome, ref int index)
    {
        for (int i = 0; i < bias.Length; i++)
        {
            bias[i] = genome[index++];
        }
    }

    // Получение весов и смещений из генома
    public void SetGenome(double[] genome)
    {
        ArgumentNullException.ThrowIfNull(genome);

        if (genome.Length != 515)
        {
            throw new ArgumentException(
                "Invalid genome size",
                nameof(genome)
            );
        }

        int index = 0;

        LoadWeights(weightsIH, genome, ref index);
        LoadWeights(weightsHH, genome, ref index);
        LoadWeights(weightsHO, genome, ref index);

        LoadBias(biasH1, genome, ref index);
        LoadBias(biasH2, genome, ref index);
        LoadBias(biasOutPut, genome, ref index);
    }
}