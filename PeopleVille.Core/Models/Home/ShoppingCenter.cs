using PeopleVille.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using PeopleVille.Core.Data;

namespace PeopleVille.Core.Models.Home
{
    public class ShoppingCenter : Building, IWorkplace
    {
        public override required string Address { get; set; }
        public override int FoodInventory { get; set; }
        public override int WaterInventory { get; set; }
        public JobTitle JobTitle { get; set; } = Data.JobTitle.Cashier;
        public int JobCapacity { get; set; }
        
        public int WorkStartTime { get; set; }
        public int WorkEndTime { get; set; }
    }
}
