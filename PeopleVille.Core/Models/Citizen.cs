using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, int gender, string homeAddress)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public int Gender { get; } = gender;
        public string HomeAddress { get; set; } = homeAddress;
        public Job? Job { get; set; }

        public required string CurrentLocation { get; set; }
        public School? School { get; set; }

        public void DoSomething()
        {
            int currentHour = world.currentDateTime.Hour;
            int yearsOld = world.currentDateTime.Year - Birth.Year;

            if (world.currentDateTime.Date < Birth.Date.AddYears(yearsOld))
            {
                yearsOld--;
            }

            CurrentLocation = HomeAddress;

            if (currentHour == 22) // eating time
            {
                world = Eat(world);
                return;
            }

            if (currentHour < 7 || currentHour > 22) // sleeping time
            {
                return;
            }

            if (GetHomeResources(world) && currentHour >= 7 && currentHour < 9 && world.ShoppingCenters.Count > 0) // shopping time
            {
                world = ReduceHomeBalance(world);
                return;
            }

            if (yearsOld < 18 && School != null && currentHour >= School.StartTime.Hour && currentHour < School.EndTime.Hour) // school time
            {
                CurrentLocation = School.Address;
            }
            else if (yearsOld >= 18 && Job != null && currentHour >= Job.Workplace.WorkStartTime && currentHour < Job.Workplace.WorkEndTime) // work time
            {
                CurrentLocation = Job.Workplace.Address;
            }
        }

        private Building? FindHome(World world)
        {
            return world.Houses
                .Cast<Building>()
                .Concat(world.Apartments)
                .FirstOrDefault(home => home.Address == HomeAddress);
        }

        private bool GetHomeResources(World world)
        {
            Building home = FindHome(world);

            if (home is House house)
            {
                return house.FoodInventory < 5 || house.WaterInventory < 5;
            }
            else if (home is Apartment apartment)
            {
                return apartment.FoodInventory < 5 || apartment.WaterInventory < 5;
            }
            else
            {
                throw new Exception($"Home not found for citizen with address: {HomeAddress}");
            }
        }

        private World Eat(World world)
        {
            Building home = FindHome(world);
            Random rnd = new Random();

            int foodConsumed = rnd.Next(2, 5);
            int waterConsumed = rnd.Next(2, 5);

            if (home is House house)
            {

                if (foodConsumed > house.FoodInventory || waterConsumed > house.WaterInventory)
                {
                    return world; // citizen dies when eating
                }
                else
                {
                    house.FoodInventory -= foodConsumed;
                    house.WaterInventory -= waterConsumed;
                }
            }
            else if (home is Apartment apartment)
            {
                if (foodConsumed > apartment.FoodInventory || waterConsumed > apartment.WaterInventory)
                {
                    return world; // citizen dies when eating
                }
                else
                {
                    apartment.FoodInventory -= foodConsumed;
                    apartment.WaterInventory -= waterConsumed;
                }
            }
            return world;
        }
        private World ReduceHomeBalance(World world)
        {
            Building home = FindHome(world);
            Random rnd = new Random();
            ShoppingCenter? shoppingCenter = world.ShoppingCenters[rnd.Next(world.ShoppingCenters.Count)];

            if (home is House house)
            {
                if (house.BankAccount.Balance < shoppingCenter.FoodPrice * 5 || house.BankAccount.Balance < shoppingCenter.WaterPrice * 5)
                {
                    world.Citizens.Remove(this); // citizen dies
                }
                else
                {
                    house.BankAccount.Balance -= shoppingCenter.FoodPrice * 5;
                    house.BankAccount.Balance -= shoppingCenter.WaterPrice * 5;
                    CurrentLocation = shoppingCenter.Address;
                }
            }
            else if (home is Apartment apartment)
            {
                if (apartment.BankAccount.Balance < shoppingCenter.FoodPrice * 5 || apartment.BankAccount.Balance < shoppingCenter.WaterPrice * 5)
                {
                    world.Citizens.Remove(this); // citizen dies
                }
                else
                {
                    apartment.BankAccount.Balance -= shoppingCenter.FoodPrice * 5;
                    apartment.BankAccount.Balance -= shoppingCenter.WaterPrice * 5;
                    CurrentLocation = shoppingCenter.Address;
                }
            }
            return world;
        }
    }
}
