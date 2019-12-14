using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdventOfCode2019.Library;

namespace AdventOfCode2019.Solutions
{
    public enum Grid
    {
        Asteroid,
        Space
    }
    public class Day10
    {
        IDictionary<Coordinate, Grid> AsteroidField { get; set; }
        IList<Coordinate> Asteroids { get; set;}
        ISet<Coordinate> CoordinateDeltas { get; set; }

        public Day10(string filename)
        {
            AsteroidField = File.ReadLines(filename).
                Select((s, j) => s.
                    Select((c, i) => new {
                        Key = new Coordinate(i, j),
                        Value = c == '.' ? Grid.Space : Grid.Asteroid
                    })
                ).
                SelectMany(x => x).
                ToDictionary(x => x.Key, x => x.Value);

            Asteroids = AsteroidField.
                Where(x => x.Value == Grid.Asteroid).
                Select(x => x.Key).
                ToList();

            GenerateCoordinateDeltas();
        }

        public Day10(IEnumerable<string> asteroids)
        {
            AsteroidField = asteroids.
                Select((s, j) => s.
                    Select((c, i) => new {
                        Key = new Coordinate(i, j),
                        Value = c == '.' ? Grid.Space : Grid.Asteroid
                    })
                ).
                SelectMany(x => x).
                ToDictionary(x => x.Key, x => x.Value);

            Asteroids = AsteroidField.
                Where(x => x.Value == Grid.Asteroid).
                Select(x => x.Key).
                ToList();

            GenerateCoordinateDeltas();
        }

        private static int GreatestCommonDivisor(int a, int b)
        {
            if (a == 0 && b == 0)
                return 1;

            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
                if (a > b)
                    a -= b;
                else
                    b -= a;

            return Math.Max(a, b);
        }

        private static Coordinate SimplifiedCoordinate(Coordinate coordinate)
        {
            var gcd = GreatestCommonDivisor(coordinate.X, coordinate.Y);
            return new Coordinate(coordinate.X / gcd, coordinate.Y / gcd);
        }

        private void GenerateCoordinateDeltas()
        {
            CoordinateDeltas = Enumerable.Range(0, AsteroidField.Keys.Max(c => c.X) + 1).
                Select(i => Enumerable.Range(0, AsteroidField.Keys.Max(c => c.Y) + 1).
                    Select(j => new [] {
                        new Coordinate(i, j),
                        new Coordinate(-i, j),
                        new Coordinate(i, -j),
                        new Coordinate(-i, -j)
                    })
                ).
                SelectMany(x => x).
                SelectMany(x => x).
                Select(SimplifiedCoordinate).
                ToHashSet();

            CoordinateDeltas.Remove(new Coordinate(0, 0));
        }

        private int ApplyDelta(Coordinate coordinate, Coordinate delta)
        {
            do
            {
                coordinate = new Coordinate(coordinate.X + delta.X, coordinate.Y + delta.Y);

                if (AsteroidField.ContainsKey(coordinate) && AsteroidField[coordinate] == Grid.Asteroid)
                    return 1;
            } while (AsteroidField.ContainsKey(coordinate));

            return 0;
        }

        public int Solution1()
        {
            var asteroidsSeen = Asteroids.
                Select(asteroid => new {
                    Key = asteroid,
                    Value = CoordinateDeltas.Sum(delta => ApplyDelta(asteroid, delta))
                }).
                ToDictionary(x => x.Key, x => x.Value);

            return asteroidsSeen.Values.Max();
        }

        public int Solution2()
        {
            // create a cartesian delta to polar delta function
            // order the deltas by polar
            // find an asteroid delete it
            //  while (previousAsteroid < 200)
            //      for (polar deltas etc.)
            return 0;
        }
    }
}
