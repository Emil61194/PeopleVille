using Microsoft.VisualStudio.TestTools.UnitTesting;
using PeopleVille.Core.Models;
using PeopleVille.Core.Models.Home;
using System;

namespace PeopleVille.Test
{
    [TestClass]
    public class CitizenDoSomethingTests
    {
        [TestMethod]
        public void DoSomething_DefaultsCitizenToHomeOutsideScheduledActivities()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(18, 1990);
            ShoppingCenter workplace = CreateWorkplace("Workplace", 9, 17);
            citizen.Job = new Job(workplace);
            citizen.CurrentLocation = workplace.Address;

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(home.Address, citizen.CurrentLocation);
        }

        [TestMethod]
        public void DoSomething_KeepsCitizenHomeDuringSleepingHours()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(6, 1990);
            citizen.CurrentLocation = "Away";

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(home.Address, citizen.CurrentLocation);
        }

        [TestMethod]
        public void DoSomething_SendsMinorToSchoolDuringSchoolHours()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(10, 2010);
            citizen.School = new School
            {
                Address = "School",
                StartTime = world.currentDateTime.Date.AddHours(8),
                EndTime = world.currentDateTime.Date.AddHours(15)
            };

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(citizen.School.Address, citizen.CurrentLocation);
        }

        [TestMethod]
        public void DoSomething_SendsAdultToWorkDuringWorkHours()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(10, 1990);
            ShoppingCenter workplace = CreateWorkplace("Workplace", 9, 17);
            citizen.Job = new Job(workplace);

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(workplace.Address, citizen.CurrentLocation);
        }

        [TestMethod]
        public void DoSomething_SendsCitizenShoppingWhenHomeResourcesAreLow()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(8, 1990);
            home.FoodInventory = 4;
            home.WaterInventory = 10;
            home.HouseholdFunds.Balance = 100;

            ShoppingCenter shoppingCenter = new ShoppingCenter
            {
                Address = "Shopping center",
                FoodPrice = 1,
                WaterPrice = 1
            };
            world.ShoppingCenters.Add(shoppingCenter);

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(shoppingCenter.Address, citizen.CurrentLocation);
        }

        [TestMethod]
        public void DoSomething_KeepsMinorHomeWhenNoSchoolIsAssigned()
        {
            (World world, Citizen citizen, House home) = CreateCitizen(10, 2010);
            citizen.CurrentLocation = "Away";

            citizen.PerformHourlyRoutine();

            Assert.AreEqual(home.Address, citizen.CurrentLocation);
        }

        private static (World World, Citizen Citizen, House Home) CreateCitizen(int hour, int birthYear)
        {
            World world = new()
            {
                currentDateTime = new DateTime(2025, 6, 15, hour, 0, 0)
            };

            House home = new()
            {
                Address = "Home",
                HomeId = 1,
                FoodInventory = 10,
                WaterInventory = 10
            };
            world.Houses.Add(home);

            Citizen citizen = new(
                world,
                1,
                "Test",
                "Citizen",
                new DateTime(birthYear, 1, 1),
                0,
                null,
                default, 
                home.HomeId)
            {
                CurrentLocation = "Away"
            };
            world.Citizens.Add(citizen);

            return (world, citizen, home);
        }

        private static ShoppingCenter CreateWorkplace(string address, int startTime, int endTime)
        {
            return new ShoppingCenter
            {
                Address = address,
                WorkStartTime = startTime,
                WorkEndTime = endTime
            };
        }
    }
}
