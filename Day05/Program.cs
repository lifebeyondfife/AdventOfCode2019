using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var day05 = new Day05("input");

            Console.WriteLine($"Day 05 Solution 1: {day05.Solution1()}");
            Console.WriteLine($"Day 05 Solution 2: {day05.Solution2()}");
        }
    }
}
