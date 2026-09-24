using PeopleVille.Core.Enum;
using PeopleVille.Core.Models;
using System.Collections.Concurrent;

namespace PeopleVille.Handicaps
{
    public class Handicap(World world, int id, string firstName, string lastName, DateTime birth, Genders gender, int? homeId, ConcurrentBag<object> actions) : Citizen(world, id, firstName, lastName, birth, gender, null, FamilyRoles.Adult, homeId, actions)
    {
    }
}
