using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.AspNet
{
    public class MyErrorHandler : IErrorHandler
    {
        public async Task<bool> OnError(object sender, Exception e, IMessage message, int retryNo)
        {
            Console.WriteLine($"Error message #{message.InternalId}");
            return await Task.FromResult(false);
        }
    }
}
