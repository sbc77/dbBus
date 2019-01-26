using Ninject;
using dbBus.Extensions.NInject;
using dbBus.Extensions.Mssql;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using dbBus.Core;

namespace dbBus.NInjectMsSql
{
    class Program
    {
        public const double ErrorRate = 0.1;
        public const int ExecutionTime = 100; // ms

        static async Task Main()
        {
            var kernel = new StandardKernel();

            kernel.Bind(typeof(ILogger<>)).To(typeof(ConsoleLogger<>));

            var log = kernel.Get<ILogger<Program>>();

            var bus = Bus.Configure()
                .UseNinject(kernel)
                .UseMssql(Environment.GetEnvironmentVariable("dbBus_CS")) // connection string
                .RegisterHandler<MyMessageHandler1>()
                .RegisterHandler<MyMessageHandler2>()
                .RegisterHandler<MyMessageHandler3>()
                .RegisterErrorHandler<MyErrorHandler>()
                .Build();


            bus.Start();

            while (true)
            {
                await bus.Publish(new MyMessage1 { Content = "Works?", Created = DateTime.Now });
                await Task.Delay(1000);
            }
        }
    }
}
