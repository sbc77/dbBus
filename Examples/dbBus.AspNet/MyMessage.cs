using dbBus.Core;

namespace dbBus.AspNet
{
    public class MyMessage : IMessage
    {
        public long InternalId { get; set; }
    }
}
