using PeopleVille.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class BankAccount
    {
        public IPrivateHome Owner { get; set; }
        public decimal Balance { get; set; }
        public List<int> Transactions { get; set; }
    }
}
