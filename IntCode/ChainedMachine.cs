using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AdventOfCode2019.Library
{
    public class ChainedMachine : IntCodeMachine
    {
        private static IList<Channel<long>> Inputs { get; set; }
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

        public static Channel<long> GetPopulatedChannel(IList<long> inputs)
        {
            var channel = Channel.CreateUnbounded<long>();

            foreach (var input in inputs)
                channel.Writer.WriteAsync(input);

            return channel;
        }

        public static long ExecuteCycledProgram(IntCodeMachine machine, IList<IList<long>> inputs)
        {
            Inputs = inputs.Select(GetPopulatedChannel).ToList();
            Machines = Inputs.Select((_, i) => new ChainedMachine(machine, i)).ToList();

            Task.WaitAll(Enumerable.Range(0, Machines.Count).
                Select(i => Task.Run(() => {
                    Action<long> outputLambda = x => Inputs[(i + 1) % Inputs.Count].Writer.WriteAsync(x);
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