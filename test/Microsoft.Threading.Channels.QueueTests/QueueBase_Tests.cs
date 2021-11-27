using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Threading.Channels.Queue;
using Shouldly;
using System;
using System.Threading.Channels;
using Xunit;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class UnboundedOptionStartup : StartupBase
    {
        public UnboundedOptionStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<QueueOptions>(typeof(int).Name)
                .Configure(options =>
                {
                    options.ChannelOptions = new UnboundedChannelOptions
                    {
                        SingleReader = true,
                    };
                });

            return base.ConfigureServices(services);
        }
    }

    public class BoundedOptionStartup : StartupBase
    {
        public BoundedOptionStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<QueueOptions>(typeof(int).Name)
                .Configure(options =>
                {
                    options.ChannelOptions = new BoundedChannelOptions(1000);
                });

            return base.ConfigureServices(services);
        }
    }

    public class QueueBase_Tests : TestBase
    {
        [Fact]
        public void Ctor_WithoutOption_Test()
        {
            StartServer<StartupBase>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            queue.ShouldNotBeNull();
        }

        [Fact]
        public void Ctor_UnboundedOption_Test()
        {
            StartServer<UnboundedOptionStartup>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            queue.ShouldNotBeNull();
        }

        [Fact]
        public void Ctor_BoundedOption_Test()
        {
            StartServer<BoundedOptionStartup>();

            var queue = serviceProvider.GetService<IQueue<int>>();

            queue.ShouldNotBeNull();
        }
    }
}
