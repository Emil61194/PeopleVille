namespace PeopleVille.Core.Data
{
    public class JobOptions
    {
        public static IReadOnlyList<decimal> JobSalaries { get; } = [
            80000m, 90000m, 95000m, 85000m, 70000m,
            60000m, 75000m, 80000m, 85000m, 55000m
        ];

        public static IReadOnlyList<int> JobWorkStartTimes { get; } = [
            9, 9, 9, 10, 9,
            9, 9, 9, 8, 9
        ];

        public static IReadOnlyList<int> JobWorkEndTimes { get; } = [
            17, 17, 18, 18, 17,
            17, 17, 17, 16, 17
        ];
    }
}
