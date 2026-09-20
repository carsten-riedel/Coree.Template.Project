#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System.Collections.Generic;

#endif
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;

namespace __SourceName__.Benchmark
{
    public class Benchmarks
    {
        private readonly WindowTitleProvider windowTitleProvider;

        public Benchmarks()
        {
            var values = new Dictionary<string, string?>
            {
                [WindowTitleProvider.ConfigurationKey] = "Benchmark title",
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            windowTitleProvider = new WindowTitleProvider(configuration);
        }

        [Benchmark]
        public string ReadWindowTitle()
        {
            return windowTitleProvider.GetWindowTitle();
        }
    }
}
