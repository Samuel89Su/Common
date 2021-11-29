using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Jelly.Threading.Channels.Queue
{
    public interface IQueue<T>
    {
        IQueueProducer<T> QueueProducer { get; }

        IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
