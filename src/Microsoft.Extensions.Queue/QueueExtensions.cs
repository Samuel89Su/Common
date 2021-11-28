using Microsoft.Threading.Channels.Queue;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static class QueueExtensions
    {
        public static IServiceCollection AddQueue<T, TConsumer>(this IServiceCollection services) where TConsumer : IQueueConsumer<T>
        {
            services.AddQueue<T, TConsumer, QueueBase<T>>();

            return services;
        }

        public static IServiceCollection AddQueue<T, TConsumer, TQueue>(this IServiceCollection services) where TConsumer : IQueueConsumer<T> where TQueue : IQueue<T>
        {
            services.Add(ServiceDescriptor.Singleton(typeof(IQueue<T>), typeof(TQueue)));
            services.Add(ServiceDescriptor.Transient(typeof(IQueueConsumer<T>), typeof(TConsumer)));

            return services;
        }
    }
}
