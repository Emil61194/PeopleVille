using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
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
        public Building? CurrentLocation { get; set; }

        public void DoSomething()
        {

        }
    }
}
