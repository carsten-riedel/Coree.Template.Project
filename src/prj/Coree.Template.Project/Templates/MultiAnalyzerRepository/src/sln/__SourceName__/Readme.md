# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder is the per-package area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` lives here; keep the folder while that is true. You can still add extra solution items here.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This folder is the packable analyzer package. The `.slnx` and this readme sit next to the `.csproj`. Tests, DebugHost, and the optional benchmark stay sibling projects under `src/prj/`. There is no `src/sln/` tree for this package.
<!--#else -->
This folder is the per-package area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
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

The solution file lives at the repository root. Open a terminal there for `dotnet restore`, `dotnet build`, `dotnet test`, and `dotnet pack`. If more than one `.slnx` sits in that directory, pass the solution path to `dotnet`.
<!--#if (WriteSrcGlobalJson) -->
`src/global.json` does not apply to those commands: the SDK muxer starts at the repository root and does not walk into `src/`.
<!--#endif -->
<!--#else -->

<!--#if (PlaceSolution == "SlnFolder") -->
The `.slnx` and this readme live in this folder. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other packages keep their own `.slnx` under `src/sln/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#else -->
The `.slnx` and this readme live in this folder next to the packable analyzer. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other packages keep their own `.slnx` under `src/prj/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#endif -->

```text
<!--#if (PlaceSolution == "SlnFolder") -->
./                         you are here (this readme + __SourceName__.slnx)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
../../global.json            .NET SDK pin (DebugHost TFM)
<!--#endif -->
../../prj/__SourceName__/    packable analyzer package (netstandard2.0)
<!--#if (DirectoryMsBuildFiles) -->
../../prj/__SourceName__/Directory.Build.props  optional empty analyzer MSBuild landing file
../../prj/__SourceName__/Directory.Build.targets
<!--#endif -->
<!--#if (DotNetToolManifest) -->
../../prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../../prj/__SourceName__.Tests/  tests (not packed)
../../prj/__SourceName__.DebugHost/  Visual Studio F5 compile target (not packed)
<!--#if (BenchmarkProject == true) -->
../../prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
<!--#else -->
./                         you are here (this readme + __SourceName__.slnx + __SourceName__.csproj)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
./Directory.Build.props     optional empty analyzer MSBuild landing file
./Directory.Build.targets
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
../global.json               .NET SDK pin (DebugHost TFM)
<!--#endif -->
<!--#if (DotNetToolManifest) -->
./.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../__SourceName__.Tests/     tests (not packed)
../__SourceName__.DebugHost/ Visual Studio F5 compile target (not packed)
<!--#if (BenchmarkProject == true) -->
../__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
<!--#endif -->
```

Package metadata, license, icon, and release notes live in `src/prj/__SourceName__/Properties/NugetMetadata/`.

`--tl:off` is optional. Without it the CLI shows the compact terminal logger. Add `--tl:off` for the classic per-project log. The commands work either way.

## Restore and build

```bash
dotnet restore
dotnet build
```

## Test

The test project explicitly configures MSTest for method-level parallel execution within one test assembly. Tests must therefore not share mutable global state.

```bash
dotnet test
```

After a test run, the links below point to generated reports for the test host TFM.

<!--#if (PlaceSolution == "SlnFolder") -->
[Test results (trx)](../../prj/__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TargetFramework__.trx)
[Test results (html)](../../prj/__SourceName__.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../../prj/__SourceName__.Tests/CoverletOutput/coverage.opencover.xml)
<!--#endif -->
<!--#else -->
[Test results (trx)](../__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TargetFramework__.trx)
[Test results (html)](../__SourceName__.Tests/MSTestResults/result-__TargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../__SourceName__.Tests/CoverletOutput/coverage.opencover.xml)
<!--#endif -->
<!--#endif -->

<!--#if (CoverletMSBuild == true) -->
Coverlet measures only the analyzer assembly (`[__SourceName__]*`) and fails `dotnet test` if line, branch, or method coverage is under 100%.
<!--#endif -->
<!--#if (ReportGenerator == true) -->
<!--#if (PlaceSolution == "SlnFolder") -->
ReportGenerator writes a summary under `../../prj/__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#else -->
ReportGenerator writes a summary under `../__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#endif -->
<!--#endif -->

## Pack

```bash
dotnet pack
```

