using PeopleVille.Core.Data;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using PeopleVille.Engine;

namespace PeopleVille.Handicaps
{
    public class HandicapBuilder(GameEngine engine) : IBuilder
    {
        public void Build(World world)
        {
            Random rnd = new();
            int handicapAmount = rnd.Next(10, 30);
            string[] lastNames = LastName.LastNames.ToArray();
            Array genders = Enum.GetValues<Genders>();
            int genderCount = genders.Length;
            int lastFamilyHandicapLoop = (handicapAmount * 75) / 100;
            string lastName = lastNames[rnd.Next(lastNames.Length)];
            for (int i = 0; i < handicapAmount; i++)
            {
                Genders gender = (Genders)rnd.Next(0, genderCount);
                string[] firstNames;
                if (!FirstName.FirstNames.TryGetValue(gender, out firstNames))
                {
                    firstNames = FirstName.FirstNames.Values.SelectMany(names => names).ToArray();
                    //firstNames = ["Benjamin"];
                }
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                if (i > lastFamilyHandicapLoop || i % 5 == 0)
                {
                    lastName = lastNames[rnd.Next(lastNames.Length)];
                }
                (string address, world) = GetAddress(world, lastName, rnd);
                DateTime age = DateTime.Now.AddYears(-rnd.Next(0, 70));
                int yearsOld = DateTime.Now.Year - age.Year;

                Handicap handicap = new(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: DateTime.Now.AddYears(-rnd.Next(0, 70)),
                    gender: gender,
                    homeAddress: address,
                    actions: engine.actionsEachTick)
                {
                    CurrentLocation = address
                };
                world.Citizens?.Add(handicap);

            }
        }
        private (string, World) GetAddress(World world, string lastName, Random rnd)
        {
            List<House> houses = world.Houses.Select(h => h).ToList();
            List<Apartment> apartments = world.Apartments.Select(a => a).ToList();

            List<Citizen>? relatives = world.Citizens.Where(c => c.LastName == lastName).ToList();

            if (relatives.Count > 0 && relatives.Count < 5)
            {
                string address = relatives[0].HomeAddress;
                return (address, world);
            }


            if (rnd.Next(1, 4) == 1 && apartments.Count > 0)
            {
                Apartment apartment = apartments[rnd.Next(apartments.Count)];

                int currentApartmentsInAddress = world.Apartments.Count(a => a.Address.Contains(apartment.Address));

                int addressFloor = currentApartmentsInAddress % apartment.Floors + 1; // unsure

                string apartmentAddress = $"{apartment.Address}, {addressFloor}. {currentApartmentsInAddress + 1}";
                return (apartmentAddress, world);
            }

            House house = houses[rnd.Next(houses.Count)];
            return (house.Address, world);
        }
    }
}