using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Threading.Channels.Queue;
using System;
using System.Threading.Channels;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class Startup
    {
        private IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            configuration = configuration;

            var proxy = configuration["HTTP_PROXY"];
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptor.Singleton(typeof(IQueue<>), typeof(QueueBase<>)));

            services.AddOptions<QueueOptions>(typeof(int).Name)
                .Configure(options =>
                {
                    options.ChannelOptions = new UnboundedChannelOptions
                    {
                        SingleReader = true,
                    };
                });

            return TestBase.serviceProvider = services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
