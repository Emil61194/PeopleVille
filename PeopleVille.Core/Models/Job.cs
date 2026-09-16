using PeopleVille.Core.Interfaces;
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
        public int WorkStartTime { get; set; }
        public int WorkEndTime { get; set; }
        public required IWorkplace Workplace { get; set; }
    }
}
