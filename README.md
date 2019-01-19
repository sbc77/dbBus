# dbBus

Simple async messaging framework with database as transport layer.

DotNet Core 2.2  
DotNet Standard 2.0


Currently supported:
* DI
  - Ninject
* Databases: 
  - Mssql

## Usage
``` C#
var bus = Bus.Configure()
            .UseNinject(kernel)
            .UseMssql("Server=<addr>;Database=<database>;User Id=<user>;Password=<password>;MultipleActiveResultSets=true;"))
            .RegisterHandler<MyMessageHandler>()
            .Build();
```
In case of Mssql, is important to set **MultipleActiveResultSets** in connection string.

Create message class:
``` C#
public class MyMessage : IMessage
{
    public long InternalId { get; set; } // derived

    // your properties here
}
``` 
Create handler class:
``` C#
public class MyMessageHandler : IHandle<MyMessage>
{
    public async Task Handle(MyMessage message)
    {
        // proces message here
    }
}
```

### Sending messages
You can just simply use your bus instance:
``` C#
await bus.Publish(new MyMessage());
```
... or you can use bus via constructor dependency injection.

``` C#
public class MyAwesomeClass
{
    private readonly IBus bus;

    public MyAwesomeClass(IBus bus)
    {
        this.bus = bus;
    }

    public async Task MyMethod()
    {
        await this.bus.Publish(new MyMessage());
    }
}
```