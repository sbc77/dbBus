namespace dbBus.Core
{
    using System;
    using System.Threading.Tasks;

    public interface IBus
    {
        Task<IMessage> Publish(IMessage message);

        void Start();

        void Stop();
    }
}