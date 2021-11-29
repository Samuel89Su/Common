using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jelly.Threading.Channels.Queue
{
    public interface IQueueProducer<T>
    {
        bool TryComplete(Exception error = null);
        bool TryWrite(T item);
        ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default);
        ValueTask WriteAsync(T item, CancellationToken cancellationToken = default);
        void Complete(Exception error = null);
    }
}
