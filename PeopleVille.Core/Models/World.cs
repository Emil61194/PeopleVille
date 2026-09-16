using PeopleVille.Core.Models.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Models
{
    public class World
    {
        private int time;
        private int weekDay;

        public List<Citizen>? Citizens { get; set; }
        public List<BankAccount>? BankAccount { get; set; }
        public List<Job>? Jobs { get; set; }
        public List<ShoppingCenter>? ShoppingCenters { get; set; }
        public List<House>? Houses { get; set; }
        public List<Apartment>? Apartments { get; set; }
        public List<School>? Schools { get; set; }
        public bool IsNight { get; set; }

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