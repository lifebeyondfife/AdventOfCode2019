using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day09 = new Day09("input");

            Console.WriteLine($"Day 09 Solution 1: {day09.Solution1()}");
            Console.WriteLine($"Day 09 Solution 2: {day09.Solution2()}");
        }
    }
}
