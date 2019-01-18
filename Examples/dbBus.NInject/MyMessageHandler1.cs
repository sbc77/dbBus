using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    public class MyMessageHandler1 : IHandle<MyMessage1>
    {
        public async Task Handle(MyMessage1 message)
        {
            await Task.Delay(Program.ExecutionTime);

            var rnd = new Random();

            if (rnd.NextDouble() < Program.ErrorRate)
            {
                throw new Exception("Sometimes it has to fail");
            }
        }
    }
}
