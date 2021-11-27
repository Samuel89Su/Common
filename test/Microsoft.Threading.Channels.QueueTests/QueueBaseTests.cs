using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Threading.Channels.Queue;
using Shouldly;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class QueueBaseTests : TestBase
    {
        [Fact]
        public void Ctor_Test()
        {
            var queue = serviceProvider.GetService<IQueue<int>>();

            queue.ShouldNotBeNull();
        }

        [Fact]
        public async void WriteAsync_Test()
        {
            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            await producer.WriteAsync(1);
        }

        [Fact]
        public async void ReadAsync_Test()
        {
            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;
            var consumer = queue.QueueConsumer;

            await producer.WriteAsync(1);

            var val = await consumer.ReadAsync();

            val.ShouldBe(1);
        }
    }
}
