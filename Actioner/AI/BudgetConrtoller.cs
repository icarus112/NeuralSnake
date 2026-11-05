using Actioner.Models;

namespace Actioner.AI;

public class ActionerConrtoller
{
    public List<Project> SelectProjects(
        Situation situation,
        double[] outputs
    )
    {
        if (outputs.Length < situation.Projects.Count)
        {
            throw new ArgumentException("Недостаточно выходных значений нейросети.");
        }

        List<Project> selected = new List<Project>();

        var rank = situation.Projects
            .Select((project, index) => new
            {
                Project = project,
                Priority = outputs[index]
            })
            .OrderByDescending(x => x.Priority)
            .ToList();

        double budget = situation.Budget;

        foreach(var item in rank)
        {
            double cost = item.Project.Cost; 
            if(budget >= cost)
            {
                selected.Add(item.Project);
                budget -= cost;
            }
        }

        return selected;
    }
} 