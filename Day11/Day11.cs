using System;
using System.Numerics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;

using AdventOfCode2019.Library;

namespace AdventOfCode2019.Solutions
{
    public enum Colour
    {
        Black = 0,
        White = 1
    }

    public class Day11
    {
        private IntCodeMachine Machine { get; set; }
        private IDictionary<Coordinate, Colour> Hull { get; set; }
        private Coordinate RobotPosition { get; set; }
        private Complex RobotDirection { get; set; }
        private Channel<long> RobotSensor { get; set; }
        private ISet<Coordinate> Painted { get; set; }

        public Day11(string filename)
        {
            Machine = new IntCodeMachine(
                File.ReadLines(filename).
                SelectMany(x => x.Split(',')).
                Select(Int64.Parse).
                Select((x, i) => new { Key = (long) i, Value = x }).
                ToDictionary(x => x.Key, x => x.Value)
            );

            RobotPosition = new Coordinate(0, 0);
            RobotDirection = Complex.ImaginaryOne;
            RobotSensor = Channel.CreateUnbounded<long>();
            Painted = new HashSet<Coordinate>();
        }

        private void RobotPaint(Colour paint)
        {
            Painted.Add(RobotPosition);
            Hull[RobotPosition] = paint;
        }

        private void RobotMove(Complex direction)
        {
            RobotDirection *= direction;

            if (RobotDirection == new Complex(0, 1))
                RobotPosition = new Coordinate(RobotPosition.X, RobotPosition.Y - 1);
            else if (RobotDirection == new Complex(-1, 0))
                RobotPosition = new Coordinate(RobotPosition.X - 1, RobotPosition.Y);
            else if (RobotDirection == new Complex(0, -1))
                RobotPosition = new Coordinate(RobotPosition.X, RobotPosition.Y + 1);
            else if (RobotDirection == new Complex(1, 0))
                RobotPosition = new Coordinate(RobotPosition.X + 1, RobotPosition.Y);
            
            RobotSensor.Writer.WriteAsync(Hull.ContainsKey(RobotPosition) ? (long) Hull[RobotPosition] : (long) Colour.Black);
        }

        public int Solution1()
        {
            var paintNext = true;
            Action<long> output = i => {
                if (paintNext)
                    RobotPaint(i == 0 ? Colour.Black : Colour.White);
                else
                    RobotMove(i == 0 ? new Complex(0, 1) : new Complex(0, -1));
                
                paintNext = !paintNext;
            };

            RobotSensor.Writer.WriteAsync((long) Colour.Black);
            Machine.ExecuteProgram(RobotSensor, output);
            
            return Painted.Count;
        }

        public int Solution2()
        {
            return 0;
        }
    }
}
