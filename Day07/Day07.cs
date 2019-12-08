using System;
using System.Threading.Channels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day07
    {
        private IntCodeMachine Machine { get; set; }
        private static int Level { get; set; }
        private static int Offset { get; set; }

        public Day07(string filename)
        {
            Machine = new IntCodeMachine(
                File.ReadLines(filename).
                SelectMany(x => x.Split(',')).
                Select(Int32.Parse).
                Select((x, i) => new { Key = i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        public Day07(IEnumerable<int> codes)
        {
            Machine = new IntCodeMachine(
                codes.
                Select((x, i) => new { Key = i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );
        }

        private static void Permutations(IList<int> permutations, IList<IList<int>> outputs, int range, int k) 
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

        private IList<IList<int>> Inputs(int offset)
        {
            var range = Enumerable.Repeat(0, 5).ToList();
            var outputs = new List<IList<int>>();

            Level = -1;
            Offset = offset;
            Permutations(range, outputs, 5, 0);

            return outputs;
        }

        private int ChainedInputs(IList<int> inputs)
        {
            var machineOutput = 0;
            Action<int> outputLambda = i => machineOutput = i;

            foreach (var phase in inputs)
            {
                var machineInputs = Channel.CreateUnbounded<int>();
                machineInputs.Writer.WriteAsync(phase);
                machineInputs.Writer.WriteAsync(machineOutput);

                Machine.ExecuteProgram(machineInputs, outputLambda);
            }

            return machineOutput;
        }

        public int ChainedCycleInputs(IList<int> inputs)
        {
            var machineInputs = inputs.Select(x => (IList<int>) new [] { x }.ToList()).ToList();
            machineInputs[0].Add(0);

            return ChainedMachine.ExecuteCycledProgram(Machine, (IList<IList<int>>) machineInputs);
        }

        public int Solution1()
        {
            return Inputs(-1).Select(ChainedInputs).Max();
        }

        public int Solution2()
        {
            return Inputs(4).Select(ChainedCycleInputs).Max();
        }
    }
}