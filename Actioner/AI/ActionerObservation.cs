using Actioner.Models;

namespace Actioner.AI;

public class ActionerObservation
{
    public double[] GetState(Situation problem)
    {
        double[] states = new double[problem.maxProjects * 3];
        int index = 0;
        for(int i = 0; i < problem.Projects.Count; i++)
        {
            double budget = problem.Budget;
            double cost = problem.Projects[i].Cost;
            states[index++] = cost / budget;
            states[index++] = problem.Projects[i].Profit / cost;
            states[index++] = problem.Projects[i].SuccessProbability;
        }

        return states;

    }
}