using System.Collections.Generic;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay10
    {
        [Theory]
        [InlineData(new string[] {".#..#", ".....", "#####", "....#", "...##"}, 8)]
        [InlineData(new string[] {"......#.#.", "#..#.#....", "..#######.", ".#.#.###..",
            ".#..#.....", "..#....#.#", "#..#....#.", ".##.#..###", "##...#..#.", ".#....####"}, 33)]
        public void TestSolution1(IEnumerable<string> asteroids, int expected)
        {
            var day10 = new Day10(asteroids);
            Assert.Equal(expected, day10.Solution1());
        }
    }
}
