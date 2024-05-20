# Solution readme.md

After running the tests, you'll find that the local links are now active. Additionally formats like HTML and Markdown files are generated. Feel free to explore these files in their respective directories for a more comprehensive view.

```
dotnet test
```

[MSTest results in trx format (html is generated too)](ClassLibrary.MSTest/MSTestResults/ClassLibrary.MSTest.trx)
<!--#if (CoverletMSBuild) -->
[CoverletOutput](ClassLibrary.MSTest/CoverletOutput/coverage.net6.0.opencover.xml)
<!--#endif -->
<!--#if (BenchmarkDotNet) -->
[BenchmarkDotNetReport](ClassLibrary.MSTest/BenchmarkDotNetReport/results/ClassLibrary.MSTest.Benchmarks-report-github.md)
<!--#endif -->
<!--#if (ReportGenerator) -->
[ReportGeneratorOutput](ClassLibrary.MSTest/ReportGeneratorOutput/SummaryGithub.md)
<!--#endif -->
<!--#if (AddDocfx) -->
[Docfx](ClassLibrary.MSTest/Docfx/result/web/api/toc.pdf)
<!--#endif -->
