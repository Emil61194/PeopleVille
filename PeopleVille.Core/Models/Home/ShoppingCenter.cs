using PeopleVille.Core.Data;
using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models.Home
{
    public class ShoppingCenter : Building, IWorkplace
    {
        public override required string Address { get; set; }
        public JobTitle JobTitle { get; set; } = Data.JobTitle.Cashier;
        public int JobCapacity { get; set; }

        public int WorkStartTime { get; set; }
        public int WorkEndTime { get; set; }
        public decimal FoodPrice { get; set; }
        public decimal WaterPrice { get; set; }
    }
}
