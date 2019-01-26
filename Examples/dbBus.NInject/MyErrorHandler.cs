using System;
using System.Threading.Tasks;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    internal class MyErrorHandler : IErrorHandler
    {

        public async Task<bool> OnError(object sender, Exception e, IMessage message, int retryNo)
        {
            Console.WriteLine($"Error reading message #{message.InternalId}, retry #{retryNo}");
            return await Task.FromResult(true);
        }
    }
}