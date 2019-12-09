using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day06 = new Day06("input");

            Console.WriteLine($"Day 06 Solution 1: {day06.Solution1()}");
            Console.WriteLine($"Day 06 Solution 2: {day06.Solution2()}");
        }
    }
}
