# Solution readme.md

After running the tests, you'll find that the local links are now active. Additionally formats like HTML and Markdown files are generated. Feel free to explore these files in their respective directories for a more comprehensive view.

```
dotnet test
```

[MSTest results in trx format (html is generated too)](prj/ClassLibrary.MSTest/MSTestResults/ClassLibrary.MSTest.trx)
<!--#if (BenchmarkProject == true) -->
Run the optional benchmarks with:

```
dotnet run --project prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
```
<!--#endif -->
<!--#if (CoverletMSBuild) -->
[CoverletOutput](prj/ClassLibrary.MSTest/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator) -->
[ReportGeneratorOutput](prj/ClassLibrary.MSTest/ReportGeneratorOutput/SummaryGithub.md)
<!--#endif -->