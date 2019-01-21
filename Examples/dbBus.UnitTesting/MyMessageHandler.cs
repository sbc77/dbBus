using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.UnitTesting
{
    public class MyMessageHandler : IHandle<MyMessage>
    {
        public static bool Received { get; set; }

        public async Task Handle(MyMessage message)
        {
            Received = true;
            await Task.CompletedTask;
        }
    }
}
