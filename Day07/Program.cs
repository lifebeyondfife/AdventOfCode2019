using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day07 = new Day07("input");

            Console.WriteLine($"Day 07 Solution 1: {day07.Solution1()}");
            Console.WriteLine($"Day 07 Solution 2: {day07.Solution2()}");
        }
    }
}
