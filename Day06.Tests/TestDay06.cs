using System.Collections.Generic;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay06
    {
        [Theory]
        [InlineData(new [] {"COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L"}, 42)]
        public void TestSolution1(IEnumerable<string> orbits, int expected)
        {
            var day06 = new Day06(orbits);

            Assert.Equal(expected, day06.Solution1());
        }

        [Theory]
        [InlineData(new [] {"COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN"}, 4)]
        public void TestSolution2(IEnumerable<string> orbits, int expected)
        {
            var day06 = new Day06(orbits);
            
            Assert.Equal(expected, day06.Solution2());
        }
    }
}
