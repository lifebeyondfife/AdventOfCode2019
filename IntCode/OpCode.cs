using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Library
{
    public enum Mode
    {
        Position = 0,
        Immediate = 1,
        Relative = 2
    }

    public enum Instruction
    {
        Addition = 1,
        Multiplication = 2,
        Input = 3,
        Output = 4,
        JumpIfTrue = 5,
        JumpIfFalse = 6,
        LessThan = 7,
        Equals = 8,
        Offset = 9,
        End = 99
    }

    public struct OpCode
    {
        public IList<Mode> Modes { get; set; }
        public Instruction Instruction { get; set; }
        public int Skip {
            get {
                switch (Instruction)
                {
                    case Instruction.Input:
                    case Instruction.Output:
                    case Instruction.Offset: return 2;
                    case Instruction.JumpIfTrue:
                    case Instruction.JumpIfFalse: return 3;
                    case Instruction.Addition:
                    case Instruction.Multiplication:
                    case Instruction.LessThan:
                    case Instruction.Equals: return 4;
                    default: throw new ApplicationException("Unexpected Instruction type");
                }
            }
        }

        private static IEnumerable<int> GetDigits(int integer)
        {
            while (integer > 0)
            {
                yield return integer % 10;
                integer /= 10;
            }
        }

        public static implicit operator OpCode(int opCode)
        {
            var opCodeStruct = new OpCode();

            if (opCode == 99)
            {
                opCodeStruct.Instruction = Instruction.End;
                return opCodeStruct;
            }

            var digits = GetDigits(opCode).ToList();
            opCodeStruct.Instruction = (Instruction) digits[0];
            opCodeStruct.Modes = Enumerable.Range(2, 3).
                Select(i => digits.Count > i ? (Mode) digits[i] : Mode.Position).
                ToList();

            return opCodeStruct;
        }
    }
}
