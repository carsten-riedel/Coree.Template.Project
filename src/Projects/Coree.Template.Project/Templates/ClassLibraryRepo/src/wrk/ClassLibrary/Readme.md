# ClassLibrary

The solution root is the repository's `src/` directory. Open a terminal there for the commands below. The CLI finds the solution; you do not pass a `.slnx` or `.csproj` path.

When creating this template through `dotnet new`, add `--TryOpenInVisualStudio --allow-scripts yes` to try opening the solution in Visual Studio 2026 on Windows. The temporary `src/wrk/Open-VisualStudio.ps1` helper uses `vswhere` to select VS 2026 and deletes itself after launching the process. If launching fails, the repository remains available and the helper stays for another attempt. Without the switch, no helper is generated.

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

The test project explicitly allows target frameworks to run in parallel. Test results, coverage files, vulnerability reports, and ReportGenerator output are isolated per target framework. No parallelism switch is needed on the command line:

```bash
dotnet test
```

MSTest is explicitly configured for method-level parallel execution within one test assembly. Tests must therefore not share mutable global state.

After a test run, the links below point to generated reports. Each selected target framework writes its own files (`net8.0`, `net10.0`, …).

[Test results (trx)](prj/ClassLibrary.Tests/MSTestResults/ClassLibrary.Tests-__TargetFramework__.trx)
[Test results (html)](prj/ClassLibrary.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](prj/ClassLibrary.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator == true) -->
ReportGenerator writes one summary per target framework under `prj/ClassLibrary.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
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

Use `-m:1` for the build so a pipeline does not depend on machine load. It avoids occasional file locks when the library is built as a solution project and as a test `ProjectReference` at the same time. Multi-target test execution is already configured as parallel in the test project.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build
dotnet pack
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

```bash
dotnet run --project prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
```
<!--#endif -->
