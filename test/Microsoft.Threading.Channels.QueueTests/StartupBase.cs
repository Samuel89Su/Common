using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Threading.Channels.Queue;
using System;
using System.Threading.Channels;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class StartupBase : IStartup
    {
        protected IConfiguration Configuration;
        public StartupBase(IConfiguration configuration)
        {
            Configuration = configuration;

            var proxy = Configuration["HTTP_PROXY"];
        }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Singleton(typeof(IQueue<>), typeof(QueueBase<>)));

            services.Add(ServiceDescriptor.Transient(typeof(IQueueConsumer<,,>), typeof(QueueConsumerBase<,,>)));
            services.Add(ServiceDescriptor.Transient(typeof(IQueueConsumer<>), typeof(QueueConsumerBase<>)));

            services.Add(ServiceDescriptor.Transient(typeof(IEventHandler<>), typeof(MockEventHandler<>)));

            return TestBase.serviceProvider = services.BuildServiceProvider();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
