using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Core.Models
{
    public class World
    {
        public DateTime currentDateTime = DateTime.UtcNow;
        public List<Citizen> Citizens { get; set; } = new List<Citizen>();
        public List<BankAccount> BankAccount { get; set; } = new List<BankAccount>();
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<ShoppingCenter> ShoppingCenters { get; set; } = new List<ShoppingCenter>();
        public List<IWorkplace> Workplaces { get; set; } = new List<IWorkplace>();
        public List<House> Houses { get; set; } = new List<House>();
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();
        public List<School> Schools { get; set; } = new List<School>();
    }
}