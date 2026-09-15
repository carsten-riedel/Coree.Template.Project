using BenchmarkDotNet.Attributes;

namespace __SourceName__.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public string MainWindow_Title()
    {
        var window = new __SourceName__.MainWindow();
        var title = window.Title;
        window.Close();
        return title;
    }
}
