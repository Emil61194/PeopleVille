using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Data
{
    public class JobOptions
    {


        public static List<decimal> JobSalaries = new List<decimal>
        {
            80000m, 90000m, 95000m, 85000m, 70000m,
            60000m, 75000m, 80000m, 85000m, 55000m
        };

        public static List<int> JobWorkStartTimes = new List<int>
            {
                9, 9, 9, 10, 9,
                9, 9, 9, 8, 9
            };

        public static List<int> JobWorkEndTimes = new List<int>
            {
                17, 17, 18, 18, 17,
                17, 17, 17, 16, 17
            };
    }
}
