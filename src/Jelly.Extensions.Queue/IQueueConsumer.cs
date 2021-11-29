using System.Threading.Tasks;

namespace Jelly.Threading.Channels.Queue
{
    public interface IQueueConsumer<T> : IQueueConsumer<T, IQueue<T>>
    { }

    public interface IQueueConsumer<T, TQueue> where TQueue : IQueue<T>
    {
        Task StartConsume();
    }
}
