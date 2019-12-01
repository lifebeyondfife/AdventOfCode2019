using System;
using System.Collections.Generic;
using Xunit;

using AdventOfCode2019.Solutions;

namespace AdventOfCode2019.Tests
{
    public class TestDay01
    {
        [Theory]
        [InlineData(new [] {12}, 2)]
        [InlineData(new [] {14}, 2)]
        [InlineData(new [] {1969}, 654)]
        [InlineData(new [] {100756}, 33583)]
        public void TestSolution1(IEnumerable<int> moduleAmounts, int expectedFuel)
        {
            var day01 = new Day01(moduleAmounts);
            Assert.Equal(expectedFuel, day01.Solution1());
        } 

        [Theory]
        [InlineData(new [] {14}, 2)]
        [InlineData(new [] {1969}, 966)]
        [InlineData(new [] {100756}, 50346)]
        public void TestSolution2(IEnumerable<int> moduleAmounts, int expectedFuel)
        {
            var day01 = new Day01(moduleAmounts);
            Assert.Equal(expectedFuel, day01.Solution2());
        } 
    }
}
