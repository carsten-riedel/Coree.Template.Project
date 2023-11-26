using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Exporters;

namespace ClassLibrary.MSTest
{

    [DryJob] // [DryJob], [ShortRunJob], [MediumRunJob], [LongRunJob], [VeryLongRunJob]
    [Config(typeof(Config))]
    [JsonExporter("-custom", indentJson: true)]
    
    public class Benchmarks
    {
        public static Summary RunBenchmark()
        {
            return BenchmarkRunner.Run<Benchmarks>();
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                this.WithOptions(ConfigOptions.JoinSummary | ConfigOptions.DisableLogFile | ConfigOptions.DisableOptimizationsValidator);
                this.WithArtifactsPath("./../../../BenchmarkDotNetReport");
            }
        }

        [Benchmark]
        public void Benchmark1()
        {
            var result = ClassLibrary.Class1.Foo();
        }

        [Benchmark]
        public void Benchmark2()
        {
            int ss = 200 + 300;
            System.Threading.Thread.Sleep(ss);
            var result = ClassLibrary.Class1.Foo();
        }
    }

    
    [TestClass]
    public class BenchmarksTests
    {

        static Summary? summary;

        // This method will run once before any tests are executed in this class
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            summary = Benchmarks.RunBenchmark();
        }

        [TestMethod]
        public void Benchmark1()
        {
            var item = summary!.Reports.First(e => e.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo == nameof(Benchmarks.Benchmark1));

            double? MedianMs = item.ResultStatistics?.Median / 1000000.0;
            double? MeanMs = item.ResultStatistics?.Mean / 1000000.0;
            var MedianMsShort = Math.Round(MedianMs!.Value, 2);
            var MeanMsShort = Math.Round(MeanMs!.Value, 2);

            Trace.WriteLine($@"-------------------------------------------------------------------");
            Trace.WriteLine($@"{item.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo} Mean:   {MeanMsShort} ms");
            Trace.WriteLine($@"{item.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo} Median: {MedianMsShort} ms");

            Assert.IsTrue(MeanMsShort < 10);
        }

        [TestMethod]
        public void Benchmark2()
        {
            var item = summary!.Reports.First(e => e.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo == nameof(Benchmarks.Benchmark2));

            double? MedianMs = item.ResultStatistics?.Median / 1000000.0;
            double? MeanMs = item.ResultStatistics?.Mean / 1000000.0;
            var MedianMsShort = Math.Round(MedianMs!.Value, 2);
            var MeanMsShort = Math.Round(MeanMs!.Value, 2);

            Trace.WriteLine($@"-------------------------------------------------------------------");
            Trace.WriteLine($@"{item.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo} Mean:   {MeanMsShort} ms");
            Trace.WriteLine($@"{item.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo} Median: {MedianMsShort} ms");

            Assert.IsTrue(MeanMsShort < 550);
        }

    }
}
