using Microsoft.Extensions.DependencyInjection;
using Jelly.Threading.Channels.Queue;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jelly.Threading.Channels.QueueTests
{
    public class IQueueConsumer_Tests : TestBase
    {
        [Fact]
        public void Ctor_Test()
        {
            StartServer<StartupBase>();

            serviceProvider.GetService<IQueueConsumer<int>>()
                .ShouldNotBeNull();
        }

        [Fact]
        public async void ReadAllAsync_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            var producer = queue.QueueProducer;

            Task.Factory.StartNew(async () =>
            {
                await producer.WriteAsync(1);

                await Task.Delay(TimeSpan.FromSeconds(3));

                producer.TryComplete();
            });

            var consumer = serviceProvider.GetService<IQueueConsumer<int>>();

            await consumer.StartConsume();
        }
    }

    public class MockConsumer : QueueConsumerBase<int>
    {
        public MockConsumer(IQueue<int> queue) : base(queue)
        {
        }

        protected override async Task Handle(int data)
        {
            Console.WriteLine(data);
        }
    }
}
