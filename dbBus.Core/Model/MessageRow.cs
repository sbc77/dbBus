namespace dbBus.Core.Model
{
    using System;
    using TheOne.OrmLite.Core.DataAnnotations;

    [OrmLiteAlias("Messages")]
    public class MessageRow
    {
        [OrmLiteAutoIncrement]
        public long Id { get; set; }

        [OrmLiteStringLength(256)]
        public string MessageType { get; set; }

        public DateTime Created { get; set; }

        public DateTime ValidUntil { get; set; }

        [OrmLiteStringLength(256)]
        public string PublishedBy { get; set; }

        [OrmLiteStringLength(int.MaxValue)]
        public string Content { get; set; }


    }
}