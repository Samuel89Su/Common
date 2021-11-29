using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Jelly.Threading.Channels.Queue
{
    public class QueueBase<T> : IQueue<T>, IQueueProducer<T>
    {
        private readonly Channel<T> _channel;

        protected ChannelReader<T> ChannelReader { get; private set; }
        protected ChannelWriter<T> ChannelWriter { get; private set; }

        public string Name => typeof(T).Name;

        public IQueueProducer<T> QueueProducer => this;

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

            if (_channel == null)
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
        public IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return ChannelReader.ReadAllAsync(cancellationToken);
        }
        #endregion
    }

}
