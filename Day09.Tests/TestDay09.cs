using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay09
    {
        [Theory]
        [InlineData(new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99},
            new long[] {109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99})]
        [InlineData(new long[] {1102,34915192,34915192,7,4,7,99,0}, new long[] {1219070632396864})]
        [InlineData(new long[] {104,1125899906842624,99}, new long[] {1125899906842624})]
        public void TestSolution1(IEnumerable<long> intcode, IEnumerable<long> expected)
        {
            var day09 = new Day09(intcode);

            var outputs = new List<long>();
            Action<long> output = i => outputs.Add(i);

            var inputs = Channel.CreateUnbounded<long>();

            day09.Machine.ExecuteProgram(inputs, output);

            foreach (var x in outputs.Zip(expected, (a, b) => new { Expected = a, Actual = b }))
            {
                Assert.Equal(x.Expected, x.Actual);
            }
        }
    }
}
