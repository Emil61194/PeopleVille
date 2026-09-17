using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, int gender, string homeAddress)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public int Gender { get; } = gender;
        public string HomeAddress { get; set; } = homeAddress;
        public Job? Job { get; set; }

        public required string CurrentLocation { get; set; }
        BankAccount BankAccount { get; set; } = new BankAccount();
        public School? School { get; set; }

        public void DoSomething()
        {
            int yearsOld = DateTime.Now.Year - Birth.Year;

            if (Job.Workplace.WorkStartTime > world.Time && Job.Workplace.WorkEndTime < world.Time)
            {
                CurrentLocation = HomeAddress;
            }
            else if (yearsOld > 18 && Job != null && Job.Workplace.WorkStartTime < world.Time && Job.Workplace.WorkEndTime > world.Time)
            {
                CurrentLocation = Job.Workplace.Address;
            }
            else if (yearsOld > 18 && Job == null)
            {
                CurrentLocation = HomeAddress; // Maybe later on activites for jobless people
            }
            else if (yearsOld < 18 && School.StartTime < world.Time && School.EndTime > world.Time)
            {
                CurrentLocation = School.Address;
            }

        }
    }
}
