using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ClassLibrary.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public string Foo()
    {
        return ClassLibrary.Class1.Foo();
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        SetProjectDirectoryAsCurrentDirectory();
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }

    private static void SetProjectDirectoryAsCurrentDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (directory.EnumerateFiles("*.csproj").Any())
            {
                Directory.SetCurrentDirectory(directory.FullName);
                return;
            }

            directory = directory.Parent;
        }
    }
}
