using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Engine.Builders
{
    public class ApartmentBuilder
    {
        public void BuildApartments(World world)
        {
            Random rnd = new Random();
            List<string> addresses = Core.Data.Address.AddressList.ToList();
            string[] usedAddresses = world.Workplaces.SelectMany(w => w.Address).Select(a => a.ToString()).ToArray();
            usedAddresses = usedAddresses.Concat(world.Houses.SelectMany(h => h.Address).Select(a => a.ToString())).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            int apartmentCount = rnd.Next(1, 3);

            for (int i = 0; i < apartmentCount; i++)
            {
                string address = addresses[rnd.Next(addresses.Count)];
                int floors = rnd.Next(2, 5);

                Apartment apartment = new Apartment
                {
                    Address = address,
                    Floors = floors,
                    Rent = rnd.Next(500, 2000),
                    FoodInventory = rnd.Next(50, 300),
                    WaterInventory = rnd.Next(50, 300),
                    CitizenCapacity = floors * rnd.Next(2, 5),
                };
                addresses.Remove(address);
                world.Apartments.Add(apartment);
            }

        }
    }
}
