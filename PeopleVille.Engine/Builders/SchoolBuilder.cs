using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Engine.Builders
{
    public class SchoolBuilder
    {
        public List<School> BuildSchools()
        {
            List<School> schools = new List<School>();
            List<string> addresses = Core.Data.Address.AddressList;

            Random rnd = new Random();
            int schoolCount = rnd.Next(1, 3);
            for (int i = 0; i < schoolCount; i++)
            {
               School school = new School
               {
                   Address = addresses[rnd.Next(addresses.Count)],
                   StartTime = DateTime.Today.AddHours(rnd.Next(7, 9)),
                   EndTime = DateTime.Today.AddHours(rnd.Next(14, 17))
               };
                addresses.Remove(school.Address); 
                schools.Add(school);
            }
            return schools;
        }
    }
}
