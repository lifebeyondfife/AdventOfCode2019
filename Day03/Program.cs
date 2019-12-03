using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day03 = new Day03("input");

            Console.WriteLine($"Day 03 Solution 1: {day03.Solution1()}");
            Console.WriteLine($"Day 03 Solution 2: {day03.Solution2()}");
        }
    }
}
