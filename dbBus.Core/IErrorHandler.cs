namespace dbBus.Core
{
    using System;
    using System.Threading.Tasks;

    public interface IErrorHandler
    {
        Task<bool> OnError(object sender, Exception e, IMessage message, int retryNo);
    }
}