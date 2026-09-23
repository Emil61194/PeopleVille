using PeopleVille.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Interfaces
{
    public interface IBuilder
    {
        public void Build(World world);
    }
}
