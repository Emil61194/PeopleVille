using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Operations
{
    public class ApartmentOperation
    {
        public required string Address { get; set; }
        public int CitizenCapacity { get; set; }
        public int Floors { get; set; }
        public decimal Rent { get; set; }
        public int FoodInventory { get; set; }
        public int WaterInventory { get; set; }
        public int BankAccountBalance { get; set; }
        public required string Message { get; set; }
    }
}
