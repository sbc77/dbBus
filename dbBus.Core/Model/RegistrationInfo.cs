namespace dbBus.Core.Model
{
    using System;

    public class RegistrationInfo
    {
        public string MessageTypeName { get; set; }

        public string HandlerTypeName { get; set; }

        public Type MessageType { get; set; }

        public Type HandlerType { get; set; }
    }
}