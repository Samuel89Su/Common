using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.Queue
{
    public interface IQueueConsumer<T> : IQueueConsumer<T, IQueue<T>> where T : IEventData
    { }

    public interface IQueueConsumer<T, TQueue, TEventHandler> : IQueueConsumer<T, TQueue> where TQueue : IQueue<T> where TEventHandler : IEventHandler<T> where T : IEventData
    { }

    public interface IQueueConsumer<T, TQueue> where TQueue : IQueue<T>
    {
        Task StartConsume();
    }
}
