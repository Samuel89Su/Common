using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.Queue
{
    public class QueueConsumerBase<T> : QueueConsumerBase<T, IQueue<T>, IEventHandler<T>>, IQueueConsumer<T> where T : IEventData
    {
        public QueueConsumerBase(IQueue<T> queue, IEventHandler<T> eventHandler) : base(queue, eventHandler)
        {
        }
    }

    public class QueueConsumerBase<T, TQueue, TEventHandler> : QueueConsumerBase<T, TQueue>, IQueueConsumer<T, TQueue, TEventHandler> where TQueue : IQueue<T> where TEventHandler : IEventHandler<T> where T : IEventData
    {
        private readonly TEventHandler _eventHandler;
        public QueueConsumerBase(IQueue<T> queue, TEventHandler eventHandler)
            : base(queue)
        {
            _eventHandler = eventHandler;
        }

        protected override async Task Handle(T data)
        {
            await _eventHandler.HandleEventAsync(data);
        }
    }

    public abstract class QueueConsumerBase<T, TQueue> : IQueueConsumer<T, TQueue> where TQueue : IQueue<T>
    {
        private readonly IQueue<T> _queue;
        public QueueConsumerBase(IQueue<T> queue)
        {
            _queue = queue;
        }

        public async Task StartConsume()
        {
            try
            {
                await foreach (var data in ReadAllAsync(default))
                {
                    await Handle(data);
                }
            }
            catch (ChannelClosedException ex)
            {
            }
        }

        protected abstract Task Handle(T data);

        protected IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return _queue.ReadAllAsync(cancellationToken);
        }
    }
}
