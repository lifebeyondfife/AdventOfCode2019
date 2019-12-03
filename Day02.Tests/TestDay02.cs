using System.Collections.Generic;
using System.Linq;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay02
    {
        [Theory]
        [InlineData(new [] {1,0,0,0,99}, new [] {2,0,0,0,99})]
        [InlineData(new [] {2,3,0,3,99}, new [] {2,3,0,6,99})]
        [InlineData(new [] {2,4,4,5,99,0}, new [] {2,4,4,5,99,9801})]
        [InlineData(new [] {1,1,1,4,99,5,6,0,99}, new [] {30,1,1,4,2,5,6,0,99})]
        public void TestSolution1(IEnumerable<int> intcode, IEnumerable<int> expected)
        {
            var day02 = new Day02(intcode);

            foreach (var x in day02.FinalState().Zip(expected, (a, b) => new { Expected = a, Actual = b }))
            {
                Assert.Equal(x.Expected, x.Actual);
            }
        }
    }
}
