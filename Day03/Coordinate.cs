using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public struct Coordinate
    {
        public static Coordinate Location { get; set; }
        public int X, Y;
        
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(string direction)
        {
            X = Location.X;
            Y = Location.Y;
            var distance = Int32.Parse(direction.Substring(1, direction.Length - 1));

            switch (direction.First())
            {
                case 'U': Y += distance; break;
                case 'D': Y -= distance; break;
                case 'L': X -= distance; break;
                case 'R': X += distance; break;
                default: throw new ApplicationException("Unknown direction.");
            }

            Location = new Coordinate(X, Y);
        }

        public int ManhattanDistance(Coordinate a)
        {
            return Math.Abs(X - a.X) + Math.Abs(Y - a.Y);
        }

        public static IEnumerable<Coordinate> PointsBetween(Coordinate a, Coordinate b)
        {
            return a.X == b.X ? PointsBetweenY(a.X, a.Y, b.Y) : PointsBetweenX(a.Y, a.X, b.X); 
        }

        private static IEnumerable<Coordinate> PointsBetweenY(int x, int y0, int y1)
        {
            var iterator = y0 < y1 ? 1 : -1;
            for (var i = y0; i != y1; i += iterator)
                yield return new Coordinate(x, i);
        }

        private static IEnumerable<Coordinate> PointsBetweenX(int y, int x0, int x1)
        {
            var iterator = x0 < x1 ? 1 : -1;
            for (var i = x0; i != x1; i += iterator)
                yield return new Coordinate(i, y);
        }
    }
}