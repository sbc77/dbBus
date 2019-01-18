using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    public class MyMessageHandler3 : IHandle<MyMessage2>
    {
        public async Task Handle(MyMessage2 message)
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
