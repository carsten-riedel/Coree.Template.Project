using System;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Running;

namespace __SourceName__.Benchmark;

public static class Program
{
    [STAThread]
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