Creates one `.nupkg` in `src/prj/__SourceName__/bin/Pack/` with the analyzer under `analyzers/dotnet/cs` (not `lib/`) and `__SourceName__.props` under `build/` and `buildTransitive/` (`EmDashAnalyzerSeverity`, `SmartQuotesAnalyzerSeverity`, `EmDashAnalyzerIncludes`, `EmDashAnalyzerExcludes`, `SmartQuotesAnalyzerIncludes`, `SmartQuotesAnalyzerExcludes`). Test, DebugHost, and optional benchmark projects are not packed.
<!--#if (NuGetAuditHighCriticalAsErrors) -->

Restore fails this analyzer package on high (`NU1903`) and critical (`NU1904`) vulnerable packages. Low and moderate stay warnings. `NugetReport` next to the tests lists that package’s packages (txt/json) and is still info-only.
<!--#endif -->
<!--#if (PublicApiAnalyzers) -->

## Public API baseline

The analyzer project uses `Microsoft.CodeAnalysis.PublicApiAnalyzers`. The first real `dotnet build` writes `src/prj/__SourceName__/Properties/PublicAPI/PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt` if they are missing, then records the current public surface. Commit those files. Later public additions belong in `PublicAPI.Unshipped.txt` (analyzer RS0016 / `dotnet format analyzers` with `--diagnostics RS0016`).
<!--#endif -->
<!--#if (WritePackageDocTemplate) -->

## Package documentation template

`src/prj/__SourceName__/Properties/NugetMetadata/docs/DocShell.html` is the offline documentation seed. It packs with the nupkg (`docs/` inside the package). Bootstrap the site from that file (vendor the local css/js/licenses next to it), then write package documentation for this analyzer. Repository-root `docs/` is a separate site if present.
<!--#endif -->

## Publish

The analyzer package sets `IsPublishable` to `false`. Distribution is `dotnet pack`. To write output to `src/prj/__SourceName__/bin/Publish/` for a one-off inspect, set `IsPublishable` to `true` and run:

```bash
dotnet publish
```

This is a Roslyn analyzer, not an executable. Consumers install the nupkg; they do not publish this project.

## CI

Use `-m:1` for the build so a pipeline does not depend on machine load. It avoids occasional file locks when the analyzer is built as a solution project and as a test `ProjectReference` at the same time. Multi-target test execution is already configured as parallel in the test project. Run these commands from this folder so each package has exactly one `.slnx` in the working directory.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build
dotnet pack
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

The benchmark is a local executable targeting the selected test host framework. It is neither packed nor published; start it with the command below.

```bash
<!--#if (PlaceSolution == "SlnFolder") -->
dotnet run --project ../../prj/__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#else -->
dotnet run --project ../__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#endif -->
```
<!--#endif -->
<!--#endif -->

## Debug (Visual Studio)

To break in the analyzer, install the **.NET Compiler Platform SDK** Visual Studio component, then:

1. Set `__SourceName__` as the startup project (not `__SourceName__.DebugHost`).
2. Select the `__SourceName__` launch profile (Roslyn Component).
3. Set a breakpoint in `EmDashAnalyzer` or `SmartQuotesAnalyzer`.
4. Press F5. Visual Studio compiles `__SourceName__.DebugHost` and attaches to that compilation.

`__SourceName__.DebugHost` is only the compile target. The em dash in `"1—2"` reports EMD001; the typographic quotes in `"“hello”"` report TSQ001. ASCII `"1-2"` and `"hello"` do not. `SampleTypography.txt` and this host csproj are additional files for the same IDs when the glob properties match. F5 / `dotnet run` on the console only runs `Main`; it does not attach to the analyzer.

Severity is an MSBuild property on the compile target (`EmDashAnalyzerSeverity`, `SmartQuotesAnalyzerSeverity`): `warning` (default), `error`, `message`, or `off`. Additional-file globs are `EmDashAnalyzerIncludes` / `EmDashAnalyzerExcludes` and the SmartQuotes pair (semicolon-separated, project directory; demo includes `*.txt;*.csproj`; empty includes skip that scan; excludes subtract from that analyzer's includes). The analyzer nupkg ships `build/` and `buildTransitive/` props so PackageReference consumers get the same knobs. DebugHost imports that props file because it uses a project analyzer reference, not the nupkg.

For stepping without F5, debug `FunctionalTests` from Test Explorer.

`DebugHost` is not packed. `dotnet pack` still produces only the analyzer nupkg.
