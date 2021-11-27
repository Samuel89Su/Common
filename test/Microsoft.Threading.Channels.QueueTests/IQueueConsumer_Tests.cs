using Microsoft.Threading.Channels.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class IQueueConsumer_Tests : TestBase
    {
        [Fact]
        public void Ctor_Test()
        {
            StartServer<StartupBase>();

            serviceProvider.GetService<IQueueConsumer<MockEvent, IQueue<MockEvent>, IEventHandler<MockEvent>>>()
                .ShouldNotBeNull();

            serviceProvider.GetService<IQueueConsumer<MockEvent>>()
                .ShouldNotBeNull();
        }

        [Fact]
        public async void ReadAllAsync_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<MockEvent>>();

            var producer = queue.QueueProducer;

            Task.Factory.StartNew(async () =>
            {
                await producer.WriteAsync(new MockEvent { Id = 1 });

                await Task.Delay(TimeSpan.FromSeconds(3));

                producer.TryComplete();
            });

            var consumer = serviceProvider.GetService<IQueueConsumer<MockEvent>>();

            await consumer.StartConsume();
        }
    }

    public class MockEvent : IEventData
    {
        public int Id { get; set; }
    }
}
