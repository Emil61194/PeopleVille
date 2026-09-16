using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class HouseBuilder
    {
        public List<House> BuildHouses(World world)
        {
            Random rnd = new Random();
            List<House> houses = new List<House>();
            List<string> addresses = Core.Data.Address.AddressList;

            string[] usedAddresses = world.Schools.SelectMany(s => s.Address).Select(a => a.ToString()).ToArray();
            usedAddresses = usedAddresses.Concat(world.ShoppingCenters.SelectMany(s => s.Address).Select(a => a.ToString())).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            string[] distinctLastNames = world.Citizens.Select(c => c.LastName).Distinct().ToArray();
            int houseCount = distinctLastNames.Length;
            foreach (string lastName in distinctLastNames)
            {
                if (world.Citizens.Count(c => c.LastName == lastName) > 4)
                {
                    houseCount++;
                }
            }

            for (int i = 0; i < houseCount; i++)
            {
                int randomAddressNumber = rnd.Next(addresses.Count);
                int numberOfResidents = world.Citizens.Count(c => c.HomeAddress == addresses[randomAddressNumber]);

                House house = new House
                {
                    Address = addresses[randomAddressNumber],
                    CitizenCapacity = numberOfResidents,
                    FoodInventory = rnd.Next(50, 300),
                    WaterInventory = rnd.Next(50, 300),
                };
                addresses.Remove(house.Address);
                houses.Add(house);
            }
            return houses;
        }
    }
}
