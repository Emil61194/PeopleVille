using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class Job
    {
        public required string Title { get; set; }
        public decimal Salary { get; set; }
        public DateTime WorkStartTime { get; set; }
        public DateTime WorkEndTime { get; set; }
    }
}
