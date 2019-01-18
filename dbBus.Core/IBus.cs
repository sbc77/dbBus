using System;
using System.Threading.Tasks;

namespace dbBus.Core
{
    public interface IBus
    {
        Task<IMessage> Publish(IMessage message);
        void Start();
        void Stop();
    }
}