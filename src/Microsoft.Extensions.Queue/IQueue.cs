using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.Queue
{
    public interface IQueue<T>
    {
        IQueueProducer<T> QueueProducer { get; }
        IQueueConsumer<T> QueueConsumer { get; }
    }
}
