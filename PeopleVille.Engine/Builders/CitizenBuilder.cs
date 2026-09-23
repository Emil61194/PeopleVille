using PeopleVille.Core.Data;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System.Collections.Concurrent;

namespace PeopleVille.Engine.Builders
{
    public delegate void RoutineAction();

    public class CitizenBuilder
    {
        public void BuildCitizens(World world, Action tickAction, ConcurrentBag<object> actionsEachTick)
        {
            Random rnd = new();
            int citizenAmount = rnd.Next(40, 100);

            string[] lastNames = [.. LastName.LastNames];

            List<Job> jobs = world.Jobs;

            Array genders = Enum.GetValues<Genders>();
            int genderCount = genders.Length;

            for (int i = 0; i < citizenAmount; i++)
            {
                Genders gender = (Genders)rnd.Next(0, genderCount);
                FirstName.FirstNames.TryGetValue(gender, out string[]? firstNames);
                firstNames ??= [.. FirstName.FirstNames.Values.SelectMany(names => names)];
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                string lastName = lastNames[rnd.Next(lastNames.Length)];

                Job chosenJob = jobs[rnd.Next(jobs.Count)];

                (string address, world) = GetAddress(world, lastName, rnd);

                Family family = world.Citizens
                    .FirstOrDefault(citizen => citizen.LastName == lastName)?.Family
                    ?? new Family(i + 1, []);

                DateTime birth = DateTime.Now.AddYears(-rnd.Next(0, 70));
                int yearsOld = DateTime.Now.Year - birth.Year;
                FamilyRoles familialStatus = yearsOld < 18 ? FamilyRoles.Child : FamilyRoles.Adult;

                Citizen citizen = new(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: birth,
                    gender: (int)gender,
                    family: family,
                    familialStatus: familialStatus,
                    homeAddress: address)
                {
                    Job = chosenJob,
                    CurrentLocation = address,
                };


                if (yearsOld < 18)
                {
                    citizen.School = world.Schools[rnd.Next(world.Schools.Count)];
                }

                family.AddMember(citizen);
                world.Citizens?.Add(citizen);
                tickAction += citizen.PerformHourlyRoutine;
            }
        }

        private static (string, World) GetAddress(World world, string lastName, Random rnd)
        {
            List<House> houses = [.. world.Houses.Select(h => h)];
            List<Apartment> apartments = [.. world.Apartments.Select(a => a)];

            List<Citizen>? relatives = [.. world.Citizens.Where(c => c.LastName == lastName)];

            if (relatives.Count > 0 && relatives.Count < 5)
            {
                string address = relatives[0].HomeAddress;
                return (address, world);
            }


            if (rnd.Next(1, 4) == 1 && apartments.Count > 0)
            {
                Apartment apartment = apartments[rnd.Next(apartments.Count)];

                int currentApartmentsInAddress = world.Apartments.Count(a => a.Address.Contains(apartment.Address));

                int addressFloor = currentApartmentsInAddress % apartment.Floors; // track apartment occupancy, might otherwise just maintain explicit floor/unit data

                string apartmentAddress = $"{apartment.Address}, {addressFloor}. {currentApartmentsInAddress + 1}";
                return (apartmentAddress, world);
            }

            House house = houses[rnd.Next(houses.Count)];
            return (house.Address, world);
        }
    }
}
