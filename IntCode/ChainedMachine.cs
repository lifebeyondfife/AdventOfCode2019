using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode2019.Library
{
    public class ChainedMachine : IntCodeMachine
    {
        private static IList<Channel<int>> Inputs { get; set; }
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

        public static Channel<int> GetPopulatedChannel(IList<int> inputs)
        {
            var channel = Channel.CreateUnbounded<int>();

            foreach (var input in inputs)
                channel.Writer.WriteAsync(input);

            return channel;
        }

        public static int ExecuteCycledProgram(IntCodeMachine machine, IList<IList<int>> inputs)
        {
            Inputs = inputs.Select(GetPopulatedChannel).ToList();
            Machines = Inputs.Select((_, i) => new ChainedMachine(machine, i)).ToList();

            Task.WaitAll(Enumerable.Range(0, Machines.Count).
                Select(i => Task.Run(() => {
                    Action<int> outputLambda = x => Inputs[(i + 1) % Inputs.Count].Writer.WriteAsync(x);
                    Machines[i].ExecuteProgram(Inputs[i], outputLambda);
                })).
                ToArray()
            );

            var answer = Inputs[0].Reader.ReadAsync().AsTask();
            answer.Wait();

            return answer.Result;
        }
    }
}