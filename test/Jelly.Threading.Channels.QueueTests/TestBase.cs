using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jelly.Threading.Channels.QueueTests
{
    public class TestBase
    {
        internal static IServiceProvider serviceProvider;

        protected TestServer StartServer<TStartup>() where TStartup : class, IStartup
        {
            return new TestServer(
                WebHost
                .CreateDefaultBuilder()
                .UseStartup<TStartup>());
        }
    }
}
