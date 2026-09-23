using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;

namespace PeopleVille.Engine.Builders;

public class JobsBuilder : IBuilder
{
    public void Build(World world)
    {

        foreach (IWorkplace workplace in world.Workplaces)
        {
            for (int capacity = 0; capacity <= workplace.JobCapacity; capacity++)
            {
                world.Jobs.Add(new Job(workplace));
            }
        }

    }
}