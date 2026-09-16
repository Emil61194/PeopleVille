using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleVille.Core.Data
{
    public class Job
    {
        public static string[] JobTitles = new string[]
        {
            "Software Engineer", "Data Scientist", "Product Manager", "UX Designer", "Marketing Specialist",
            "Sales Representative", "Financial Analyst", "Human Resources Manager", "Operations Manager", "Customer Support Specialist"
        };

        public static decimal[] JobSalaries = new decimal[]
        {
            80000m, 90000m, 95000m, 85000m, 70000m,
            60000m, 75000m, 80000m, 85000m, 55000m
        };

        public static int[] JobWorkStartTimes = new int[]
            {
                9, 9, 9, 10, 9,
                9, 9, 9, 8, 9
            };

        public static int[] JobWorkEndTimes = new int[]
            {
                17, 17, 18, 18, 17,
                17, 17, 17, 16, 17
            };
    }
}
