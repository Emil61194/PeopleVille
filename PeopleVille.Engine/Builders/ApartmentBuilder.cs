using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Engine.Builders
{
    public class ApartmentBuilder : IBuilder
    {
        public void Build(World world)
        {
            Random rnd = new Random();
            List<string> addresses = Core.Data.Address.AddressList.ToList();
            string[] usedAddresses = world.Workplaces.Select(w => w.Address).ToArray();
            usedAddresses = usedAddresses.Concat(world.Houses.Select(h => h.Address)).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            int apartmentCount = rnd.Next(1, 3);

            for (int i = 0; i < apartmentCount; i++)
            {
                if (addresses.Count == 0)
                {
                    break;
                }
                int floors = rnd.Next(2, 5);

                Apartment apartment = new Apartment
                {
                    Address = addresses[rnd.Next(addresses.Count)],
                    Floors = rnd.Next(2, 5),
                    Rent = rnd.Next(500, 2000),
                    FoodInventory = rnd.Next(50, 300),
                    WaterInventory = rnd.Next(50, 300),
                    CitizenCapacity = floors * rnd.Next(2, 5),
                };
                addresses.Remove(apartment.Address);
                world.Apartments.Add(apartment);
            }

        }
    }
}
