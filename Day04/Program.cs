using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var day04 = new Day04("273025-767253");

            Console.WriteLine($"Day 04 Solution 1: {day04.Solution1()}");
            Console.WriteLine($"Day 04 Solution 2: {day04.Solution2()}");
        }
    }
}
