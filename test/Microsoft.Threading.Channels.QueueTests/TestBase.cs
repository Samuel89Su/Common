using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Threading.Channels.QueueTests
{
    public class TestBase
    {
        internal static IServiceProvider serviceProvider;

        private TestServer _server;
        public TestBase()
        {
            _server = new TestServer(
                WebHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>());
        }
    }
}
