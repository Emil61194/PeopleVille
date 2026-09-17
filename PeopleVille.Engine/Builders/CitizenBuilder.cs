using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class CitizenBuilder
    {
        public List<Citizen> BuildCitizens(World world, Action? tickAction)
        {
            Random rnd = new Random();
            int citizenAmount = rnd.Next(20, 100);

            string[] firstNames = Core.Data.FirstName.MaleFirstNames.Concat(Core.Data.FirstName.FemaleFirstNames).ToArray();
            string[] lastNames = Core.Data.LastName.LastNames.ToArray();
            List<string> addresses = Core.Data.Address.AddressList;
            string[] jobTitles = Core.Data.JobOptions.JobTitles.ToArray();
            decimal[] jobSalaries = Core.Data.JobOptions.JobSalaries.ToArray();
            int[] jobWorkStartTimes = Core.Data.JobOptions.JobWorkStartTimes.ToArray();
            int[] jobWorkEndTimes = Core.Data.JobOptions.JobWorkEndTimes.ToArray();
            var workplaces = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IWorkplace).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();


            for (int i = 0; i < citizenAmount; i++)
            {
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                string lastName = lastNames[rnd.Next(lastNames.Length)];
                string address = addresses[rnd.Next(addresses.Count)];

                Citizen citizen = new Citizen(world, id: i + 1, firstName, lastName, DateTime.Now.AddYears(-rnd.Next(0, 70)), rnd.Next(0, 10), address)
                {
                    CurrentLocation = address,
                    Job = new Job
                    {
                        Title = jobTitles[rnd.Next(jobTitles.Length)],
                        Salary = jobSalaries[rnd.Next(jobSalaries.Length)],
                        WorkStartTime = jobWorkStartTimes[rnd.Next(jobWorkStartTimes.Length)],
                        WorkEndTime = jobWorkEndTimes[rnd.Next(jobWorkEndTimes.Length)],
                        Workplace = workplaces[rnd.Next(workplaces.Count)].GetConstructor(Type.EmptyTypes)?.Invoke(null) as IWorkplace // unsure
                    }

                };
                addresses.Remove(address);
                world.Citizens?.Add(citizen);
                tickAction += citizen.DoSomething;
            }
            
            return world.Citizens ?? new List<Citizen>();
        }
    }
}
