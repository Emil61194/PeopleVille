using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Operations
{
    public class HouseOperation
    {
        public required string Address { get; set; }
        public int CitizenCapacity { get; set; }
        public int FoodInventory { get; set; }
        public int WaterInventory { get; set; }
        public int BankAccountBalance { get; set; }
        public required string Message { get; set; }
        public required DateTime WorldTime { get; set; }
    }
}
