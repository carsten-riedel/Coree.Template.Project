# ClassLibrary
<!--#if ((HostIdentifier == "vs") && PlaceSolutionInSolutionFolder) -->

Visual Studio created an extra `.slnx` in the repository root. Close this solution, open the `.slnx` in this folder (`src/sln/ClassLibrary/`), and delete the extra `.slnx` in the repository root.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolutionInSolutionFolder == false)) -->

Visual Studio created a conventional root `.slnx`. Open `ClassLibrary.generated.slnx` in the repository root for the template layout (includes `sln`). Delete the extra conventional `.slnx` if you do not need it.
<!--#endif -->

<!--#if (PlaceSolutionInSolutionFolder) -->
The `.slnx` and this readme live in this folder. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other libraries keep their own `.slnx` under `src/sln/<name>/`, so `dotnet` does not ask you to specify a solution.

```text
./                         you are here (this readme + ClassLibrary.slnx)
../../prj/ClassLibrary/    packable class library
../../prj/ClassLibrary.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
../../prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```
<!--#else -->
The solution file lives at the repository root. Open a terminal there for the commands below. If more than one `.slnx` sits in that directory, pass the solution path to `dotnet`.

```text
./                         repository root
src/sln/ClassLibrary/      this readme
src/prj/ClassLibrary/      packable class library
src/prj/ClassLibrary.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```
<!--#endif -->

Package metadata, license, icon, and release notes live in `src/prj/ClassLibrary/NugetAssets/`.

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
<!--#if (PlaceSolutionInSolutionFolder) -->

[Test results (trx)](../../prj/ClassLibrary.Tests/MSTestResults/ClassLibrary.Tests-__TargetFramework__.trx)
[Test results (html)](../../prj/ClassLibrary.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../../prj/ClassLibrary.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator == true) -->
ReportGenerator writes one summary per target framework under `../../prj/ClassLibrary.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#endif -->
<!--#else -->

[Test results (trx)](src/prj/ClassLibrary.Tests/MSTestResults/ClassLibrary.Tests-__TargetFramework__.trx)
[Test results (html)](src/prj/ClassLibrary.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](src/prj/ClassLibrary.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#if (ReportGenerator == true) -->
ReportGenerator writes one summary per target framework under `src/prj/ClassLibrary.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#endif -->
<!--#endif -->

## Pack

```bash
dotnet pack
```

Creates one `.nupkg` in `src/prj/ClassLibrary/bin/Pack/` containing the library for all selected target frameworks. Test and optional benchmark projects are not packed.

Optional: copy the package to a local feed by setting `LocalPackagesDir` in the library project, or:

```bash
dotnet pack -p:LocalPackagesDir="path/to/local/packages"
```

## Publish

The class library sets `IsPublishable` to `false`. Distribution is `dotnet pack`. To write output to `src/prj/ClassLibrary/bin/Publish/` for the highest selected target framework, set `IsPublishable` to `true` and run:

```bash
dotnet publish
```

This is a class library, not an executable.

## CI

Use `-m:1` for the build so a pipeline does not depend on machine load. It avoids occasional file locks when the library is built as a solution project and as a test `ProjectReference` at the same time. Multi-target test execution is already configured as parallel in the test project. Run these commands from this folder so each library has exactly one `.slnx` in the working directory.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build
dotnet pack
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

The benchmark is a local executable targeting the highest selected framework. It is neither packed nor published; start it with the command below.

```bash
<!--#if (PlaceSolutionInSolutionFolder) -->
dotnet run --project ../../prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
<!--#else -->
dotnet run --project src/prj/ClassLibrary.Benchmark/ClassLibrary.Benchmark.csproj -c Release
<!--#endif -->
```
<!--#endif -->
