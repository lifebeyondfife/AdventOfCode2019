using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day01
    {
        private IList<int> ModuleAmounts { get; set; }
        private Func<int, int> Fuel = x => x / 3 - 2;
        private int FuelOfFuel(int amount)
        {
            if (amount < 0)
                return 0;
            
            return Fuel(amount) + Math.Max(0, FuelOfFuel(Fuel(amount)));
        }

        public Day01(string filename)
        {
            ModuleAmounts = File.ReadAllLines(filename).
                Select(Int32.Parse).
                ToList();
        }

        public Day01(IEnumerable<int> moduleAmounts)
        {
            ModuleAmounts = moduleAmounts.ToList();
        }

        public int Solution1()
        {
            return ModuleAmounts.Select(Fuel).Sum();
        }

        public int Solution2()
        {
            return ModuleAmounts.Select(FuelOfFuel).Sum();
        }
    }
}