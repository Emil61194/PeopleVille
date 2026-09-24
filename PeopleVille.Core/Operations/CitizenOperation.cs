using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Operations
{
    public class CitizenOperation
    {
        public int CitizenId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Gender { get; set; }
        public required string HomeAddress { get; set; }
        public required string CurrentLocation { get; set; }
        public bool IsAdult { get; set; }
        public bool IsEmployed { get; set; }
        public required string Message { get; set; }
        public required DateTime WorldTime { get; set; }
    }
}