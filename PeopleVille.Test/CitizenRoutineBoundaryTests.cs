using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;

namespace PeopleVille.Test
{
    [TestClass]
    public class CitizenRoutineBoundaryTests
    {
        [DataTestMethod]
        [DataRow(8, "Home")]
        [DataRow(9, "Work")]
        [DataRow(16, "Work")]
        [DataRow(17, "Home")]
        public void WorkSchedule_IncludesStartAndExcludesEnd(int hour, string expectedLocation)
        {
            var (world, citizen, home) = CreateCitizen(hour);
            citizen.Job = new Job(new ShoppingCenter
            {
                Address = "Work", WorkStartTime = 9, WorkEndTime = 17
            });

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(expectedLocation, citizen.CurrentLocation);
        }

        [DataTestMethod]
        [DataRow(7, "Home")]
        [DataRow(8, "School")]
        [DataRow(14, "School")]
        [DataRow(15, "Home")]
        public void SchoolSchedule_IncludesStartAndExcludesEnd(int hour, string expectedLocation)
        {
            var (world, citizen, home) = CreateCitizen(hour, new DateTime(2010, 1, 1));
            citizen.School = new School
            {
                Address = "School",
                StartTime = world.currentDateTime.Date.AddHours(8),
                EndTime = world.currentDateTime.Date.AddHours(15)
            };

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(expectedLocation, citizen.CurrentLocation);
        }

        [DataTestMethod]
        [DataRow(14, "School")]
        [DataRow(15, "Work")]
        [DataRow(16, "Work")]
        public void EighteenthBirthday_SwitchesFromSchoolToWorkOnBirthday(int day, string expectedLocation)
        {
            var (world, citizen, home) = CreateCitizen(10, new DateTime(2007, 6, 15));
            world.currentDateTime = new DateTime(2025, 6, day, 10, 0, 0);
            citizen.School = new School
            {
                Address = "School",
                StartTime = world.currentDateTime.Date.AddHours(8),
                EndTime = world.currentDateTime.Date.AddHours(15)
            };
            citizen.Job = new Job(new ShoppingCenter
            {
                Address = "Work", WorkStartTime = 9, WorkEndTime = 17
            });

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(expectedLocation, citizen.CurrentLocation);
        }

        [DataTestMethod]
        [DataRow(0, 200)]
        [DataRow(1, 0)]
        [DataRow(23, 0)]
        public void Salary_IsDepositedIntoCitizenAccountOnlyAtMidnight(int hour, int expectedBalance)
        {
            var (world, citizen, home) = CreateCitizen(hour);
            BankAccount account = UseHome(world, home, true);
            account.Balance = 25;
            var otherHome = new House { Address = "Other home" };
            otherHome.BankAccount.Balance = 75;
            world.Houses.Insert(0, otherHome);
            citizen.Job = new Job(new ShoppingCenter { Address = "Work", Salary = 200 });

            citizen.PerformHourlyRoutine();

            Assert.AreEqual((decimal)expectedBalance, citizen.BankAccount.Balance);
            Assert.AreEqual(25m, account.Balance);
            Assert.AreEqual(75m, otherHome.BankAccount.Balance);
            Assert.AreEqual(home.Address, citizen.CurrentLocation);
        }

        [DataTestMethod]
        [DataRow(false, 4, 10)]
        [DataRow(false, 10, 4)]
        [DataRow(true, 4, 10)]
        [DataRow(true, 10, 4)]
        public void Shopping_ChargesFiveUnitsOfEachResourceWhenEitherIsLow(bool apartment, int food, int water)
        {
            var (world, citizen, home) = CreateCitizen(8);
            home.FoodInventory = food;
            home.WaterInventory = water;
            BankAccount account = UseHome(world, home, apartment);
            account.Balance = 100m;
            world.ShoppingCenters.Add(new ShoppingCenter
            {
                Address = "Shop", FoodPrice = 1.5m, WaterPrice = 2.25m
            });

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(81.25m, account.Balance);
            Assert.AreEqual("Shop", citizen.CurrentLocation);
            Assert.IsTrue(world.Citizens.Contains(citizen));
        }

        [DataTestMethod]
        [DataRow(6, 4, "Home", 100)]
        [DataRow(7, 4, "Shop", 90)]
        [DataRow(8, 4, "Shop", 90)]
        [DataRow(9, 4, "Home", 100)]
        [DataRow(8, 5, "Home", 100)]
        public void Shopping_RespectsHoursAndInventoryThreshold(int hour, int inventory, string location, int balance)
        {
            var (world, citizen, home) = CreateCitizen(hour);
            home.FoodInventory = home.WaterInventory = inventory;
            home.BankAccount.Balance = 100;
            world.ShoppingCenters.Add(new ShoppingCenter
            {
                Address = "Shop", FoodPrice = 1, WaterPrice = 1
            });

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(location, citizen.CurrentLocation);
            Assert.AreEqual((decimal)balance, home.BankAccount.Balance);
        }

        [TestMethod]
        public void LowResources_WithoutShoppingCenters_LeavesCitizenHomeAndBalanceUnchanged()
        {
            var (world, citizen, home) = CreateCitizen(8);
            home.FoodInventory = home.WaterInventory = 0;
            home.BankAccount.Balance = 100;

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(home.Address, citizen.CurrentLocation);
            Assert.AreEqual(100m, home.BankAccount.Balance);
            Assert.IsTrue(world.Citizens.Contains(citizen));
        }

        [DataTestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void Shopping_WithNoFunds_RemovesOnlyAffectedCitizenWithoutChargingHome(bool apartment)
        {
            var (world, citizen, home) = CreateCitizen(8);
            home.FoodInventory = 0;
            BankAccount account = UseHome(world, home, apartment);
            var neighbor = new Citizen(world, 2, "Other", "Citizen", new DateTime(1990, 1, 1),
                0, "Other home") { CurrentLocation = "Other home" };
            world.Citizens.Add(neighbor);
            world.ShoppingCenters.Add(new ShoppingCenter
            {
                Address = "Shop", FoodPrice = 1, WaterPrice = 1
            });

            citizen.PerformHourlyRoutine();

            Assert.IsFalse(world.Citizens.Contains(citizen));
            Assert.IsTrue(world.Citizens.Contains(neighbor));
            Assert.AreEqual(0m, account.Balance);
        }

        private static BankAccount UseHome(World world, House home, bool apartment)
        {
            if (!apartment)
                return home.BankAccount;

            world.Houses.Remove(home);
            var replacement = new Apartment
            {
                Address = home.Address,
                FoodInventory = home.FoodInventory,
                WaterInventory = home.WaterInventory
            };
            world.Apartments.Add(replacement);
            return replacement.BankAccount;
        }

        private static (World World, Citizen Citizen, House Home) CreateCitizen(int hour, DateTime? birth = null)
        {
            var world = new World { currentDateTime = new DateTime(2025, 6, 15, hour, 0, 0) };
            var home = new House { Address = "Home", FoodInventory = 10, WaterInventory = 10 };
            world.Houses.Add(home);
            var citizen = new Citizen(world, 1, "Test", "Citizen", birth ?? new DateTime(1990, 1, 1),
                0, home.Address) { CurrentLocation = "Away" };
            world.Citizens.Add(citizen);
            return (world, citizen, home);
        }
    }
}
