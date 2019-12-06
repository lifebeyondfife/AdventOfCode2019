using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day05
    {
        private IntCodeMachine Machine { get; set; }
        
        public Day05(string filename)
        {
            Machine = new IntCodeMachine(
                File.ReadLines(filename).
                SelectMany(x => x.Split(',')).
                Select(Int32.Parse).
                Select((x, i) => new { Key = i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        public int Solution1()
        {
            Machine.ExecuteProgram(new Stack<int>(new [] { 1 }), out IList<int> outputs);
            
            return outputs.Last();
        }

        public int Solution2()
        {
            Machine.ExecuteProgram(new Stack<int>(new [] { 5 }), out IList<int> outputs);
            
            return outputs.Last();
        }
    }
}