namespace dbBus
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Logging;

    public class DefaultBusLogger<T> : ILogger<T>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}  {logLevel,-11}  {Thread.CurrentThread.ManagedThreadId,3}  {typeof(T).Name,-20} {state}");

            if (exception != null)
            {
                Console.WriteLine(exception);
            }

        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}