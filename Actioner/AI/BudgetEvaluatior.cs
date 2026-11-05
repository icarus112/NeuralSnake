using Actioner.Models;


namespace Actioner.AI;

public class BudgetEvaluator
{
    public double Calculate(List<Project> selected)
    {
        double fitness = 0;

        foreach(Project project in selected)
        {
            double prob = project.SuccessProbability;
            double profit = project.Profit;
            double cost = project.Cost;

            fitness += prob*profit - (1 - prob) * cost;
        }

        return fitness;
    }
}