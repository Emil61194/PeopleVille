using PeopleVille.Core.Interfaces;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Engine.Builders
{
    public class SchoolBuilder : IBuilder
    {
        public void Build(World world)
        {
            List<string> addresses = Core.Data.Address.AddressList.ToList();
            string[] usedAddresses = world.Workplaces.Select(w => w.Address).ToArray();
            addresses = addresses.Except(usedAddresses).ToList();

            Random rnd = new Random();
            int schoolCount = rnd.Next(1, 3);
            for (int i = 0; i < schoolCount; i++)
            {
                if (addresses.Count == 0)
                {
                    break;
                }
                School school = new School
                {
                    Address = addresses[rnd.Next(addresses.Count)],
                    StartTime = DateTime.Today.AddHours(rnd.Next(7, 9)),
                    EndTime = DateTime.Today.AddHours(rnd.Next(14, 17))
                };
                addresses.Remove(school.Address);
                world.Schools.Add(school);
            }
        }
    }
}
