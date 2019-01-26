namespace dbBus.UnitTesting
{
    using System.Threading.Tasks;
    using dbBus.Extensions.NInject;
    using dbBus.Extensions.Sqlite;
    using Ninject;
    using Xunit;

    public class BusTests
    {
        [Fact]
        public async Task HandleMessageTest()
        {
            var kernel = new StandardKernel();

            var bus = Bus.Configure()
              .UseNinject(kernel)
              .UseDefaultConsoleLogger()
              .UseSqlite(":memory:")
              .RegisterHandler<MyMessageHandler>()
              .Build();

            bus.Start();

            MyMessageHandler.Received = false;

            await bus.Publish(new MyMessage());

            // Default pull interval is 1s, 
            // so we expect that after 3s message should be handled
            await Task.Delay(3000);

            Assert.True(MyMessageHandler.Received);

            bus.Stop();
        }
    }
}
