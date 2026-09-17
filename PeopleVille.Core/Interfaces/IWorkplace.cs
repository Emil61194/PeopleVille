using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;
using PeopleVille.Core.Data;
using PeopleVille.Core.Data;

namespace PeopleVille.Core.Interfaces
{
    public interface IWorkplace
    {
        public string Address { get; set; }
        public JobTitle JobTitle { get; set; }
        public int JobCapacity { get; set; }

        public DateTime WorkStartTime { get; set; }
        public DateTime WorkEndTime { get; set; }
    }
}
