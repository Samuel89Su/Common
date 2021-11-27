using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Threading.Channels.Queue
{
    public interface IEventHandler<in TEvent> where TEvent : IEventData
    {
        Task HandleEventAsync(TEvent eventData);
    }
}
