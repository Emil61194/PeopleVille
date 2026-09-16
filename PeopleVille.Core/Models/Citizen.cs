using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, int gender)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public int Gender { get; } = gender;
        public Job? Job { get; set; }
        public string HomeAddress { get; set; }

        public required string CurrentLocation { get; set; }

        public void DoSomething()
        {
            TimeSpan age = DateTime.Now - Birth;
            int yearsOld = (int)(age.TotalDays / 365.25);
    
            if (world.Time > 21 || world.Time < 6)
            {
                CurrentLocation = HomeAddress;
            }
            else if (yearsOld > 18 && Job != null && Job.WorkStartTime < world.Time && Job.WorkEndTime > world.Time)
            {
                CurrentLocation = Job.Workplace.Address;
            }
            else if (yearsOld > 18 && Job == null)
            {
                CurrentLocation = HomeAddress; // Maybe later on activites for jobless people
            }
            else if (yearsOld < 18)
            {
                CurrentLocation = HomeAddress; // Add school later on
            }

        }
    }
}
