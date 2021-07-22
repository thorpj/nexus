using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Nexus.Tests
{
    public abstract class TestsBase : IDisposable
    {
        protected static readonly IConfiguration Configuration = new ConfigurationBuilder().AddUserSecrets<TestsBase>().Build();
        protected readonly ITestOutputHelper Output;
        protected readonly ILogger Logger;

        // Runs before all tests to provide shared context
        protected TestsBase(ITestOutputHelper output)
        {
            this.Output = output;
            // Logger = output.BuildLogger();
        }

        // Runs after all tests
        public void Dispose()
        {
            
        }
    }
}