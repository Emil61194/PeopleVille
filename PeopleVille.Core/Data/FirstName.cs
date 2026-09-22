using PeopleVille.Core.Enum;

namespace PeopleVille.Core.Data
{
    public class FirstName
    {
        public static Dictionary<Genders, string[]> FirstNames = new()
        {
            [Genders.Male] = new[]
            {
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
            },

            [Genders.Female] = new[]
            {
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
            }
        };
    }
}
