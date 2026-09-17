using PeopleVille.Core.Data;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class CitizenBuilder
    {
        public void BuildCitizens(World world, Action? tickAction)
        {
            Random rnd = new Random();
            int citizenAmount = rnd.Next(40, 100);

            string[] firstNames = Core.Data.FirstName.MaleFirstNames.Concat(Core.Data.FirstName.FemaleFirstNames).ToArray();
            string[] lastNames = Core.Data.LastName.LastNames.ToArray();

            List<Job> jobs = world.Jobs;

            Array genders = Enum.GetValues(typeof(Genders));
            int genderCount = genders.Length;



            for (int i = 0; i < citizenAmount; i++)
            {
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                string lastName = lastNames[rnd.Next(lastNames.Length)];

                Job chosenJob = jobs[rnd.Next(jobs.Count)];

                (string address, world) = GetAddress(world, lastName, rnd);

                Citizen citizen = new Citizen(world: world,
                    id: i + 1,
                    firstName: firstName,
                    lastName: lastName,
                    birth: DateTime.Now.AddYears(-rnd.Next(0, 70)),
                    gender: rnd.Next(0, genderCount + 1),
                    homeAddress: address)
                {
                    Job = chosenJob,
                    CurrentLocation = address
                };
                jobs.Remove(chosenJob);
                world.Citizens?.Add(citizen);
                tickAction += citizen.DoSomething;

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
                world.Apartments.Remove(apartment);
                return (apartmentAddress, world);
            }

            House house = houses[rnd.Next(houses.Count)];
            world.Houses.Remove(house);
            return (house.Address, world);
        }
    }
}
