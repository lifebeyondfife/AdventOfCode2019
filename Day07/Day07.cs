using System;
using System.Threading.Channels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdventOfCode2019.Library;

namespace AdventOfCode2019.Solutions
{
    public class Day07
    {
        private IntCodeMachine Machine { get; set; }
        private static long Level { get; set; }
        private static long Offset { get; set; }

        public Day07(string filename)
        {
            Machine = new IntCodeMachine(
                File.ReadLines(filename).
                SelectMany(x => x.Split(',')).
                Select(Int64.Parse).
                Select((x, i) => new { Key = (long) i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        public Day07(IEnumerable<long> codes)
        {
            Machine = new IntCodeMachine(
                codes.
                Select((x, i) => new { Key = (long) i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        private static void Permutations(IList<long> permutations, IList<IList<long>> outputs, int range, int k) 
        { 
            permutations[k] = ++Level; 
        
            if (Level == range) 
                outputs.Add(permutations.Select(x => x + Offset).ToList()); 
            else
                foreach (var i in Enumerable.Range(0, range))
                    if (permutations[i] == 0) 
                        Permutations(permutations, outputs, range, i); 

            --Level; 
            permutations[k] = 0;
        } 

        private IList<IList<long>> Inputs(long offset)
        {
            var range = Enumerable.Repeat(0L, 5).ToList();
            var outputs = new List<IList<long>>();

            Level = -1;
            Offset = offset;
            Permutations(range, outputs, 5, 0);

            return outputs;
        }

        private long ChainedInputs(IList<long> inputs)
        {
            var machineOutput = 0L;
            Action<long> outputLambda = i => machineOutput = i;

            foreach (var phase in inputs)
            {
                var machineInputs = Channel.CreateUnbounded<long>();
                machineInputs.Writer.WriteAsync(phase);
                machineInputs.Writer.WriteAsync(machineOutput);

                Machine.ExecuteProgram(machineInputs, outputLambda);
            }

            return machineOutput;
        }

        public long ChainedCycleInputs(IList<long> inputs)
        {
            var machineInputs = inputs.Select(x => (IList<long>) new long[] { x }.ToList()).ToList();
            machineInputs[0].Add(0);

            return ChainedMachine.ExecuteCycledProgram(Machine, (IList<IList<long>>) machineInputs);
        }

        public long Solution1()
        {
            return Inputs(-1).Select(ChainedInputs).Max();
        }

        public long Solution2()
        {
            return Inputs(4).Select(ChainedCycleInputs).Max();
        }
    }
}