using BenchmarkDotNet.Attributes;

using __SourceName__.Extensions;

namespace __SourceName__.Benchmark;

public class Benchmarks
{
    private readonly string value = "MahApps MVVM";

    [Benchmark]
    public string EmptyIfNull()
    {
        return value.EmptyIfNull();
    }
}
