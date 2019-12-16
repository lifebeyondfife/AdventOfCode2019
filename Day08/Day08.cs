using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.Solutions
{
    public class Day08
    {
        private IList<int[,]> Layers { get; set; }

        public Day08(string filename)
        {
            Layers = new List<int[,]>();
            var imageString = File.ReadLines(filename).First();

            var index = 0;
            while (index < imageString.Length)
            {
                Layers.Add(new int[25, 6]);
                for (var j=0; j < 6; ++j)
                    for (var i=0; i < 25; ++i)
                        Layers.Last()[i,j] = Int32.Parse(imageString[index++].ToString());
            }
        }

        private int InstancesOf(int[,] layer, int value)
        {
            var count = 0;
            for (var j=0; j <= layer.GetUpperBound(1); ++j)
                for (var i=0; i <= layer.GetUpperBound(0); ++i)
                    if (layer[i, j] == value)
                        ++count;

            return count;
        }

        public int Solution1()
        {
            var fewestZeros = Layers.
                Select((l, i) => new { Layer = l, Index = i }).
                OrderBy(li => InstancesOf(li.Layer, 0)).
                First().
                Layer;

            return InstancesOf(fewestZeros, 1) * InstancesOf(fewestZeros, 2);
        }

        private string Pixel(int x, int y)
        {
            var pixelCode = Layers.
                Select(l => l[x, y]).
                SkipWhile(x => x == 2).
                First();

            return pixelCode == 0 ? " " : "█";
        }

        public string Solution2()
        {
            var image = new StringBuilder("\n");

            for (var j=0; j <= Layers.First().GetUpperBound(1); ++j)
            {
                for (var i=0; i <= Layers.First().GetUpperBound(0); ++i)
                {
                    image.Append(Pixel(i, j));
                }
                image.AppendLine();
            }

            return image.ToString();
        }
    }
}
