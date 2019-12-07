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

        public ChainedMachine(IDictionary<int, int> intCode)
            : base(intCode)
        {
        }

        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition())
                    await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout))) 
                throw new TimeoutException();
        }

       protected override void Input(Queue<int> inputs, int location)
        {
            var hasInput = WaitUntil(() => inputs.Count > 0);

            // replace with your own implementaion which uses the injected queue
            base.Input(inputs, location);
        }

        protected override void Output(OpCode opCode, Queue<int> outputs, int location)
        {
            // replace with your own implementaion which uses the injected queue
            base.Output(opCode, outputs, location);
        }

        public static int ExecuteCycledProgram(IntCodeMachine machine, IList<IList<int>> inputs)
        {
            // create input and output queues

            // create copies of 'machine', hand out Id values appropriately
            
            // when inputs / outputs are ready, put them on the right queue
            return 0;
        }
    }
}