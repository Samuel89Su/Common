using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.Threading.Channels.Queue
{
    public interface IQueueConsumer<T> : IQueueConsumer<T, IQueue<T>>
    { }

    public interface IQueueConsumer<T, TQueue> where TQueue : IQueue<T>
    {
        IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
