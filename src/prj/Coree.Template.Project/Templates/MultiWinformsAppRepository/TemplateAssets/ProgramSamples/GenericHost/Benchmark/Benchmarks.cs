#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System.Collections.Generic;

#endif
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;

namespace __SourceName__.Benchmark
{
    public class Benchmarks
    {
        private readonly IConfiguration configuration;

        public Benchmarks()
        {
            var values = new Dictionary<string, string?>
            {
                [MainForm.WindowTitleConfigurationKey] = "Benchmark title",
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(values)
                .Build();

            this.configuration = configuration;
        }

        [Benchmark]
        public string ReadWindowTitle()
        {
            return configuration[MainForm.WindowTitleConfigurationKey] ?? "MainForm";
        }
    }
}
