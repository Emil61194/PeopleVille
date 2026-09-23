using PeopleVille.Core.Enum;

namespace PeopleVille.Core.Data
{
    public class FirstName
    {
        public static IReadOnlyDictionary<Genders, string[]> FirstNames { get; } = new Dictionary<Genders, string[]>
        {
            [Genders.Male] = [
                "James",
                "John",
                "Robert",
                "Michael",
                "William",
                "David",
                "Richard",
                "Joseph",
                "Thomas",
                "Charles"
            ],
            [Genders.Female] = [
                "Mary",
                "Patricia",
                "Jennifer",
                "Linda",
                "Elizabeth",
                "Barbara",
                "Susan",
                "Jessica",
                "Sarah",
                "Karen"
            ]
        };
    }
}
