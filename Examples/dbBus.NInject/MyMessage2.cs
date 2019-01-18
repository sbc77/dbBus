using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    public class MyMessage2 : IMessage
    {
        public long InternalId { get; set; }

        public bool IsAwesome { get; set; }
    }
}
