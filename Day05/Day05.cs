using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.IO;
using System.Linq;

using AdventOfCode2019.Library;

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
                Select(Int64.Parse).
                Select((x, i) => new { Key = (long) i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        public long Solution1()
        {
            var outputs = new List<long>();
            Action<long> output = i => outputs.Add(i);

            var inputs = Channel.CreateUnbounded<long>();
            inputs.Writer.WriteAsync(1);

            Machine.ExecuteProgram(inputs, output);
            
            return outputs.Last();
        }

        public long Solution2()
        {
            var outputs = new List<long>();
            Action<long> output = i => outputs.Add(i);

            var inputs = Channel.CreateUnbounded<long>();
            inputs.Writer.WriteAsync(5);

            Machine.ExecuteProgram(inputs, output);
            
            return outputs.Last();
        }
    }
}