public class Project
{
    public double Cost{get; private set;}
    public double Profit{get; private set;}
    public double SuccessProbability{get; private set;}

    public Project(double cost, double profit, double successProbability)
    {

        if (cost <= 0)
            throw new ArgumentException("Стоимость проекта не может быть отрицательной.");
        
        if (profit < 0)
            throw new ArgumentException("Прибыль проекта не может быть отрицательной.");
        
        if (successProbability < 0 || successProbability > 1)
            throw new ArgumentException("Вероятность успеха проекта должна быть в диапазоне от 0 до 1.");

        Cost = cost;
        Profit = profit;
        SuccessProbability = successProbability;
    }

}