namespace Actioner.Models;


public class Situation
{
    public List<Project> Projects { get; private set; }
    public double Budget { get; private set; }
    public int maxProjects {get; private set; }

    public Situation(List<Project> projects, double budget, int maxProjects = 20)
    {
        if (budget <= 0)
            throw new ArgumentException("Бюджет должен быть положительным.");

        if (projects == null || projects.Count == 0 || projects.Count > maxProjects)
            throw new ArgumentException($"Список проектов не может быть пустым и не может содержать более {maxProjects} проектов.");
        Projects = projects;
        Budget = budget;
    }


}