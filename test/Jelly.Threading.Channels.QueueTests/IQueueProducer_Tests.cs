using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Jelly.Threading.Channels.Queue;
using Shouldly;
using System;
using System.Threading.Channels;
using Xunit;

namespace Jelly.Threading.Channels.QueueTests
{
    public class IQueueProducer_Tests : TestBase
    {
        [Fact]
        public async void WriteAsync_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            await producer.WriteAsync(1);
        }

        [Fact]
        public void TryWrite_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            producer.TryWrite(1)
                .ShouldBeTrue();
        }

        [Fact]
        public async void WaitToWriteAsync_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            (await producer.WaitToWriteAsync(default))
                .ShouldBeTrue();
        }

        [Fact]
        public void Complete_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            producer.Complete(default);
        }

        [Fact]
        public void TryComplete_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            producer.TryComplete(default)
                .ShouldBeTrue();
        }
    }
}
