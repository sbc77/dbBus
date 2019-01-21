using System;
using dbBus.Core;
using dbBus.Extensions.NInject;

using Ninject;
using Xunit;

namespace dbBus.UnitTesting
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var kernel = new StandardKernel();

            var bus = Bus.Configure()
              .UseNinject(kernel)
              .UseSqlite(":memory:")
              .RegisterHandler<MyMessageHandler1>()
              .RegisterHandler<MyMessageHandler2>()
              .RegisterHandler<MyMessageHandler3>()
              .Build();

            bus.Start();
        }
    }
}
