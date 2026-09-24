using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Engine.Builders
{
    public class HouseBuilder : IBuilder
    {
        public void Build(World world)
        {
            Random rnd = new Random();
            List<string> addresses = Core.Data.Address.AddressList.ToList();
            string[] usedAddresses = world.Workplaces.Select(w => w.Address).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            int houseCount = rnd.Next(20, 50);

            for (int i = 0; i < houseCount; i++)
            {
                if (addresses.Count == 0)
                {
                    break;
                }

                House house = new()
                {
                    Address = addresses[rnd.Next(addresses.Count - 1)],
                    FoodInventory = rnd.Next(50, 300),
                    WaterInventory = rnd.Next(50, 300),
                    CitizenCapacity = rnd.Next(1, 5),
                };
                addresses.Remove(house.Address);
                world.Houses.Add(house);
            }
        }
    }
}
