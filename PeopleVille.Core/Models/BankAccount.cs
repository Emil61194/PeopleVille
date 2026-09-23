using PeopleVille.Core.Interfaces;
using System.Text.Json.Serialization;

namespace PeopleVille.Core.Models
{
    public class BankAccount
    {
        [JsonIgnore]
        public IPrivateHome? Owner { get; set; }
        public decimal Balance { get; set; }
        [JsonIgnore]
        public List<int>? Transactions { get; set; }
    }
}
