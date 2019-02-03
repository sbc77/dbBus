using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.AspNet
{
    public class MyHandler : IHandle<MyMessage>
    {
        public async Task Handle(MyMessage message)
        {
            //throw new Exception("Fail!");
            await Task.CompletedTask;
        }
    }
}
