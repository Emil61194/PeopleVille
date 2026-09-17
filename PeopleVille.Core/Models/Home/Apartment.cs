using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models.Home
{
    public class Apartment : Building, IPrivateHome
    {
        public decimal Rent { get; set; }
        public int Floors { get; set; }
        public int CitizenCapacity { get; set; }
        public override required string Address { get; set; }
        public int FoodInventory { get; set; }
        public int WaterInventory { get; set; }
        public BankAccount BankAccount { get; set; } = new BankAccount();

    }
}
