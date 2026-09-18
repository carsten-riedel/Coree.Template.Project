# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder is the per-library area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` lives here; keep the folder while that is true. You can still add extra solution items here.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This folder is the packable MSBuild task library. The `.slnx` and this readme sit next to the `.csproj`. Tests, DebugHost, and the optional benchmark stay sibling projects under `src/prj/`. There is no `src/sln/` tree for this library.
<!--#else -->
This folder is the per-library area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
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
The `.slnx` and this readme live in this folder. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other libraries keep their own `.slnx` under `src/sln/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#else -->
The `.slnx` and this readme live in this folder next to the packable library. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other libraries keep their own `.slnx` under `src/prj/<name>/`, so `dotnet` does not ask you to specify a solution.
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
../../prj/__SourceName__/    packable MSBuild task library
<!--#if (DirectoryMsBuildFiles) -->
../../prj/__SourceName__/Directory.Build.props  optional empty library MSBuild landing file
../../prj/__SourceName__/Directory.Build.targets
<!--#endif -->
<!--#if (DotNetToolManifest) -->
../../prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../../prj/__SourceName__.Tests/  automated tests (not packed)
../../prj/__SourceName__.DebugHost/  Visual Studio F5 MSBuild consumer (not packed)
<!--#if (BenchmarkProject == true) -->
../../prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
<!--#else -->
./                         you are here (this readme + __SourceName__.slnx + __SourceName__.csproj)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
./Directory.Build.props     optional empty library MSBuild landing file
./Directory.Build.targets
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
../global.json               .NET SDK pin (DebugHost TFM)
<!--#endif -->
<!--#if (DotNetToolManifest) -->
./.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
../__SourceName__.Tests/     automated tests (not packed)
../__SourceName__.DebugHost/ Visual Studio F5 MSBuild consumer (not packed)
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

The test project explicitly configures MSTest for method-level parallel execution within one test assembly. Tests must therefore not share mutable global state. `__SourceName__.Tests` is automated unit and integration tests only (`FunctionalTests` plus `IntegrationTests` / `Resources/TestScript.msbuild`). `__SourceName__.DebugHost` is the manual F5 MSBuild consumer; do not put those tests there.

```bash
dotnet test
```

After a test run, the links below point to generated reports for the DebugHost TFM.

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
Coverlet measures only the task library (`[__SourceName__]*`, excluding `[__SourceName__.DebugHost]*`) and fails `dotnet test` if line, branch, or method coverage is under 100%. `IntegrationTests` runs MSBuild in a subprocess and does not count toward that gate; `FunctionalTests` covers `AddTask.Execute()`, `TaskNodeTask.Execute()`, `HomeTask.Execute()`, and `DumpEnvVarsTask.Execute()`. `HomeTask` marks OS-specific helpers with `[ExcludeFromCodeCoverage]` so one testhost does not have to hit every platform branch.
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

Creates one `.nupkg` in `src/prj/__SourceName__/bin/Pack/` with the task assembly under `tasks/netstandard2.0/` (not `lib/`) and `__SourceName__.props` / `__SourceName__.targets` under `build/`. PackageReference consumers get those files auto-imported into their csproj: `UsingTask` in `.props`, sample `CoreCompile` property populate (`HomeTask` / `TaskNodeTask`) in `.targets`. Private runtime dependencies and `deps.json` land next to the task DLL when present. Test, DebugHost, and optional benchmark projects are not packed.
<!--#if (NuGetAuditHighCriticalAsErrors) -->

Restore fails this task package on high (`NU1903`) and critical (`NU1904`) vulnerable packages. Low and moderate stay warnings. `NugetReport` next to the tests lists that package’s packages (txt/json) and is still info-only.
<!--#endif -->
<!--#if (PublicApiAnalyzers) -->

## Public API baseline

The task library uses `Microsoft.CodeAnalysis.PublicApiAnalyzers`. The first real `dotnet build` writes `src/prj/__SourceName__/Properties/PublicAPI/PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt` if they are missing, then records the current public surface. Commit those files. Later public additions belong in `PublicAPI.Unshipped.txt` (analyzer RS0016 / `dotnet format analyzers` with `--diagnostics RS0016`).
<!--#endif -->
<!--#if (WritePackageDocTemplate) -->

## Package documentation template

`src/prj/__SourceName__/Properties/NugetMetadata/docs/DocShell.html` is the offline documentation seed. It packs with the nupkg (`docs/` inside the package). Bootstrap the site from that file (vendor the local css/js/licenses next to it), then write package documentation for this library. Repository-root `docs/` is a separate site if present.
<!--#endif -->

## Publish

The task package sets `IsPublishable` to `false`. Distribution is `dotnet pack`. Consumers install the nupkg; they do not publish this project.

## CI

Use `-m:1` for the build so a pipeline does not depend on machine load. It avoids occasional file locks when the library is built as a solution project and as a test `ProjectReference` at the same time. Run these commands from this folder so each library has exactly one `.slnx` in the working directory.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet test --no-build
dotnet pack
```
<!--#if (BenchmarkProject == true) -->

## Benchmarks

The benchmark is a local executable targeting the selected DebugHost framework. It is neither packed nor published; start it with the command below.

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

To break in the MSBuild task:

1. Set `__SourceName__.DebugHost` as the startup project (not the task library).
2. Select the `__SourceName__.DebugHost` launch profile (Executable). Visual Studio cannot start a class library without that profile.
3. Set a breakpoint in `AddTask.Execute`.
4. Press F5. Visual Studio builds the task first, then starts a **separate** `dotnet msbuild` process with `-p:BuildProjectReferences=false -t:RunDebugHostTask -nodeReuse:false`, so MSBuild loads the already-built DLL.

A normal `dotnet build` of the solution does not run `RunDebugHostTask`. Do not use Microsoft.Build.Locator or an in-process `BuildManager`.

The nupkg ships `build/__SourceName__.props` and `build/__SourceName__.targets`. NuGet auto-imports both into every PackageReference consumer csproj (and that project's `Directory.Build.props` / `Directory.Build.targets`). The `.props` file (imported first) registers fully qualified `UsingTask` entries for `__SourceName__.AddTask`, `__SourceName__.TaskNodeTask`, `__SourceName__.HomeTask`, and `__SourceName__.DumpEnvVarsTask` pointing at `tasks/netstandard2.0/`. The `.targets` file (imported after the project) runs `HomeTask` / `TaskNodeTask` before `CoreCompile` so the consumer gets `$(HomeDirectory)`, `$(TaskNodeDirectory)`, and `$(TaskNodeLocation)`. `DumpEnvVarsTask` and `AddTask` stay opt-in: invoke them from a consumer target or `Directory.Build.targets`. DebugHost does not import those nupkg files: F5 loads `bin/$(Configuration)/netstandard2.0/__SourceName__.dll` from the in-repo build output.

For stepping without F5, debug `FunctionalTests` (in-process `Execute`) or `IntegrationTests` (`Resources/TestScript.msbuild`) from Test Explorer.

`DebugHost` is not packed. `dotnet pack` still produces only the task nupkg.
