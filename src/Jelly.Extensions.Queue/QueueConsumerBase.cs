using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Jelly.Threading.Channels.Queue
{
    public class QueueConsumerBase<T> : QueueConsumerBase<T, IQueue<T>>, IQueueConsumer<T>
    {
        public QueueConsumerBase(IQueue<T> queue) : base(queue)
        {
        }
    }

    public class QueueConsumerBase<T, TQueue> : IQueueConsumer<T, TQueue> where TQueue : IQueue<T>
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

        protected virtual Task Handle(T data)
        {
            return Task.CompletedTask;
        }

        private IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return _queue.ReadAllAsync(cancellationToken);
        }
    }
}
