using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;
using PeopleVille.Core.Interfaces;

namespace PeopleVille.Core.Models
{
    public class World
    {
        private int time;
        private int weekDay;
        public DateTime currentDateTime = DateTime.UtcNow;
        public List<Citizen> Citizens { get; set; } = new List<Citizen>();
        public List<BankAccount> BankAccount { get; set; } = new List<BankAccount>();
        public List<Job> Jobs { get; set; } = new List<Job>();
        public List<ShoppingCenter> ShoppingCenters { get; set; } = new List<ShoppingCenter>();
        public List<IWorkplace> Workplaces { get; set; } = new List<IWorkplace>();
        public List<House> Houses { get; set; } = new List<House>();
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();
        public List<School> Schools { get; set; } = new List<School>();

        public int Time
        {
            get => time;
            set
            {
                if (value >= 24)
                    time = 1;
                else
                    time = value;
                if (time < 6 && time > 21) IsNight = true;
                else IsNight = false;
            }
        }
        public int WeekDay
        {
            get => weekDay;
            set
            {
                if (value >= 7)
                    weekDay = 1;
                else
                    weekDay = value;
            }
        }
    }
}