using System.Collections.Concurrent;
using PeopleVille.Core.Enum;
using PeopleVille.Core.Models.Home;
using PeopleVille.Core.Operations;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, Genders gender, Family? family, FamilyRoles familialStatus, int? homeId, ConcurrentBag<object>? actionSink = null)
    {
        public Citizen(World world, int id, string firstName, string lastName, DateTime birth, Genders gender, homeId)
            // Delegate to the primary constructor without creating a default family.
            : this(world, id, firstName, lastName, birth, gender, null, FamilyRoles.Adult, null)
        {
        }

        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public Genders Gender { get; } = gender;
        public Family? Family { get; set; } = family;
        public FamilyRoles FamilyRoles { get; set; } = familialStatus;
        public BankAccount BankAccount { get; set; } = new();
        public int? HomeId { get; set; } = homeId;
        public Job? Job { get; set; }

        public required string CurrentLocation { get; set; }
        public School? School { get; set; }

        private static readonly int percentage = 50;
        private static readonly double multiplier = percentage / 100.0; // 0.XX
        private readonly ConcurrentBag<object>? _actionSink = actionSink;

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

            CurrentLocation = HomeId.Address;

            if (currentHour == 22) // eating time
            {
                Eat();
                return;
            }

            if (currentHour < 7 || currentHour > 22) // sleeping time
            {
                PublishCitizenAction($"{FirstName} {LastName} is sleeping.");
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
                PublishCitizenAction($"Attending school at {world.currentDateTime}");
            }
            else if (yearsOld >= 18 && Job != null && currentHour >= Job.Workplace.WorkStartTime && currentHour < Job.Workplace.WorkEndTime) // work time
            {
                CurrentLocation = Job.Workplace.Address;
                PublishCitizenAction($"Working at {Job.Workplace.Address} at {world.currentDateTime}");
            }
        }

        private void PublishCitizenAction(string message)
        {
            int yearsOld = world.currentDateTime.Year - Birth.Year;
            if (world.currentDateTime.Date < Birth.Date.AddYears(yearsOld))
            {
                yearsOld--;
            }

            _actionSink?.Add(new CitizenOperation
            {
                CitizenId = Id,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender.ToString(),
                HomeAddress = HomeId.Address,
                CurrentLocation = CurrentLocation,
                IsAdult = yearsOld >= 18,
                IsEmployed = Job is not null,
                Message = message
            });
        }

        private void AddMoney()
        {
            if (Job is null)
            {
                return;
            }

            if (FamilyRoles is FamilyRoles.Child)
            {
                AppendSalaryToBalance(Job, true);
                return;
            }
            
            AppendSalaryToBalance(Job, false);
            return;
        }

        private void AppendSalaryToBalance(Job job, bool isChild)
        {
            lock (BankAccount.Balance)
            {
                if (isChild)
                {
                    int incomePool = Job.Workplace.Salary * multiplier; // under 18 pay
                    HomeId.HouseholdFunds.Balance += incomePool * multiplier;
                    BankAccount.Balance += incomePool;
                } else {
                    int incomePool = Job.Workplace.Salary;
                    HomeId.HouseholdFunds.Balance += incomePool * multiplier; // might have issues if odd number turns into decimal unless handled elsewhere in code or generally rounded up/down
                    BankAccount.Balance += incomePool;
                }
            }

            PublishCitizenAction($"Received salary of {Job.Workplace.Salary} at {world.currentDateTime}");
        }

        private Building FindHome()
        {
            return world.Houses.Cast<Building>()
                .Concat(world.Apartments)
                .FirstOrDefault(home => home.HomeId == HomeId)!; // no homeless yet so can be nullforgiven
        }

        private bool GetHomeResources()
        {
            Building home = FindHome();

            return home switch
            {
                House house => house.FoodInventory < 5 || house.WaterInventory < 5,
                Apartment apartment => apartment.FoodInventory < 5 || apartment.WaterInventory < 5,
                _ => false // find solution for citizen with no found address
            };
        }

        private void Eat()
        {
            Building home = FindHome();
            Random rnd = new();

            int foodConsumed = rnd.Next(2, 5);
            int waterConsumed = rnd.Next(2, 5);

            switch (home)
            {
                case House house when foodConsumed <= house.FoodInventory && waterConsumed <= house.WaterInventory:
                    house.FoodInventory -= foodConsumed;
                    house.WaterInventory -= waterConsumed;
                    break;
                case Apartment apartment when foodConsumed <= apartment.FoodInventory && waterConsumed <= apartment.WaterInventory:
                    apartment.FoodInventory -= foodConsumed;
                    apartment.WaterInventory -= waterConsumed;
                    break;
                default:
                    break;
            }
        }

        private void BegForMoney(Building home, Random rnd)
        {

            if (rnd.Next(0, 5) == 0) // 20% chance for begging to succeed
            {
                Building? homeWithMostFood = world.Houses
                    .Cast<Building>()
                    .Concat(world.Apartments)
                    .Where(h => h != home && (h is House h1 ? h1.FoodInventory >= 10 : (h as Apartment)!.FoodInventory >= 10))
                    .OrderByDescending(h => h is House h1 ? h1.FoodInventory : (h as Apartment)!.FoodInventory)
                    .FirstOrDefault();

                Building? homeWithMostWater = world.Houses
                    .Cast<Building>()
                    .Concat(world.Apartments)
                    .Where(h => h != home && (h is House h1 ? h1.WaterInventory >= 10 : (h as Apartment)!.WaterInventory >= 10))
                    .OrderByDescending(h => h is House h1 ? h1.WaterInventory : (h as Apartment)!.WaterInventory)
                    .FirstOrDefault();

                switch (homeWithMostFood)
                {
                    case House house:
                        house.FoodInventory -= 10;
                        break;
                    case Apartment apartment:
                        apartment.FoodInventory -= 10;
                        break;
                }

                switch (home)
                {
                    case House currentHouse:
                        currentHouse.FoodInventory += 10;
                        break;
                    case Apartment currentApartment:
                        currentApartment.FoodInventory += 10;
                        break;
                }

                if (homeWithMostWater != null && homeWithMostWater is House house2)
                {
                    house2.WaterInventory -= 10;
                    if (home is House currentHouse)
                    {
                        currentHouse.WaterInventory += 10;
                    }
                    else if (home is Apartment currentApartment)
                    {
                        currentApartment.WaterInventory += 10;
                    }
                }
                else if (homeWithMostWater != null && homeWithMostWater is Apartment apartment2)
                {
                    apartment2.WaterInventory -= 10;
                    if (home is House currentHouse)
                    {
                        currentHouse.WaterInventory += 10;
                    }
                    else if (home is Apartment currentApartment)
                    {
                        currentApartment.WaterInventory += 10;
                    }
                }
            }
        }

        private void ReduceHomeBalance()
        {
            Building home = FindHome();
            Random rnd = new();
            ShoppingCenter? shoppingCenter = world.ShoppingCenters[rnd.Next(world.ShoppingCenters.Count)];

            lock (Citizen.BankAccount)
            {
                switch (home)
                {
                    case House:
                    case Apartment:
                        if (Citizen.BankAccount.Balance <= shoppingCenter.FoodPrice * 5 && Citizen.BankAccount.Balance <= shoppingCenter.WaterPrice * 5)
                        {
                            world.Citizens.Remove(this); // citizen dies
                        }
                        else
                        {
                            Citizen.BankAccount.Balance -= shoppingCenter.FoodPrice * 5;
                            balance -= shoppingCenter.WaterPrice * 5;
                            CurrentLocation = shoppingCenter.Address;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
