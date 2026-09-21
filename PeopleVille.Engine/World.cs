using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Engine
{
    public class World
    {
        public DateTime currentDateTime = DateTime.UtcNow;
        public List<Citizen>? Citizens { get; set; }
        public List<BankAccount>? BankAccount { get; set; }
        public List<Job>? Jobs { get; set; }
        public List<ShoppingCenter>? ShoppingCenters { get; set; }
        public List<House>? Houses { get; set; }
        public List<Apartment>? Apartments { get; set; }
        public List<School>? Schools { get; set; }
    }
}