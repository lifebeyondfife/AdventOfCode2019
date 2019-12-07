using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019.Solutions
{
    public class IntCodeMachine
    {

        private IDictionary<int, int> OriginalIntCode { get; set; }
        private IDictionary<int, int> IntCode { get; set; }
        private IDictionary<Instruction, Action<Action<int>, Stack<int>, IList<int>, OpCode, int[]>> Commands { get; set; }
        
        public IntCodeMachine(IDictionary<int, int> intCode)
        {
            OriginalIntCode = intCode;
            Commands =
                new Dictionary<Instruction, Action<Action<int>, Stack<int>, IList<int>, OpCode, int[]>>
                {
                    { Instruction.Addition, (s, i, o, op, p) => Addition(op, p[0], p[1], p[2]) },
                    { Instruction.Multiplication, (s, i, o, op, p) => Multiplication(op, p[0], p[1], p[2]) },
                    { Instruction.Input, (s, i, o, op, p) => Input(i, p[0]) },
                    { Instruction.Output, (s, i, o, op, p) => Output(op, o, p[0]) },
                    { Instruction.JumpIfTrue, (s, i, o, op, p) => JumpIfTrue(s, op, p[0], p[1]) },
                    { Instruction.JumpIfFalse, (s, i, o, op, p) => JumpIfFalse(s, op, p[0], p[1]) },
                    { Instruction.LessThan, (s, i, o, op, p) => LessThan(op, p[0], p[1], p[2]) },
                    { Instruction.Equals, (s, i, o, op, p) => Equals(op, p[0], p[1], p[2]) }                    
                };
        }

        private void Addition(OpCode opCode, int operand1, int operand2, int location)
        {
            IntCode[location] = (opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) +
                (opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]);
        }

        private void Multiplication(OpCode opCode, int operand1, int operand2, int location)
        {
            IntCode[location] = (opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) *
                (opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]);
        }

        private void Input(Stack<int> inputs, int location)
        {
            IntCode[location] = inputs.Pop();
        }

        private void Output(OpCode opCode, IList<int> outputs, int location)
        {
            outputs.Add(opCode.Modes[0] == Mode.Immediate ? location : IntCode[location]);
        }

        private void JumpIfTrue(Action<int> setPointer, OpCode opCode, int operand1, int operand2)
        {
            if ((opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) != 0)
                setPointer(opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]);
        }

        private void JumpIfFalse(Action<int> setPointer, OpCode opCode, int operand1, int operand2)
        {
            if ((opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) == 0)
                setPointer(opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]);
        }

        private void LessThan(OpCode opCode, int operand1, int operand2, int location)
        {
            if ((opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) <
                (opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]))
                IntCode[location] = 1;
            else
                IntCode[location] = 0;
        }

        private void Equals(OpCode opCode, int operand1, int operand2, int location)
        {
            if ((opCode.Modes[0] == Mode.Immediate ? operand1 : IntCode[operand1]) ==
                (opCode.Modes[1] == Mode.Immediate ? operand2 : IntCode[operand2]))
                IntCode[location] = 1;
            else
                IntCode[location] = 0;
        }

        private void ResetState()
        {
            var intCode = new KeyValuePair<int, int>[OriginalIntCode.Count];
            OriginalIntCode.CopyTo(intCode, 0);
            IntCode = intCode.ToDictionary(x => x.Key, x => x.Value);
        }

        public void ExecuteProgram(Stack<int> inputs, out IList<int> outputs)
        {
            ResetState();

            outputs = new List<int>();
            var i = 0;
            while (i < IntCode.Keys.Count)
            {
                int? pointerReset = null;
                Action<int> setPointer = x => pointerReset = x;
                var opCode = (OpCode) IntCode[i];

                if (opCode.Instruction == Instruction.End)
                    return;
                
                Commands[opCode.Instruction](
                    setPointer, inputs, outputs, opCode,
                    Enumerable.
                        Range(i + 1, opCode.Skip).
                        Select(x => IntCode[x]).
                        ToArray()
                );

                if (pointerReset.HasValue)
                    i = pointerReset.Value;
                else
                    i += opCode.Skip;
            }
        }
    }
}
