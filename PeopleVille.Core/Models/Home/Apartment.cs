using PeopleVille.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models.Home
{
    public class Apartment : Building, IPrivateHome
    {           
        public decimal Rent { get; set; }
        public int Floors { get; set; }
        public int CitizenCapacity { get; set; }
        public override string Address { get; set; }
        public override int FoodInventory { get; set; }
        public override int WaterInventory { get; set; }
    }
}
