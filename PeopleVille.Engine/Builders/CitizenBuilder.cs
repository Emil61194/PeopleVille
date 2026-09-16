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
        public List<Citizen> BuildCitizens(World world)
        {
            Random rnd = new Random();
            int citizenAmount = rnd.Next(20, 100);

            string[] firstNames = Core.Data.FirstName.MaleFirstNames.Concat(Core.Data.FirstName.FemaleFirstNames).ToArray();
            string[] lastNames = Core.Data.LastName.LastNames.ToArray();
            string[] addresses = Core.Data.Address.AddressList.ToArray();
            string[] jobTitles = Core.Data.Job.JobTitles.ToArray();
            decimal[] jobSalaries = Core.Data.Job.JobSalaries.ToArray();
            int[] jobWorkStartTimes = Core.Data.Job.JobWorkStartTimes.ToArray();
            int[] jobWorkEndTimes = Core.Data.Job.JobWorkEndTimes.ToArray();
            var workplaces = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IWorkplace).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();


            for (int i = 0; i < citizenAmount; i++)
            {
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                string lastName = lastNames[rnd.Next(lastNames.Length)];
                string address = addresses[rnd.Next(addresses.Length)];

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
                world.Citizens?.Add(citizen);

            }
            return world.Citizens ?? new List<Citizen>();
        }
    }
}
