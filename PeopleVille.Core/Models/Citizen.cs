using PeopleVille.Core.Enum;
using PeopleVille.Core.Models.Home;

namespace PeopleVille.Core.Models
{
    public class Citizen(World world, int id, string firstName, string lastName, DateTime birth, int gender, Family family, FamilyRoles familialStatus, string homeAddress)
    {
        public int Id { get; set; } = id;
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public DateTime Birth { get; } = birth;
        public int Gender { get; } = gender;
        public FamilyRoles FamilyRoles { get; set; } = familialStatus;
        public Family family { get; set; } = family;
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
            }
            else if (yearsOld >= 18 && Job != null && currentHour >= Job.Workplace.WorkStartTime && currentHour < Job.Workplace.WorkEndTime) // work time
            {
                CurrentLocation = Job.Workplace.Address;
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
                throw new Exception($"Home not found for citizen with address: {HomeAddress}");
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
                else
                {
                    BegForMoney(home, rnd);
                }
            }
            else if (home is Apartment apartment)
            {
                if (foodConsumed < apartment.FoodInventory || waterConsumed < apartment.WaterInventory)
                {
                    apartment.FoodInventory -= foodConsumed;
                    apartment.WaterInventory -= waterConsumed;
                }
                else
                {
                    BegForMoney(home, rnd);
                }
            }
        }

        private void BegForMoney(Building home, Random rnd)
        {

            if (rnd.Next(0, 2) == 0) // 50% chance for begging to succeed
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

                if (homeWithMostFood != null && homeWithMostFood is House house)
                {
                    house.FoodInventory -= 10;

                    if (home is House currentHouse)
                    {
                        currentHouse.FoodInventory += 10;
                    }
                    else if (home is Apartment currentApartment)
                    {
                        currentApartment.FoodInventory += 10;
                    }

                }
                else if (homeWithMostFood != null && homeWithMostFood is Apartment apartment)
                {
                    apartment.FoodInventory -= 10;
                    if (home is House currentHouse)
                    {
                        currentHouse.FoodInventory += 10;
                    }
                    else if (home is Apartment currentApartment)
                    {
                        currentApartment.FoodInventory += 10;
                    }
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
        }
    }
}
