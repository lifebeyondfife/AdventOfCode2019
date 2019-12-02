using System;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day02 = new Day02("input");
            
            Console.WriteLine($"Day 02 Solution 1: {day02.Solution1()}");
            Console.WriteLine($"Day 02 Solution 2: {day02.Solution2()}");
        }
    }
}
