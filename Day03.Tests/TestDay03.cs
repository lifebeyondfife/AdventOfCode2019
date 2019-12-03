using System.Collections.Generic;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay03
    {
        [Theory]
        [InlineData(new [] {"R8,U5,L5,D3", "U7,R6,D4,L4"}, 6)]
        [InlineData(new [] {"R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83"}, 159)]
        [InlineData(new [] {"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"}, 135)]
        public void TestSolution1(IEnumerable<string> wires, int expected)
        {
            var day03 = new Day03(wires);
            
            Assert.Equal(expected, day03.Solution1());
        }

        [Theory]
        [InlineData(new [] {"R8,U5,L5,D3", "U7,R6,D4,L4"}, 30)]
        [InlineData(new [] {"R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83"}, 610)]
        [InlineData(new [] {"R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7"}, 410)]
        public void TestSolution2(IEnumerable<string> wires, int expected)
        {
            var day03 = new Day03(wires);
            
            Assert.Equal(expected, day03.Solution2());
        }
    }
}
