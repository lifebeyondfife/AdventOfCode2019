using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day08 = new Day08("input");

            Console.WriteLine($"Day 08 Solution 1: {day08.Solution1()}");
            Console.WriteLine($"Day 08 Solution 2: {day08.Solution2()}");
        }
    }
}
