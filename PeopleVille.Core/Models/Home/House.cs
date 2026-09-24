using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models.Home
{
    public class House : Building, IPrivateHome
    {
        public int HomeId { get; set; }
        public int CitizenCapacity { get; set; }
        public override required string Address { get; set; }
        public int FoodInventory { get; set; }
        public int WaterInventory { get; set; }
        public BankAccount HouseholdFunds { get; set; } = new BankAccount();
    }
}
