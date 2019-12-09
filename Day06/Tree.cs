using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class Tree
    {
        public int Depth { get; private set; }
        private IList<Tree> Children { get; set; }
        private Tree Parent { get; set; }

        public Tree()
        {
            Children = new List<Tree>();
        }

        public Tree AddChild(Tree child)
        {
            Children.Add(child);
            child.Parent = this;
            return child;
        }

        public void PopulateDepths(int depth)
        {
            Depth = depth;

            foreach (var child in Children)
                child.PopulateDepths(depth + 1);
        }

        private int SumDepths()
        {
            return Depth + Children.Sum(c => c.SumDepths());
        }

        public int OrbitTotal()
        {
            return SumDepths();
        }

        public static Tree FindEarliestParent(Tree tree1, Tree tree2)
        {
            var depth1 = tree1.Depth;
            var depth2 = tree2.Depth;

            while (tree1.Depth > tree2.Depth)
                tree1 = tree1.Parent;

            while (tree2.Depth > tree1.Depth)
                tree2 = tree2.Parent;

            while (tree1 != tree2)
            {
                tree1 = tree1.Parent;
                tree2 = tree2.Parent;
            }

            return tree1;
        }
    }
}