using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Models;
using System;

namespace PeopleVille.Test
{
    [TestClass]
    public class FamilyTests
    {
        [TestMethod]
        public void AddMember_RequiresTwoParentsBeforeAddingChild()
        {
            Family family = new(1, []);
            Citizen child = CreateCitizen(FamilyRoles.Child, 1);

            Assert.ThrowsException<InvalidOperationException>(() => family.AddMember(child));
        }

        [TestMethod]
        public void AddMember_AllowsFamilyWithTwoParentsAndNoChildren()
        {
            Family family = new(1, []);

            family.AddMember(CreateCitizen(FamilyRoles.Adult, 1));
            family.AddMember(CreateCitizen(FamilyRoles.Adult, 2));

            Assert.IsTrue(family.HasRequiredParents);
            Assert.AreEqual(2, family.FamilyMembersCount);
        }

        [TestMethod]
        public void AddMember_RejectsFourthChild()
        {
            Family family = new(1, []);
            family.AddMember(CreateCitizen(FamilyRoles.Adult, 1));
            family.AddMember(CreateCitizen(FamilyRoles.Adult, 2));

            for (int id = 3; id <= 5; id++)
            {
                family.AddMember(CreateCitizen(FamilyRoles.Child, id));
            }

            Assert.ThrowsException<InvalidOperationException>(() => family.AddMember(CreateCitizen(FamilyRoles.Child, 6)));
            Assert.AreEqual(5, family.FamilyMembersCount);
        }

        private static Citizen CreateCitizen(FamilyRoles familyRole, int id)
        {
            World world = new();
            return new Citizen(world, id, $"Citizen{id}", "Family", DateTime.Today, 0, new Family(1, []), familyRole, null)
            {
                CurrentLocation = "Home"
            };
        }
    }
}