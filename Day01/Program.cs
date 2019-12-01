using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day01 = new Day01("input");

            Console.WriteLine($"Day 01 Solution 1: {day01.Solution1()}");
            Console.WriteLine($"Day 01 Solution 2: {day01.Solution2()}");
        }
    }
}
