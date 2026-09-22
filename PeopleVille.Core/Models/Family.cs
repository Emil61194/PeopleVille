namespace PeopleVille.Core.Models
{
    public class Family(int id, List<Citizen> family)
    {
        public int Id { get; set; } = id;
        public int FamilyMembersCount { get; set; }
        public BankAccount Balance { get; set; } = new();
        public List<Citizen> FamilyMembers { get; set; } = family;

        private readonly (int parents, int children) MaxAllowedFamilyMembers = (parents: 2, children: 3);
        private readonly List<Citizen> familyMembers = [];

        public void AddMember(Citizen citizen)
        {
            if (!FamilyMembers.Contains(citizen))
            {
                FamilyMembers.Add(citizen);
                FamilyMembersCount = FamilyMembers.Count;
            }

            RefreshBalance();
        }

        public void RefreshBalance()
        {
            Balance.Balance = FamilyMembers
                .Where(member => member.FamilyRoles == Enum.FamilyRoles.Adult)
                .Sum(parent => parent.BankAccount.Balance / 2);
        }

        private static Family CreateFamily(List<Citizen> citizens)
        {
            var familyMembers = new FamilyMembers();
            familyMembers.SortCitizensIntoFamilyRoles(citizens);

            return new Family(id: 0, family: []);
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
