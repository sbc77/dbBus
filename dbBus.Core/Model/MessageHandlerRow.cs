namespace dbBus.Core.Model
{
    using System;
    using TheOne.OrmLite.Core.DataAnnotations;

    [OrmLiteAlias("MessageHandlers")]
    public class MessageHandlerRow
    {
        [OrmLiteAutoIncrement]
        public long Id { get; set; }

        [OrmLiteStringLength(256)]
        public string HandlerId { get; set; }

        public DateTime HandleDate { get; set; }

        public bool Handled { get; set; }

        [OrmLiteStringLength(Int32.MaxValue)]
        public string Error { get; set; }

        [OrmLiteReferences(typeof(MessageRow))]
        public long MessageId { get; set; }

        public int RetryNo { get; set; }
    }
}