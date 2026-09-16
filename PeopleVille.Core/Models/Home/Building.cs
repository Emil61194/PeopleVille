using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models.Home
{
    public abstract class Building
    {
        public abstract required string Address { get; set; }
    }
}
