using BenchmarkDotNet.Attributes;

namespace __SourceName__.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public string Form1_Text()
    {
        using var form = new __SourceName__.Form1();
        return form.Text;
    }
}
