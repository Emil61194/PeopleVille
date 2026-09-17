using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleVille.Core.Models;
using PeopleVille.Engine;
using System.Linq;


namespace PeopleVille.Test
{
    [TestClass]
    public class InitializeCityTests
    {
        [TestMethod]
        public void InitializeCity_ReturnsWorldWithAllCollectionsInitialized()
        {
            World world = new GameEngine().InitializeCity();

            Assert.IsNotNull(world.Citizens);
            Assert.IsNotNull(world.BankAccount);
            Assert.IsNotNull(world.Jobs);
            Assert.IsNotNull(world.ShoppingCenters);
            Assert.IsNotNull(world.Workplaces);
            Assert.IsNotNull(world.Houses);
            Assert.IsNotNull(world.Apartments);
            Assert.IsNotNull(world.Schools);
        }

        [TestMethod]
        public void InitializeCity_CreatesExpectedCityContents()
        {
            World world = new GameEngine().InitializeCity();

            Assert.IsTrue(world.ShoppingCenters.Count is >= 1 and <= 2);
            Assert.IsTrue(world.Schools.Count is >= 1 and <= 2);
            Assert.IsTrue(world.Jobs.Count > 0);
            Assert.IsTrue(world.Houses.Count is >= 20 and <= 49);
            Assert.IsTrue(world.Apartments.Count is >= 1 and <= 2);
            Assert.IsTrue(world.Citizens.Count is >= 40 and <= 99);
            Assert.AreEqual(0, world.BankAccount.Count);
        }

        [TestMethod]
        public void InitializeCity_AssignsEveryCitizenAJobAndHome()
        {
            World world = new GameEngine().InitializeCity();

            Assert.IsTrue(world.Citizens.All(c => c.Job is not null));
            Assert.IsTrue(world.Citizens.All(c => !string.IsNullOrWhiteSpace(c.HomeAddress)));
            Assert.AreEqual(world.Citizens.Count, world.Citizens.Select(c => c.Id).Distinct().Count());
        }
    }
}
