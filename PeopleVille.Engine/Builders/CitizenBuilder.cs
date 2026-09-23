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
        public static void BuildCitizens(World world,ref Action tickAction, ConcurrentBag<object> actionsEachTick)
        {
            Random rnd = new();
            int citizenAmount = rnd.Next(10, 15);

            string[] lastNames = [.. LastName.LastNames];

            List<Job> jobs = world.Jobs;

            Array genders = Enum.GetValues<Genders>();
            int genderCount = genders.Length;

            int lastFamilyCitizenLoop = (citizenAmount * 75) / 100;
            string lastName = lastNames[rnd.Next(lastNames.Length)];

            for (int i = 0; i < citizenAmount; i++)
            {
                Genders gender = (Genders)rnd.Next(0, genderCount);
                FirstName.FirstNames.TryGetValue(gender, out string[]? firstNames);
                firstNames ??= [.. FirstName.FirstNames.Values.SelectMany(names => names)];
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                if (i > lastFamilyCitizenLoop || i % 5 == 0)
                {
                    lastName = lastNames[rnd.Next(lastNames.Length)];
                }

                Job chosenJob = jobs[rnd.Next(jobs.Count)];

                (string address, world) = GetAddress(world, lastName, rnd);

                DateTime birth = DateTime.Now.AddYears(-rnd.Next(0, 70));
                int yearsOld = DateTime.Now.Year - birth.Year;
                FamilyRoles familialStatus = yearsOld < 18 ? FamilyRoles.Child : FamilyRoles.Adult;

                Citizen citizen = new(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: birth,
                    gender: gender,
                    family: null,
                    familialStatus: familialStatus,
                    homeAddress: address,
                    actionSink: actionsEachTick)
                {
                    Job = chosenJob,
                    CurrentLocation = address,
                };


                if (yearsOld < 18)
                {
                    citizen.School = world.Schools[rnd.Next(world.Schools.Count)];
                }

                AssignFamily(citizen, world);
                world.Citizens?.Add(citizen);
                tickAction += citizen.PerformHourlyRoutine;
            }
        }

        private static void AssignFamily(Citizen citizen, World world)
        {
            Family? family = world.Citizens
                .Where(existingCitizen => existingCitizen.LastName == citizen.LastName)
                .Select(existingCitizen => existingCitizen.Family)
                .FirstOrDefault(existingFamily => existingFamily is not null);

            if (family is not null)
            {
                int childCount = family.FamilyMembers.Count(member => member.FamilyRoles == FamilyRoles.Child);

                if (citizen.FamilyRoles == FamilyRoles.Child && family.HasRequiredParents && childCount < 3)
                {
                    family.AddMember(citizen);
                    citizen.Family = family;
                }

                return;
            }

            if (citizen.FamilyRoles != FamilyRoles.Adult)
            {
                return;
            }

            Citizen? partner = world.Citizens.LastOrDefault(existingCitizen =>
                existingCitizen.LastName == citizen.LastName
                && existingCitizen.Family is null
                && existingCitizen.FamilyRoles == FamilyRoles.Adult);

            if (partner is null)
            {
                return;
            }

            family = new Family(citizen.Id, []);
            family.AddMember(partner);
            family.AddMember(citizen);
            partner.Family = family;
            citizen.Family = family;
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
