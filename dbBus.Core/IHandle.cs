namespace dbBus.Core
{
    using System.Threading.Tasks;

    public interface IHandle<in T> where T : IMessage
    {
        Task Handle(T message);
    }
}