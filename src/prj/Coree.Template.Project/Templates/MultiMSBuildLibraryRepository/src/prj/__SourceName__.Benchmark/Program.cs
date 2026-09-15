using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace __SourceName__.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public int Add()
    {
        var addTask = new __SourceName__.AddTask { Param1 = 1, Param2 = 2 };
        addTask.Execute();
        return addTask.AddResult;
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
