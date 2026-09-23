using PeopleVille.Core.Data;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System.Collections.Concurrent;
using PeopleVille.Core.Data;

namespace PeopleVille.Engine.Builders
{
    public delegate void RoutineAction();

    public class CitizenBuilder
    {
        public void BuildCitizens(World world,ref Action tickAction, ConcurrentBag<object> actionsEachTick)
        {
            Random rnd = new();
            int citizenAmount = rnd.Next(40, 100);

            string[] lastNames = LastName.LastNames.ToArray();

            List<Job> jobs = world.Jobs;

            Array genders = Enum.GetValues<Genders>();
            int genderCount = genders.Length;

            int lastFamilyCitizenLoop = (citizenAmount * 75) / 100;
            string lastName = lastNames[rnd.Next(lastNames.Length)];

            for (int i = 0; i < citizenAmount; i++)
            {
                Genders gender = (Genders)rnd.Next(0, genderCount);
                string[] firstNames;
                if (!FirstName.FirstNames.TryGetValue(gender, out firstNames))
                {
                    firstNames = FirstName.FirstNames.Values.SelectMany(names => names).ToArray();
                }
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                if (i > lastFamilyCitizenLoop || i % 5 == 0)
                {
                    lastName = lastNames[rnd.Next(lastNames.Length)];
                }

                Job chosenJob = null;
                if (jobs.Count > 0)
                {
                    chosenJob = jobs[rnd.Next(jobs.Count)];
                    jobs.Remove(chosenJob);
                }

                (string address, world) = GetAddress(world, lastName, rnd);

                DateTime age = DateTime.Now.AddYears(-rnd.Next(0, 70));
                int yearsOld = DateTime.Now.Year - age.Year;


                Citizen citizen = new(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: DateTime.Now.AddYears(-rnd.Next(0, 70)),
                    gender: gender,
                    homeAddress: address,
                    actions: actionsEachTick)
                {
                    Job = chosenJob,
                    CurrentLocation = address
                };

                if (yearsOld < 18)
                {
                    citizen.School = world.Schools[rnd.Next(world.Schools.Count)];
                }

                world.Citizens?.Add(citizen);
                RoutineAction routineAction = citizen.PerformHourlyRoutine;
                tickAction += routineAction.Invoke;
            }
        }

        private static (string, World) GetAddress(World world, string lastName, Random rnd)
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
