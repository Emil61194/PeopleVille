using PeopleVille.Core.Enum;
using PeopleVille.Core.Models.Home;
using PeopleVille.Core.Operations;
using System.Collections.Concurrent;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, Genders gender, string homeAddress, ConcurrentBag<object> actions)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public Genders Gender { get; } = gender;
        public string HomeAddress { get; set; } = homeAddress;
        public Job? Job { get; set; }
        public required string CurrentLocation { get; set; }
        public School? School { get; set; }

        public void PerformHourlyRoutine()
        {
            int currentHour = world.currentDateTime.Hour;
            int yearsOld = world.currentDateTime.Year - Birth.Year;

            if (Job != null && currentHour == 0) // paycheck
            {
                AddMoney();
            }

            if (world.currentDateTime.Date < Birth.Date.AddYears(yearsOld))
            {
                yearsOld--;
            }

            CurrentLocation = HomeAddress;

            if (currentHour == 22) // eating time
            {
                Eat();
                return;
            }

            if (currentHour < 7 || currentHour > 22) // sleeping time
            {
                actions.Add(new CitizenOperation
                {
                    CitizenId = Id,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender == 0 ? "Male" : "Female",
                    HomeAddress = HomeAddress,
                    CurrentLocation = CurrentLocation,
                    IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                    IsEmployed = Job != null,
                    Message = $"{FirstName} {LastName} is sleeping."
                }); 
                return;
            }

            if (GetHomeResources() && currentHour >= 7 && currentHour < 9 && world.ShoppingCenters.Count > 0) // shopping time
            {
                ReduceHomeBalance();
                return;
            }

            if (yearsOld < 18 && School != null && currentHour >= School.StartTime.Hour && currentHour < School.EndTime.Hour) // school time
            {
                CurrentLocation = School.Address;
                actions.Add(new CitizenOperation
                {
                    CitizenId = Id,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender == 0 ? "Male" : "Female",
                    HomeAddress = HomeAddress,
                    CurrentLocation = CurrentLocation,
                    IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                    IsEmployed = Job != null,
                    Message = $"{this.FirstName} {this.LastName} Attending school at {world.currentDateTime}"
                });
            }
            else if (yearsOld >= 18 && Job != null && currentHour >= Job.Workplace.WorkStartTime && currentHour < Job.Workplace.WorkEndTime) // work time
            {
                CurrentLocation = Job.Workplace.Address;
                actions.Add(new CitizenOperation
                {
                    CitizenId = Id,
                    FirstName = FirstName,
                    LastName = LastName,
                    Gender = Gender == 0 ? "Male" : "Female",
                    HomeAddress = HomeAddress,
                    CurrentLocation = CurrentLocation,
                    IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                    IsEmployed = Job != null,
                    Message = $"{this.FirstName} {this.LastName} Working at {Job!.Workplace.Address} at {world.currentDateTime}"
                });
            }
        }

        private void AddMoney()
        {
            Building home = FindHome();

            if (home is House house)
            {
                house.BankAccount.Balance += Job!.Workplace.Salary;

            }
            else if (home is Apartment apartment)
            {
                apartment.BankAccount.Balance += Job!.Workplace.Salary;
            }
        }

        private Building FindHome()
        {
            return world.Houses
                .Cast<Building>()
                .Concat(world.Apartments)
                .FirstOrDefault(home => home.Address == HomeAddress)!; // no homeless yet so can be nullforgiven
        }

        private bool GetHomeResources()
        {
            Building home = FindHome();

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
                return false; // find solution for citizen with no found address
            }
        }

        private void Eat()
        {
            Building home = FindHome();
            Random rnd = new Random();

            int foodConsumed = rnd.Next(2, 5);
            int waterConsumed = rnd.Next(2, 5);

            if (home is House house)
            {

                if (foodConsumed < house.FoodInventory || waterConsumed < house.WaterInventory)
                {
                    house.FoodInventory -= foodConsumed;
                    house.WaterInventory -= waterConsumed;
                }
            }
            else if (home is Apartment apartment)
            {
                if (foodConsumed < apartment.FoodInventory || waterConsumed < apartment.WaterInventory)
                {
                    apartment.FoodInventory -= foodConsumed;
                    apartment.WaterInventory -= waterConsumed;
                }
            }
        }
        private void ReduceHomeBalance()
        {
            Building home = FindHome();
            Random rnd = new Random();
            ShoppingCenter? shoppingCenter = world.ShoppingCenters[rnd.Next(world.ShoppingCenters.Count)];

            if (home is House house)
            {
                if (house.BankAccount.Balance < shoppingCenter.FoodPrice * 5 || house.BankAccount.Balance < shoppingCenter.WaterPrice * 5)
                {
                    actions.Add($"{new CitizenOperation
                    {
                        CitizenId = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Gender = Gender == 0 ? "Male" : "Female",
                        HomeAddress = HomeAddress,
                        CurrentLocation = CurrentLocation,
                        IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                        IsEmployed = Job != null,
                        Message = $"{this.FirstName + ' ' + this.LastName } Citizen has died due to insufficient funds at {world.currentDateTime}"
                    }}");
                    world.Citizens.Remove(this); // citizen dies
                }
                else
                {
                    house.BankAccount.Balance -= shoppingCenter.FoodPrice * 5;
                    house.BankAccount.Balance -= shoppingCenter.WaterPrice * 5;
                    CurrentLocation = shoppingCenter.Address;
                    actions.Add($"{new CitizenOperation
                    {
                        CitizenId = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Gender = Gender == 0 ? "Male" : "Female",
                        HomeAddress = HomeAddress,
                        CurrentLocation = CurrentLocation,
                        IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                        IsEmployed = Job != null,
                        Message = $"{this.FirstName + ' ' + this.LastName } Citizen has shopped for food and water at {shoppingCenter.Address} at {world.currentDateTime}"
                    }}");
                }
            }
            else if (home is Apartment apartment)
            {
                if (apartment.BankAccount.Balance < shoppingCenter.FoodPrice * 5 || apartment.BankAccount.Balance < shoppingCenter.WaterPrice * 5)
                {
                    actions.Add($"{new CitizenOperation
                    {
                        CitizenId = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Gender = Gender == 0 ? "Male" : "Female",
                        HomeAddress = HomeAddress,
                        CurrentLocation = CurrentLocation,
                        IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                        IsEmployed = Job != null,
                        Message = $"{this.FirstName + ' ' + this.LastName } Citizen has died due to insufficient funds at {world.currentDateTime}"
                    }}");
                    world.Citizens.Remove(this); // citizen dies
                }
                else
                {
                    apartment.BankAccount.Balance -= shoppingCenter.FoodPrice * 5;
                    apartment.BankAccount.Balance -= shoppingCenter.WaterPrice * 5;
                    CurrentLocation = shoppingCenter.Address;
                    actions.Add($"{new CitizenOperation
                    {
                        CitizenId = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Gender = Gender == 0 ? "Male" : "Female",
                        HomeAddress = HomeAddress,
                        CurrentLocation = CurrentLocation,
                        IsAdult = (world.currentDateTime.Year - Birth.Year) >= 18,
                        IsEmployed = Job != null,
                        Message = $"{this.FirstName + ' ' + this.LastName } Citizen has shopped for food and water at {shoppingCenter.Address} at {world.currentDateTime}"
                    }}");
                }
            }
        }
    }
}
