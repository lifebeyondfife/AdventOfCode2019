using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day03
    {
        private IDictionary<Coordinate, int> FrontPanel { get; set; }
        private IList<string> Wire1 { get; set; }
        private IList<string> Wire2 { get; set; }

        public Day03(string filename)
        {
            FrontPanel = new Dictionary<Coordinate, int>();
            var wires = File.ReadAllLines(filename).ToList();

            Wire1 = wires[0].Split(',');
            Wire2 = wires[1].Split(',');
        }

        public Day03(IEnumerable<string> wires)
        {
            FrontPanel = new Dictionary<Coordinate, int>();
            Wire1 = wires.First().Split(',');
            Wire2 = wires.Skip(1).First().Split(',');
        }

        private void PopulateFrontPanel(Action<Coordinate, int> action)
        {
            var counter = 0;
            foreach (var coordinate in GetRoute(Wire1))
            {
                FrontPanel[coordinate] = counter++;
            }

            FrontPanel[new Coordinate(0, 0)] = counter = 0;

            foreach (var coordinate in GetRoute(Wire2))
            {
                if (FrontPanel.ContainsKey(coordinate) && FrontPanel[coordinate] > 0)
                    action(coordinate, FrontPanel[coordinate] + counter);
                
                ++counter;
            }
        }

        private IEnumerable<Coordinate> GetRoute(IList<string> wire)
        {
            Coordinate.Location = new Coordinate(0, 0);
            var coordinatesWire = wire.Select(x => new Coordinate(x)).ToList();

            coordinatesWire.Insert(0, new Coordinate(0, 0));

            return coordinatesWire.
                Zip(coordinatesWire.Skip(1), (a, b) => new { First = a, Second = b }).
                Select(c => Coordinate.PointsBetween(c.First, c.Second)).
                SelectMany(c => c);
        }

        public int Solution1()
        {
            var crossedWires = new List<Coordinate>();
            Action<Coordinate, int> action = (c, _) => crossedWires.Add(c);

            PopulateFrontPanel(action);

            var origin = new Coordinate(0, 0);
            return crossedWires.Min(x => x.ManhattanDistance(origin));
        }

        public int Solution2()
        {
            var crossedWireScores = new List<int>();
            Action<Coordinate, int> action = (_, s) => crossedWireScores.Add(s);

            PopulateFrontPanel(action);

            return crossedWireScores.Min();
        }
    }
}