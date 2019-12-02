using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day02
    {
        private IDictionary<int, int> OriginalIntcode { get; set; }
        private IDictionary<int, int> Intcode { get; set; }
        
        public Day02(string filename)
        {
            OriginalIntcode = File.ReadLines(filename).
                SelectMany(x => x.Split(',')).
                Select(Int32.Parse).
                Select((x, i) => new { Key = i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value);
        }

        public Day02(IEnumerable<int> intcode)
        {
            OriginalIntcode = intcode.
                Select((x, i) => new { Key = i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value);
        }

        private void Update(int opcode, int operand1, int operand2, int location)
        {
            Func<int, int, int> addOp = (a, b) => a + b;
            Func<int, int, int> multiplyOp = (a, b) => a * b;
            var compute = new List<Func<int, int, int>>(new [] {addOp, multiplyOp});

            Intcode[location] = compute[opcode - 1](Intcode[operand1], Intcode[operand2]);
        }

        private void ExecuteProgram(int? noun, int? verb)
        {
            var intcode = new KeyValuePair<int, int>[OriginalIntcode.Count];
            OriginalIntcode.CopyTo(intcode, 0);
            Intcode = intcode.ToDictionary(x => x.Key, x => x.Value);

            if (noun.HasValue)
                Intcode[1] = noun.Value;
            if (verb.HasValue)
                Intcode[2] = verb.Value;
            
            for (var i=0; i < Intcode.Keys.Count; i += 4)
            {
                if (Intcode[i] == 99)
                    return;
                
                Update(Intcode[i], Intcode[i+1], Intcode[i+2], Intcode[i+3]);
            }
        }

        public IEnumerable<int> FinalState()
        {
            ExecuteProgram(null, null);
            foreach (var kvp in Intcode)
                yield return kvp.Value;
        }

        public int Solution1()
        {
            ExecuteProgram(12, 2);

            return Intcode[0];
        }

        public int Solution2()
        {
            foreach (var noun in Enumerable.Range(0, 100))
            {
                foreach (var verb in Enumerable.Range(0, 100))
                {
                    ExecuteProgram(noun, verb);

                    if (Intcode[0] == 19690720)
                        return 100 * noun + verb;
                }
            }

            throw new ApplicationException("Noun and verb not found");
        }
    }
}