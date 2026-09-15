using BenchmarkDotNet.Attributes;

namespace __SourceName__.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public int Main()
    {
        return __SourceName__.Program.Main(System.Array.Empty<string>());
    }
}
