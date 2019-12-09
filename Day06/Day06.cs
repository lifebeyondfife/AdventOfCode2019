using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Day06
    {
        private IDictionary<string, Tree> TreeLookup { get; set; }
        private Tree Tree { get; set; }

        public Day06(string filename)
            : this(File.ReadLines(filename))
        {
        }

        public Day06(IEnumerable<string> orbits)
        {
            TreeLookup = new Dictionary<string, Tree>();

            foreach (var orbit in orbits)
            {
                var satellites = orbit.Split(')');

                if (!TreeLookup.ContainsKey(satellites[0]))
                    TreeLookup[satellites[0]] = new Tree();

                if (!TreeLookup.ContainsKey(satellites[1]))
                    TreeLookup[satellites[1]] = new Tree();

                TreeLookup[satellites[0]].AddChild(TreeLookup[satellites[1]]);
            }

            Tree = TreeLookup["COM"];
        }

        public int Solution1()
        {
            TreeLookup["COM"].PopulateDepths(0);
            return Tree.OrbitTotal();
        }

        public int Solution2()
        {
            TreeLookup["COM"].PopulateDepths(0);

            var earliestParent = Tree.FindEarliestParent(TreeLookup["YOU"], TreeLookup["SAN"]);
            return (TreeLookup["YOU"].Depth - earliestParent.Depth - 1)
                + (TreeLookup["SAN"].Depth - earliestParent.Depth - 1);
        }
    }
}
