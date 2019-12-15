using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day10 = new Day10("input");

            Console.WriteLine($"Day 10 Solution 1: {day10.Solution1()}");
            Console.WriteLine($"Day 10 Solution 2: {day10.Solution2(200)}");
        }
    }
}
