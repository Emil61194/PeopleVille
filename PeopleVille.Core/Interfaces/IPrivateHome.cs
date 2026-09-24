using PeopleVille.Core.Models;

namespace PeopleVille.Core.Interfaces
{
    public interface IPrivateHome
    {
        BankAccount HouseholdFunds { get; set; }
        public int CitizenCapacity { get; set; }
    }
}
