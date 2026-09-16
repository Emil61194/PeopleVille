using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class Citizen
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime Birth { get; set; }
        public int Gender { get; set; }
        public Job? Job { get; set; }
    }
}
