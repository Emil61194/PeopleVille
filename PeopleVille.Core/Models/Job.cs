using PeopleVille.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using PeopleVille.Core.Data;

namespace PeopleVille.Core.Models
{
    public class Job (IWorkplace workspace)
    {
        public IWorkplace Workplace { get; set; } = workspace;
    }
}
