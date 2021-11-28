using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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
            services.AddQueue<int, MockConsumer>();

            return TestBase.serviceProvider = services.BuildServiceProvider();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
