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

        public bool HasRequiredParents => FamilyMembers.Count(
            member => member.FamilyRoles == Enum.FamilyRoles.Adult)
                == MaxAllowedFamilyMembers.parents;

        public void AddMember(Citizen citizen)
        {
            if (!FamilyMembers.Contains(citizen))
            {
                int parentCount = FamilyMembers.Count(member => member.FamilyRoles == Enum.FamilyRoles.Adult);
                int childCount = FamilyMembers.Count(member => member.FamilyRoles == Enum.FamilyRoles.Child);

                if (citizen.FamilyRoles == Enum.FamilyRoles.Adult && parentCount >= MaxAllowedFamilyMembers.parents)
                {
                    throw new InvalidOperationException("A family cannot have more than two parents.");
                }

                if (citizen.FamilyRoles == Enum.FamilyRoles.Child)
                {
                    if (parentCount < MaxAllowedFamilyMembers.parents)
                    {
                        throw new InvalidOperationException("A family must have two parents before adding children.");
                    }

                    if (childCount >= MaxAllowedFamilyMembers.children)
                    {
                        throw new InvalidOperationException("A family cannot have more than three children.");
                    }
                }

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

        private void DeleteFamily(int id, List<Family> family)
        {
            family.RemoveAll(f => f.Id == id);
        }

        private void DeleteFamilyMembers(Citizen citizen)
        {
            familyMembers.Remove(citizen);
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
