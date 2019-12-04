using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Decider.Csp.BaseTypes;
using Decider.Csp.Integer;

namespace AdventOfCode2019.Solutions
{
    public class Day04
    {
        private int LowerBound { get; set; }
        private int UpperBound { get; set; }
        
        public Day04(string input)
        {
            var boundStrings = input.Split('-');
            LowerBound = Int32.Parse(boundStrings[0]);
            UpperBound = Int32.Parse(boundStrings[1]);
        }

        public int Solution1()
        {
            var a = new VariableInteger("a", 0, 9);
            var b = new VariableInteger("b", 0, 9);
            var c = new VariableInteger("c", 0, 9);
            var d = new VariableInteger("d", 0, 9);
            var e = new VariableInteger("e", 0, 9);
            var f = new VariableInteger("f", 0, 9);

            var constraints = new List<IConstraint>();
            constraints.Add(new ConstraintInteger(a <= b));
            constraints.Add(new ConstraintInteger(b <= c));
            constraints.Add(new ConstraintInteger(c <= d));
            constraints.Add(new ConstraintInteger(d <= e));
            constraints.Add(new ConstraintInteger(e <= f));
            constraints.Add(new ConstraintInteger((a == b) | (b == c) | (c == d) | (d == e) | (e == f)));
            constraints.Add(new ConstraintInteger(a * 100000 + b * 10000 + c * 1000 + d * 100 + e * 10 + f >= LowerBound));
            constraints.Add(new ConstraintInteger(a * 100000 + b * 10000 + c * 1000 + d * 100 + e * 10 + f <= UpperBound));

            IState<int> state = new StateInteger(new [] {a, b, c, d, e, f}, constraints);
			state.StartSearch(out StateOperationResult searchResult, out IList<IDictionary<string, IVariable<int>>> solutions);

            return solutions.Count;
        }

        public int Solution2()
        {
            var a = new VariableInteger("a", 0, 9);
            var b = new VariableInteger("b", 0, 9);
            var c = new VariableInteger("c", 0, 9);
            var d = new VariableInteger("d", 0, 9);
            var e = new VariableInteger("e", 0, 9);
            var f = new VariableInteger("f", 0, 9);

            var constraints = new List<IConstraint>();
            constraints.Add(new ConstraintInteger(a <= b));
            constraints.Add(new ConstraintInteger(b <= c));
            constraints.Add(new ConstraintInteger(c <= d));
            constraints.Add(new ConstraintInteger(d <= e));
            constraints.Add(new ConstraintInteger(e <= f));
            constraints.Add(new ConstraintInteger(
                ((a == b) & (b != c)) |
                ((b == c) & (a != b) & (c != d)) |
                ((c == d) & (b != c) & (d != e)) |
                ((d == e) & (c != d) & (e != f)) |
                ((e == f) & (d != e))
            ));
            constraints.Add(new ConstraintInteger(a * 100000 + b * 10000 + c * 1000 + d * 100 + e * 10 + f >= LowerBound));
            constraints.Add(new ConstraintInteger(a * 100000 + b * 10000 + c * 1000 + d * 100 + e * 10 + f <= UpperBound));

            IState<int> state = new StateInteger(new [] {a, b, c, d, e, f}, constraints);
			state.StartSearch(out StateOperationResult searchResult, out IList<IDictionary<string, IVariable<int>>> solutions);

            return solutions.Count;
        }
    }
}
