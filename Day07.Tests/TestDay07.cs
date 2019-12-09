using System.Linq;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay07
    {
        [Fact]
        public void TestSolution2()
        {
            var day07 = new Day07(new long[] {3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5});
            var actual = day07.ChainedCycleInputs(new long[] {9, 8, 7, 6, 5}.ToList());
            Assert.Equal(139629729, actual);
        }
    }
}
