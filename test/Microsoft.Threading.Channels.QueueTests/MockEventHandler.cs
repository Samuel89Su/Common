using Microsoft.Threading.Channels.Queue;
using System;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class MockEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEventData
    {
        public Task HandleEventAsync(TEvent eventData)
        {
            Console.WriteLine(eventData.ToString());

            return Task.CompletedTask;
        }
    }
}
