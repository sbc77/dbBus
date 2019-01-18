using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    public class MyMessageHandler2 : IHandle<MyMessage1>
    {
        private readonly IBus bus;

        public MyMessageHandler2(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(MyMessage1 message)
        {
            await Task.Delay(Program.ExecutionTime);

            var rnd = new Random();

            if (rnd.NextDouble() < Program.ErrorRate)
            {
                throw new Exception("Sometimes it has to fail");
            }

            await this.bus.Publish(new MyMessage2 { IsAwesome = true });
        }
    }
}
