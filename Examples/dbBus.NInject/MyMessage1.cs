using System;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    public class MyMessage1 : IMessage
    {
        public long InternalId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
    }
}
