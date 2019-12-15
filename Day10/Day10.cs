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

        private int ApplyDelta(Coordinate coordinate, Coordinate delta, out Coordinate? foundAsteroid)
        {
            do
            {
                coordinate = new Coordinate(coordinate.X + delta.X, coordinate.Y + delta.Y);

                if (AsteroidField.ContainsKey(coordinate) && AsteroidField[coordinate] == Grid.Asteroid)
                {
                    foundAsteroid = coordinate;
                    return 1;
                }
            } while (AsteroidField.ContainsKey(coordinate));

            foundAsteroid = null;
            return 0;
        }

        private double GetAngle(Coordinate coordinate)
        {
            if (coordinate.X == 0)
                return coordinate.Y < 0 ? 0d : Math.PI;
            
            if (coordinate.Y == 0)
                return coordinate.X > 0 ? Math.PI / 2 : 3 * Math.PI / 2;
            
            if (coordinate.X > 0 && coordinate.Y < 0)
                return Math.Atan(- (double) coordinate.X / (double) coordinate.Y);
            if (coordinate.X > 0 && coordinate.Y > 0)
                return Math.PI / 2 + Math.Atan((double) coordinate.Y / (double) coordinate.X);
            if (coordinate.X < 0 && coordinate.Y > 0)
                return Math.PI + Math.Atan(- (double) coordinate.X / (double) coordinate.Y);
            if (coordinate.X < 0 && coordinate.Y < 0)
                return 3 * Math.PI / 2 + Math.Atan((double) coordinate.Y / (double) coordinate.X);
            
            throw new ApplicationException("Cannot calculate angle of unexpected coordinate");
        }

        private IDictionary<Coordinate, int> AsteroidsSeen()
        {
            var foundAsteroid = default(Coordinate?);

            return Asteroids.
                Select(asteroid => new {
                    Key = asteroid,
                    Value = CoordinateDeltas.Sum(delta => ApplyDelta(asteroid, delta, out foundAsteroid))
                }).
                ToDictionary(x => x.Key, x => x.Value);
        }

        private Coordinate DeleteAsteroids(Coordinate station, IList<Coordinate> deltas, int nthAsteroid)
        {
            var deletedAsteroids = default(int);

            while (deletedAsteroids < nthAsteroid)
            {
                foreach (var delta in deltas)
                {
                    var asteroidFound = default(Coordinate?);

                    if (ApplyDelta(station, delta, out asteroidFound) == 1)
                    {
                        AsteroidField.Remove(asteroidFound.Value);
                        
                        if (++deletedAsteroids == nthAsteroid)
                            return asteroidFound.Value;
                    }
                }
            }

            throw new ApplicationException("Could not find asteroid");
        }

        public int Solution1()
        {
            return AsteroidsSeen().Values.Max();
        }

        public int Solution2(int nthAsteroid)
        {
            var station = AsteroidsSeen().
                OrderByDescending(kvp => kvp.Value).
                First().
                Key;
            
            var orderedDeltas = CoordinateDeltas.
                OrderBy(GetAngle).
                ToList();

            var asteroid = DeleteAsteroids(station, orderedDeltas, nthAsteroid);

            return asteroid.X * 100 + asteroid.Y;
        }
    }
}
