using PeopleVille.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models.Home
{
    public class ShoppingCenter : Building, IWorkplace
    {
        public override required string Address { get; set; }
    }
}
