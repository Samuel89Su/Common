using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.Queue
{
    public interface IQueueConsumer<T>
    {
        IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
        ValueTask<T> ReadAsync(CancellationToken cancellationToken = default);
        bool TryPeek([MaybeNullWhen(false)] out T item);
        bool TryRead([MaybeNullWhen(false)] out T item);
        ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default);
    }
}
