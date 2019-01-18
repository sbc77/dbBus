using System.Threading;
using System.Threading.Tasks;

namespace dbBus.Core
{
    public interface IHandle<T> where T : IMessage
    {
        Task Handle(T message);
    }
}