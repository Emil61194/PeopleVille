using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models.Home
{
    public class ShoppingCenter : Building
    {
        public override required string Address { get; set; }
        public override int FoodInventory { get; set; }
        public override int WaterInventory { get; set; }
    }
}
