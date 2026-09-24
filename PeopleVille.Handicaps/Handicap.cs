using PeopleVille.Core.Enum;
using PeopleVille.Core.Models;
using System.Collections.Concurrent;

namespace PeopleVille.Handicaps
{
    public class Handicap(World world, int id, string firstName, string lastName, DateTime birth, Genders gender, string homeAddress, ConcurrentBag<object> actions) : Citizen(world, id, firstName, lastName, birth, gender, null, FamilyRoles.Adult, homeAddress, actions)
    {
        public Job? Job { get; } = null;
    }
}
