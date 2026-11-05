namespace Core.Neural;

//Количество нейронов определяет размер выхода слоя.
//Количество весов одного нейрона определяется размером входа слоя.
public class NeuralNetwork
{
    private readonly NeuralLayer[] neuralLayers;

    private readonly int[] layerSizes;

    public int[] GetArchitecture()
    {
        return (int[])layerSizes.Clone();
    }

    public NeuralNetwork(int[] layers)
    {
        if (layers == null || layers.Length < 2)
        {
            throw new ArgumentException("Некорректная архитектура сети.");
        }

        if (Array.Exists(layers, size => size <= 0))
        {
            throw new ArgumentException("Размеры слоёв должны быть положительными.");
        }

        // Сохраняем архитектуру
        layerSizes = (int[])layers.Clone();

        // Количество связей между слоями
        neuralLayers = new NeuralLayer[layerSizes.Length - 1];

        for (int i = 0; i < neuralLayers.Length; i++)
        {
            neuralLayers[i] = new NeuralLayer(
                layerSizes[i],
                layerSizes[i + 1]
            );
        }
    }

    public double[] Forward(double[] inputs)
    {
        double[] output = neuralLayers[0].CalculateLayer(inputs);

        for(int i = 1; i < neuralLayers.Length; i++)
        {
            output = neuralLayers[i].CalculateLayer(output);
        }

        return output;
    }
    
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

    public int CountParameters()
    {
        int size = 0;

        foreach (NeuralLayer layer in neuralLayers)
        {
            size += layer.ParameterCount;
        }

        return size;
    }

    public double[] GetGenome()
    {
        double[] genome = new double[CountParameters()];

        int index = 0;

        foreach(NeuralLayer layer in neuralLayers)
        {
            double[] layerGenome = layer.GetGenome();

            for(int i = 0; i < layerGenome.Length; i++)
            {
                genome[index++] = layerGenome[i];
            }
        }

        return genome;
    }

    public void SetGenome(double[] genome)
    {
        if (genome.Length != CountParameters())
        {
            throw new ArgumentException("Неверный размер генома нейронной сети.");
        }

        int index = 0;

        foreach (NeuralLayer layer in neuralLayers)
        {
            double[] layerGenome = new double[layer.GetGenome().Length];

            Array.Copy(genome, index, layerGenome, 0, layerGenome.Length);

            layer.SetGenome(layerGenome);

            index += layerGenome.Length;
        }
    }

}