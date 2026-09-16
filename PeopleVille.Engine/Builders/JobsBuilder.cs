using PeopleVille.Core.Models;
using PeopleVille.Core.Data;
using PeopleVille.Core.Interfaces;

namespace PeopleVille.Engine.Builders;

public static class JobsBuilder
{
    static internal List<Job> BuildJobs(World world)
    {
        List<Job> jobs = new List<Job>();

        foreach (IWorkplace workplace in world.Workplaces)
        {
            for (int capacity = 0; capacity >= workplace.JobCapacity; capacity++)
            {
                jobs.Add(new Job(workplace));
            }
        }
        
        return jobs;
    }
}