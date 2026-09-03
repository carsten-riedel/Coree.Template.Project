# Solution readme.md

After running the tests, you'll find that the local links are now active. Additionally formats like HTML and Markdown files are generated. Feel free to explore these files in their respective directories for a more comprehensive view.

```
dotnet test
```

[Test results in trx format (html is generated too)](prj/ClassLibrary.Tests/MSTestResults/ClassLibrary.Tests.trx)
<!--#if (BenchmarkProject == true) -->
Run the optional benchmarks with:

```
dotnet run --project prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
```
<!--#endif -->
<!--#if (CoverletMSBuild) -->
[CoverletOutput](prj/ClassLibrary.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator) -->
[ReportGeneratorOutput](prj/ClassLibrary.Tests/ReportGeneratorOutput/SummaryGithub.md)
<!--#endif -->