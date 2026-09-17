using PeopleVille.Core.Models;
using PeopleVille.Core.Data;
using PeopleVille.Core.Interfaces;

namespace PeopleVille.Engine.Builders;

public static class JobsBuilder
{
    static internal void BuildJobs(World world)
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