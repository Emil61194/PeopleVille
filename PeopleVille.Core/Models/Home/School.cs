namespace PeopleVille.Core.Models.Home
{
    public class School : Building
    {
        public int CitizenCapacity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public override required string Address { get; set; }
        public int FoodInventory { get; set; }
        public int WaterInventory { get; set; }
    }
}
