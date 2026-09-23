using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models
{
    public class BankAccount
    {
        public required IPrivateHome Owner { get; set; }
        public decimal Balance { get; set; }
        public List<int>? Transactions { get; set; }
    }
}
