namespace PeopleVille.Core.Data
{
    public class LastName
    {
        public static IReadOnlyList<string> LastNames { get; } = [
            "Andersen", "Jensen", "Nielsen", "Hansen", "Pedersen",
            "Christensen", "Larsen", "Sørensen", "Rasmussen", "Madsen"
        ];
    }
}