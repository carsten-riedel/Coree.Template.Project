# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder is the per-app area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` lives here; keep the folder while that is true. You can still add extra solution items here.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This folder is the WPF app. The `.slnx` and this readme sit next to the `.csproj`. Tests and the optional benchmark stay sibling projects under `src/prj/`. There is no `src/sln/` tree for this app.
<!--#else -->
This folder is the per-app area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` is written elsewhere (`PlaceSolution`). Use this folder for shared notes or extra solution items, or delete it if you do not need it.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "SlnFolder")) -->

Visual Studio created an extra `.slnx` in the repository root. Close this solution, open the `.slnx` in this folder (`src/sln/__SourceName__/`), and delete the extra `.slnx` in the repository root.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "BesideCsproj")) -->

Visual Studio created an extra `.slnx` in the repository root. Close this solution, open `src/prj/__SourceName__/__SourceName__.slnx`, and delete the extra `.slnx` in the repository root.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "RepoRoot")) -->

Visual Studio created a conventional root `.slnx`. Open `__SourceName__.generated.slnx` in the repository root for the template layout (includes `sln`). Delete the extra conventional `.slnx` if you do not need it.
<!--#endif -->
<!--#if (PlaceSolution == "RepoRoot") -->

The solution file lives at the repository root. Open a terminal there for `dotnet restore`, `dotnet build`, `dotnet test`, and `dotnet publish`. If more than one `.slnx` sits in that directory, pass the solution path to `dotnet`.
<!--#if (WriteSrcGlobalJson) -->
`src/global.json` does not apply to those commands: the SDK muxer starts at the repository root and does not walk into `src/`.
<!--#endif -->
<!--#else -->

<!--#if (PlaceSolution == "SlnFolder") -->
The `.slnx` and this readme live in this folder. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other apps keep their own `.slnx` under `src/sln/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#else -->
The `.slnx` and this readme live in this folder next to the WPF app. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other apps keep their own `.slnx` under `src/prj/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#endif -->

```text
<!--#if (PlaceSolution == "SlnFolder") -->
./                         you are here (this readme + __SourceName__.slnx)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
../../global.json            .NET SDK pin (highest selected TFM)
<!--#endif -->
../../prj/__SourceName__/    WPF app
<!--#if (DirectoryMsBuildFiles) -->
../../prj/__SourceName__/Directory.Build.props  optional empty WPF app MSBuild landing file
../../prj/__SourceName__/Directory.Build.targets
<!--#endif -->
<!--#if (DotNetToolManifest) -->
../../prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../../prj/__SourceName__.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
../../prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
<!--#else -->
./                         you are here (this readme + __SourceName__.slnx + __SourceName__.csproj)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
./Directory.Build.props     optional empty WPF app MSBuild landing file
./Directory.Build.targets
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
../global.json               .NET SDK pin (highest selected TFM)
<!--#endif -->
<!--#if (DotNetToolManifest) -->
./.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../__SourceName__.Tests/     tests (not packed)
<!--#if (BenchmarkProject == true) -->
../__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
<!--#endif -->
```

`--tl:off` is optional. Without it the CLI shows the compact terminal logger. Add `--tl:off` for the classic per-project log. The commands work either way.

## Restore and build

```bash
dotnet restore
dotnet build
```

## Test

The test project explicitly allows target frameworks to run in parallel. Test results and other per-target-framework reports next to the test project are isolated. No parallelism switch is needed on the command line:

```bash
dotnet test
```

MSTest is explicitly configured for method-level parallel execution within one test assembly. Tests must therefore not share mutable global state.

After a test run, the links below point to generated reports. Each selected target framework writes its own files (`net8.0-windows`, `net10.0-windows`, …).

<!--#if (PlaceSolution == "SlnFolder") -->
[Test results (trx)](../../prj/__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TargetFramework__.trx)
[Test results (html)](../../prj/__SourceName__.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../../prj/__SourceName__.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#else -->
[Test results (trx)](../__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TargetFramework__.trx)
[Test results (html)](../__SourceName__.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../__SourceName__.Tests/CoverletOutput/coverage.__TargetFramework__.opencover.xml)
<!--#endif -->
<!--#endif -->

<!--#if (CoverletMSBuild == true) -->
<!--#if (ProgramSample == "MahAppsMvvm") -->
Coverlet measures the testable application code (`[__SourceName__]*`), skips generated `*.g.cs` / `*.xaml`, and excludes WPF startup plus view code-behind. ViewModels and extensions remain gated at 100% line, branch, and method coverage.
<!--#else -->
Coverlet measures the app (`[__SourceName__]*`), skips generated `*.g.cs` / `*.xaml` (WPF `App.Main` and markup compile), and fails `dotnet test` if line, branch, or method coverage is under 100% on the rest.
<!--#endif -->
<!--#endif -->
<!--#if (ReportGenerator == true) -->
<!--#if (PlaceSolution == "SlnFolder") -->
ReportGenerator writes one summary per target framework under `../../prj/__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0-windows/SummaryGithub.md`).
<!--#else -->
ReportGenerator writes one summary per target framework under `../__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0-windows/SummaryGithub.md`).
<!--#endif -->
<!--#endif -->

## Publish

```bash
dotnet publish
```

`dotnet publish` with no `--framework` publishes the default TFM from `src/prj/__SourceName__/Properties/Build/PublishDefaultFramework.targets` to `src/prj/__SourceName__/bin/Publish/`. Override with `dotnet publish --framework net8.0-windows` (or another selected TFM). Edit that targets file to change the default. To try a newer TFM (net11.0-windows, …) before shipping it, append it to `TargetFrameworks` so build/test catch incompatibilities; leave the targets file until bare publish should follow. Publish always uses `win-x64`. The publish recipe (self-contained, single-file, …) is a generate-time choice and applies only while publishing. Test and optional benchmark projects are not published.

## Pack

`IsPackable` is `false`. Distribution is `dotnet publish`. These apps are not NuGet tools. Test and optional benchmark projects are not packed.

## CI

Use `-m:1` for the build so a pipeline does not depend on machine load. It avoids occasional file locks when the app is built as a solution project and as a test `ProjectReference` at the same time. Multi-target test execution is already configured as parallel in the test project. Run these commands from this folder so each app has exactly one `.slnx` in the working directory.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build
dotnet publish --no-build
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

The benchmark is a local executable targeting the highest selected framework. It is neither packed nor published; start it with the command below.

```bash
<!--#if (PlaceSolution == "SlnFolder") -->
dotnet run --project ../../prj/__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#else -->
dotnet run --project ../__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#endif -->
```
<!--#endif -->
<!--#endif -->
