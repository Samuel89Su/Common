using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Threading.Channels.Queue
{
    public class QueueBase<T> : IQueue<T>, IQueueProducer<T>, IQueueConsumer<T>
    {
        private Channel<T> _channel;

        protected ChannelReader<T> ChannelReader { get; private set; }
        protected ChannelWriter<T> ChannelWriter { get; private set; }

        public string Name => typeof(T).Name;

        public IQueueProducer<T> QueueProducer => this;

        public IQueueConsumer<T> QueueConsumer => this;

        public QueueBase(IServiceProvider serviceProvider)
        {
            var queueOptionMonitor = serviceProvider.GetService<IOptionsMonitor<QueueOptions>>();
            if (queueOptionMonitor != null)
            {
                var queueOptions = queueOptionMonitor.Get(Name);
                if (queueOptions != null && queueOptions.ChannelOptions != null)
                {
                    if (queueOptions.ChannelOptions is UnboundedChannelOptions unboundedOptions)
                    {
                        _channel = Channel.CreateUnbounded<T>(unboundedOptions);
                    }
                    else if (queueOptions.ChannelOptions is BoundedChannelOptions boundedOptions)
                    {
                        _channel = Channel.CreateBounded<T>(boundedOptions);
                    }
                }
            }
            else
            {
                _channel = Channel.CreateUnbounded<T>();
            }

            ChannelReader = _channel.Reader;
            ChannelWriter = _channel.Writer;
        }

        #region Writer
        public void Complete(Exception? error = null)
        {
            ChannelWriter.Complete(error);
        }
        public bool TryComplete(Exception? error = null)
        {
            return ChannelWriter.TryComplete(error);
        }
        public bool TryWrite(T item)
        {
            return ChannelWriter.TryWrite(item);
        }
        public ValueTask<bool> WaitToWriteAsync(CancellationToken cancellationToken = default)
        {
            return ChannelWriter.WaitToWriteAsync(cancellationToken);
        }
        public ValueTask WriteAsync(T item, CancellationToken cancellationToken = default)
        {
            return ChannelWriter.WriteAsync(item, cancellationToken);
        }
        #endregion

        #region Reader
        //
        // 摘要:
        //     Creates an System.Collections.Generic.IAsyncEnumerable`1 that enables reading
        //     all of the data from the channel.
        //
        // 参数:
        //   cancellationToken:
        //     The cancellation token to use to cancel the enumeration. If data is immediately
        //     ready for reading, then that data may be yielded even after cancellation has
        //     been requested.
        //
        // 返回结果:
        //     The created async enumerable.
        public virtual IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return ChannelReader.ReadAllAsync(cancellationToken);
        }
        //
        // 摘要:
        //     Asynchronously reads an item from the channel.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken used to cancel the read operation.
        //
        // 返回结果:
        //     A System.Threading.Tasks.ValueTask`1 that represents the asynchronous read operation.
        public ValueTask<T> ReadAsync(CancellationToken cancellationToken = default)
        {
            return ChannelReader.ReadAsync(cancellationToken);
        }
        //
        // 摘要:
        //     Attempts to peek at an item from the channel.
        //
        // 参数:
        //   item:
        //     The peeked item, or a default value if no item could be peeked.
        //
        // 返回结果:
        //     true if an item was read; otherwise, false.
        public bool TryPeek([MaybeNullWhen(false)] out T item)
        {
            return ChannelReader.TryPeek(out item);
        }
        //
        // 摘要:
        //     Attempts to read an item from the channel.
        //
        // 参数:
        //   item:
        //     The read item, or a default value if no item could be read.
        //
        // 返回结果:
        //     true if an item was read; otherwise, false.
        public bool TryRead([MaybeNullWhen(false)] out T item)
        {
            return ChannelReader.TryRead(out item);
        }
        //
        // 摘要:
        //     Returns a System.Threading.Tasks.ValueTask`1 that will complete when data is
        //     available to read.
        //
        // 参数:
        //   cancellationToken:
        //     A System.Threading.CancellationToken used to cancel the wait operation.
        //
        // 返回结果:
        //     A System.Threading.Tasks.ValueTask`1 that will complete with a true result when
        //     data is available to read or with a false result when no further data will ever
        //     be available to be read due to the channel completing successfully.
        //     If the channel completes with an exception, the task will also complete with
        //     an exception.
        public ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default)
        {
            return ChannelReader.WaitToReadAsync(cancellationToken);
        }
        #endregion
    }

}
