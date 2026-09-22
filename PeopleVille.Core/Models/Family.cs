namespace PeopleVille.Core.Models
{
    public class Family(int id, List<Family> family)
    {
        public int Id { get; set; } = id;
        public int FamilyMembersCount { get; set; }
        public required BankAccount Balance { get; set; }
        public List<Family> FamilyMembers { get; set; } = family;

        private readonly (int parents, int children) MaxAllowedFamilyMembers = (parents: 2, children: 3);
        private readonly List<Family> familyMembers = [];

        private static Family CreateFamily(List<Citizen> citizens)
        {
            var familyMembers = new FamilyMembers();
            familyMembers.SortCitizensIntoFamilyRoles(citizens);

            return new Family(id: 0, family: [])
            {
                Balance = null!
            };
        }

        private void DeleteFamily(Family family)
        {
            familyMembers.Remove(family);
        }
    }

    public class FamilyMembers()
    {
        private readonly List<Citizen> parents = [];
        private readonly List<Citizen> children = [];

        public void SortCitizensIntoFamilyRoles(List<Citizen> citizens)
        {
            foreach (var member in citizens)
            {
                var familyGroup = member.FamilyRoles != Enum.FamilyRoles.Adult
                    ? children
                    : parents;

                familyGroup.Add(member);
            }
        }
    }
}
