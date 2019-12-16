using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day11 = new Day11("input");

            Console.WriteLine($"Day 11 Solution 1: {day11.Solution1()}");
            Console.WriteLine($"Day 11 Solution 2: {day11.Solution2()}");
        }
    }
}
