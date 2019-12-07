using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2019.Solutions
{
    public class ChainedMachine : IntCodeMachine
    {
        private int Id { get; set; }
        private static IList<Queue<int>> Inputs { get; set; }
        private static IList<Queue<int>> Outputs { get; set; }
        private static IList<ChainedMachine> Machines { get; set; }

        public ChainedMachine(IntCodeMachine machine)
            : base(machine.OriginalIntCode.ToDictionary(x => x.Key, x => x.Value))
        {
        }

        private ChainedMachine(IntCodeMachine machine, int id)
            : this(machine)
        {
            Id = id;
        }

        protected override void Input(Queue<int> inputs, int location)
        {
            while (Inputs[Id].Count == 0)
            {
                Task.Delay(100);
            }

            if (Inputs[Id].Count == 0)
            {
                Console.WriteLine($"Id: {Id}");
            }

            IntCode[location] = Inputs[Id].Dequeue();
        }

        protected override void Output(OpCode opCode, Queue<int> outputs, int location)
        {
            Inputs[(Id + 1) % 5].Enqueue(opCode.Modes[0] == Mode.Immediate ? location : IntCode[location]);
        }

        public static async Task<int> ExecuteCycledProgram(IntCodeMachine machine, IList<IList<int>> inputs)
        {
            Inputs = inputs.Select(x => new Queue<int>(x)).ToList();
            Machines = Inputs.Select((_, i) => new ChainedMachine(machine, i)).ToList();

            foreach (var chainedMachine in Machines)
            {
                await Task.Run(() => chainedMachine.ExecuteProgram(null, out IList<int> outputs));
            }

            while (Machines.Last().PreviousOpCode.Instruction != Instruction.End)
            {
                Task.Delay(100);
            }

            return Inputs.First().First();
        }
    }
}