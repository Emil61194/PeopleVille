// whole file should be refactored to have handicaps just be a citizen status rather than a whole new concept just because of dll as a sort of "DLC"

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
                FirstName.FirstNames.TryGetValue(gender, out string[]? firstNames);
                firstNames ??= FirstName.FirstNames.Values.SelectMany(names => names).ToArray();
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                if (i > lastFamilyHandicapLoop || i % 5 == 0)
                {
                    lastName = lastNames[rnd.Next(lastNames.Length)];
                }
                (string address, int homeId) = GetAddress(world, lastName, rnd);
                DateTime age = DateTime.Now.AddYears(-rnd.Next(0, 70));
                int yearsOld = DateTime.Now.Year - age.Year;

                Handicap handicap = new(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: DateTime.Now.AddYears(-rnd.Next(0, 70)),
                    gender: gender,
                    homeId: homeId,
                    actions: engine.actionsEachTick)
                {
                    CurrentLocation = address
                };
                AssignFamily(handicap, world);
                world.Citizens?.Add(handicap);
                engine.Tick += handicap.PerformHourlyRoutine;

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

        private static (string Address, int HomeId) GetAddress(World world, string lastName, Random rnd)
        {
            List<House> houses = [.. world.Houses.Select(h => h)];
            List<Apartment> apartments = [.. world.Apartments.Select(a => a)];

            List<Citizen>? relatives = [.. world.Citizens.Where(c => c.LastName == lastName)];

            if (relatives.Count > 0 && relatives.Count < 5)
            {
                Building? relativeHome = FindHome(world, relatives[0].HomeId);
                if (relativeHome is not null)
                {
                    return (relativeHome.Address, relativeHome.HomeId);
                }
            }


            if (rnd.Next(1, 4) == 1 && apartments.Count > 0)
            {
                Apartment apartment = apartments[rnd.Next(apartments.Count)];

                return (apartment.Address, apartment.HomeId);
            }

            House house = houses[rnd.Next(houses.Count)];
            return (house.Address, house.HomeId);
        }

        private static Building? FindHome(World world, int? homeId)
        {
            return world.Houses.Cast<Building>()
                .Concat(world.Apartments)
                .FirstOrDefault(home => home.HomeId == homeId);
        }
    }
}