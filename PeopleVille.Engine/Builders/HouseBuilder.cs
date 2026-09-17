using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class HouseBuilder
    {
        public void BuildHouses(World world)
        {
            Random rnd = new Random();
            List<string> addresses = Core.Data.Address.AddressList;
            string[] usedAddresses = world.Workplaces.SelectMany(w => w.Address).Select(a => a.ToString()).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            int houseCount = rnd.Next(20, 50); 

            for (int i = 0; i < houseCount; i++)
            {
                int randomAddressNumber = rnd.Next(addresses.Count);

                House house = new House
                {
                    Address = addresses[randomAddressNumber],
                    FoodInventory = rnd.Next(50, 300),
                    WaterInventory = rnd.Next(50, 300),
                };
                addresses.Remove(house.Address);
                world.Houses.Add(house);
            }
        }
    }
}
