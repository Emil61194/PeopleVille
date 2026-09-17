using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models
{
    public class Job(IWorkplace workspace)
    {
        public IWorkplace Workplace { get; set; } = workspace;
    }
}
