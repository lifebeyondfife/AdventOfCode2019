using System;
using System.Collections.Generic;
using System.Threading.Channels;
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
            var outputs = new List<int>();
            Action<int> output = i => outputs.Add(i);

            var inputs = Channel.CreateUnbounded<int>();
            inputs.Writer.WriteAsync(1);

            Machine.ExecuteProgram(inputs, output);
            
            return outputs.Last();
        }

        public int Solution2()
        {
            var outputs = new List<int>();
            Action<int> output = i => outputs.Add(i);

            var inputs = Channel.CreateUnbounded<int>();
            inputs.Writer.WriteAsync(5);

            Machine.ExecuteProgram(inputs, output);
            
            return outputs.Last();
        }
    }
}