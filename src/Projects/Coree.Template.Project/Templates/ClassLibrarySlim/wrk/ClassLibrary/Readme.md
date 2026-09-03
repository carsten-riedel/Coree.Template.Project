# ClassLibrary

This folder is the generated solution root. Open a terminal here and run the commands as written. The CLI finds the solution; you do not pass a `.slnx` or `.csproj` path.

```text
./                         you are here
wrk/ClassLibrary/          this readme
prj/ClassLibrary/          packable class library
prj/ClassLibrary.Tests/    tests (not packed)
<!--#if (BenchmarkProject == true) -->
prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

Package metadata, license, icon, and release notes live in `prj/ClassLibrary/NugetAssets/`.

`--tl:off` is optional. Without it the CLI shows the compact terminal logger. Add `--tl:off` for the classic per-project log. The commands work either way.

## Restore and build

```bash
dotnet restore
dotnet build
```

## Test

From .NET 9, `dotnet test` runs target frameworks in parallel. Sequential TFMs keep coverage and result files from being written at the same time:

```bash
dotnet test -p:TestTfmsInParallel=false
```

After a test run, the links below point to generated reports. Each selected target framework writes its own files (`net8.0`, `net10.0`, …).

[Test results (trx)](prj/ClassLibrary.Tests/MSTestResults/ClassLibrary.Tests-__TargetFramework__.trx)
[Test results (html)](prj/ClassLibrary.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](prj/ClassLibrary.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator == true) -->
[ReportGenerator summary](prj/ClassLibrary.Tests/ReportGeneratorOutput/SummaryGithub.md)
<!--#endif -->

## Pack

```bash
dotnet pack
```

Creates a `.nupkg` in `prj/ClassLibrary/bin/Pack/` for every selected target framework.

Optional: copy the package to a local feed by setting `LocalPackagesDir` in the library project, or:

```bash
dotnet pack -p:LocalPackagesDir="path/to/local/packages"
```

## Publish

```bash
dotnet publish
```

Writes library output to `prj/ClassLibrary/bin/Publish/`. This is a class library, not an executable.

## CI

Use these flags so a pipeline does not depend on machine load. `-m:1` serializes MSBuild (avoids occasional file locks when the library is built as a solution project and as a test `ProjectReference` at the same time). `TestTfmsInParallel=false` serializes multi-target tests.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build -p:TestTfmsInParallel=false
dotnet pack
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

```bash
dotnet run --project prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
```
<!--#endif -->
