using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ClassLibrary;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ClassLibrary.Benchmark;

public class Benchmarks
{
    [Benchmark]
    public DiagnosticAnalyzer CreateEmDashAnalyzer()
    {
        return new EmDashAnalyzer();
    }

    [Benchmark]
    public DiagnosticAnalyzer CreateSmartQuotesAnalyzer()
    {
        return new SmartQuotesAnalyzer();
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
