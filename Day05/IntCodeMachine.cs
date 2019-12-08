using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace AdventOfCode2019.Solutions
{
    public class IntCodeMachine
    {
        public IDictionary<int, int> OriginalIntCode { get; private set; }
        protected int Id { get; set; }
        protected IDictionary<int, int> IntCode { get; set; }
        protected IDictionary<Instruction, Action<Action<int>, Channel<int>, Action<int>, OpCode, int[]>> Commands { get; set; }
        
        public IntCodeMachine()
        {
        }

        public IntCodeMachine(IDictionary<int, int> intCode)
        {
            OriginalIntCode = intCode;
            Commands =
                new Dictionary<Instruction, Action<Action<int>, Channel<int>, Action<int>, OpCode, int[]>>
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

        private void Input(Channel<int> inputs, int location)
        {
            var inputTask = inputs.Reader.ReadAsync().AsTask();
            inputTask.Wait();
            IntCode[location] = inputTask.Result;
        }

        private void Output(OpCode opCode, Action<int> output, int location)
        {
            output(opCode.Modes[0] == Mode.Immediate ? location : IntCode[location]);
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

        public void ExecuteProgram(Channel<int> inputs, Action<int> output)
        {
            IntCode = OriginalIntCode.ToDictionary(x => x.Key, x => x.Value);

            var i = 0;
            while (i < IntCode.Keys.Count)
            {
                int? pointerReset = null;
                Action<int> setPointer = x => pointerReset = x;
                var opCode = (OpCode) IntCode[i];

                if (opCode.Instruction == Instruction.End)
                    return;
                
                Commands[opCode.Instruction](
                    setPointer, inputs, output, opCode,
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
