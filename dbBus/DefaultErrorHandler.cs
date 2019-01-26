namespace dbBus
{
    using System;
    using System.Threading.Tasks;

    using dbBus.Core;
    using Microsoft.Extensions.Logging;

    public class DefaultErrorHandler : IErrorHandler
    {
        private readonly ILogger<DefaultErrorHandler> log;

        public DefaultErrorHandler(ILogger<DefaultErrorHandler> log)
        {
            this.log = log;
        }
        public async Task<bool> OnError(object sender, Exception e, IMessage message, int retryNo)
        {
            this.log.LogError(e, $"Error handling message #{message.InternalId}, retry #{retryNo}");
            return await Task.FromResult(true);
        }
    }
}