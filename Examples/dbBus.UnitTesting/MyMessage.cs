using dbBus.Core;

namespace dbBus.UnitTesting
{
    public class MyMessage : IMessage
    {
        public long InternalId { get; set; }
    }
}
