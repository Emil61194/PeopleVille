using PeopleVille.Core.Data;

namespace PeopleVille.Core.Interfaces
{
    public interface IWorkplace
    {
        public string Address { get; set; }
        public JobTitle JobTitle { get; set; }
        public int JobCapacity { get; set; }

        public int WorkStartTime { get; set; }
        public int WorkEndTime { get; set; }
    }
}
