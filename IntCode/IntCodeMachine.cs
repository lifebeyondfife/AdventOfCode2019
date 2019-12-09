using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace AdventOfCode2019.Library
{
    public class IntCodeMachine
    {
        public IDictionary<long, long> OriginalIntCode { get; private set; }
        protected int Id { get; set; }
        protected long Base { get; set; }
        protected IDictionary<long, long> IntCode { get; set; }
        protected IDictionary<Instruction, Action<Action<long>, Channel<long>, Action<long>, OpCode, long[]>> Commands { get; set; }
        
        public IntCodeMachine()
        {
        }

        public IntCodeMachine(IDictionary<long, long> intCode)
        {
            OriginalIntCode = intCode;
            Commands =
                new Dictionary<Instruction, Action<Action<long>, Channel<long>, Action<long>, OpCode, long[]>>
                {
                    { Instruction.Addition, (s, i, o, op, p) => Addition(op, p[0], p[1], p[2]) },
                    { Instruction.Multiplication, (s, i, o, op, p) => Multiplication(op, p[0], p[1], p[2]) },
                    { Instruction.Offset, (s, i, o, op, p) => Offset(op, p[0]) },
                    { Instruction.Input, (s, i, o, op, p) => Input(i, op, p[0]) },
                    { Instruction.Output, (s, i, o, op, p) => Output(op, o, p[0]) },
                    { Instruction.JumpIfTrue, (s, i, o, op, p) => JumpIfTrue(s, op, p[0], p[1]) },
                    { Instruction.JumpIfFalse, (s, i, o, op, p) => JumpIfFalse(s, op, p[0], p[1]) },
                    { Instruction.LessThan, (s, i, o, op, p) => LessThan(op, p[0], p[1], p[2]) },
                    { Instruction.Equals, (s, i, o, op, p) => Equals(op, p[0], p[1], p[2]) }                    
                };
        }

        private long InterpretedValue(long location, Mode mode)
        {
            switch (mode)
            {
                case Mode.Position: return IntCode.ContainsKey(location) ? IntCode[location] : 0;
                case Mode.Immediate: return location;
                case Mode.Relative: return IntCode.ContainsKey(location + Base) ? IntCode[location + Base] : 0;
                default: throw new ApplicationException("Unknown mode type");
            }
        }

        private long LiteralValue(long location, Mode mode)
        {
            switch (mode)
            {
                case Mode.Position: throw new ApplicationException("Cannot write to value");
                case Mode.Immediate: return location;
                case Mode.Relative: return location + Base;
                default: throw new ApplicationException("Unknown mode type");
            }
        }

        private void Addition(OpCode opCode, long operand1, long operand2, long location)
        {
            IntCode[LiteralValue(location, opCode.Modes[2])] = InterpretedValue(operand1, opCode.Modes[0])
                + InterpretedValue(operand2, opCode.Modes[1]);
        }

        private void Multiplication(OpCode opCode, long operand1, long operand2, long location)
        {
            IntCode[LiteralValue(location, opCode.Modes[2])] = InterpretedValue(operand1, opCode.Modes[0])
                * InterpretedValue(operand2, opCode.Modes[1]);
        }

        private void Offset(OpCode opCode, long location)
        {
            Base += InterpretedValue(location, opCode.Modes[0]);
        }

        private void Input(Channel<long> inputs, OpCode opCode, long location)
        {
            var inputTask = inputs.Reader.ReadAsync().AsTask();
            inputTask.Wait();
            IntCode[LiteralValue(location, opCode.Modes[0])] = inputTask.Result;
        }

        private void Output(OpCode opCode, Action<long> output, long location)
        {
            output(InterpretedValue(location, opCode.Modes[0]));
        }

        private void JumpIfTrue(Action<long> setPointer, OpCode opCode, long operand1, long operand2)
        {
            if (InterpretedValue(operand1, opCode.Modes[0]) != 0)
                setPointer(InterpretedValue(operand2, opCode.Modes[1]));
        }

        private void JumpIfFalse(Action<long> setPointer, OpCode opCode, long operand1, long operand2)
        {
            if (InterpretedValue(operand1, opCode.Modes[0]) == 0)
                setPointer(InterpretedValue(operand2, opCode.Modes[1]));
        }

        private void LessThan(OpCode opCode, long operand1, long operand2, long location)
        {
            if (InterpretedValue(operand1, opCode.Modes[0]) < InterpretedValue(operand2, opCode.Modes[1]))
                IntCode[LiteralValue(location, opCode.Modes[2])] = 1;
            else
                IntCode[LiteralValue(location, opCode.Modes[2])] = 0;
        }

        private void Equals(OpCode opCode, long operand1, long operand2, long location)
        {
            if (InterpretedValue(operand1, opCode.Modes[0]) == InterpretedValue(operand2, opCode.Modes[1]))
                IntCode[LiteralValue(location, opCode.Modes[2])] = 1;
            else
                IntCode[LiteralValue(location, opCode.Modes[2])] = 0;
        }

        public void ExecuteProgram(Channel<long> inputs, Action<long> output)
        {
            IntCode = OriginalIntCode.ToDictionary(x => x.Key, x => x.Value);
            Base = 0;

            var i = 0L;
            while (i < IntCode.Keys.Count)
            {
                long? pointerReset = null;
                Action<long> setPointer = x => pointerReset = x;
                var opCode = (OpCode) IntCode[i];

                if (opCode.Instruction == Instruction.End)
                    return;
                
                Commands[opCode.Instruction](
                    setPointer, inputs, output, opCode,
                    Enumerable.
                        Range(1, opCode.Skip).
                        Select(x => IntCode.ContainsKey((long) x + i) ? IntCode[(long) x + i] : 0).
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
