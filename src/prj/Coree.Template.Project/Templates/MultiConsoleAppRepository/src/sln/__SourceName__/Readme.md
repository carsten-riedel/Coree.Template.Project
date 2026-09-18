# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder is the per-app area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` lives here; keep the folder while that is true. You can still add extra solution items here.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This folder is the console app. The `.slnx` and this readme sit next to the `.csproj`. Tests and the optional benchmark stay sibling projects under `src/prj/`. There is no `src/sln/` tree for this app.
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
The `.slnx` and this readme live in this folder next to the console app. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other apps keep their own `.slnx` under `src/prj/<name>/`, so `dotnet` does not ask you to specify a solution.
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
../../prj/__SourceName__/    console app
<!--#if (DirectoryMsBuildFiles) -->
../../prj/__SourceName__/Directory.Build.props  optional empty console app MSBuild landing file
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
./Directory.Build.props     optional empty console app MSBuild landing file
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
<!--#if (PackAsNuGetTool) -->

Package metadata, license, icon, and release notes live in `src/prj/__SourceName__/Properties/NugetMetadata/`.
<!--#endif -->

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

After a test run, the links below point to generated reports. Each selected target framework writes its own files (`net8.0`, `net10.0`, …).

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
Coverlet measures only the console app (`[__SourceName__]*`) and fails `dotnet test` if line, branch, or method coverage is under 100%.
<!--#endif -->
<!--#if (ReportGenerator == true) -->
<!--#if (PlaceSolution == "SlnFolder") -->
ReportGenerator writes one summary per target framework under `../../prj/__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#else -->
ReportGenerator writes one summary per target framework under `../__SourceName__.Tests/ReportGeneratorOutput/<TFM>/SummaryGithub.md` (for example, `.../ReportGeneratorOutput/net10.0/SummaryGithub.md`).
<!--#endif -->
<!--#endif -->

## Publish

```bash
dotnet publish
```

`dotnet publish` with no `--framework` publishes the default TFM from `src/prj/__SourceName__/Properties/Build/PublishDefaultFramework.targets` to `src/prj/__SourceName__/bin/Publish/`. Override with `dotnet publish --framework net8.0` (or another selected TFM). Edit that targets file to change the default. To try a newer TFM (net11, …) before shipping it, append it to `TargetFrameworks` so build/test catch incompatibilities; leave the targets file until bare publish should follow. RID and the publish recipe (self-contained, single-file, …) are generate-time choices and apply only while publishing. Test and optional benchmark projects are not published.
<!--#if (NuGetAuditHighCriticalAsErrors) -->

Restore fails this console app on high (`NU1903`) and critical (`NU1904`) vulnerable packages. Low and moderate stay warnings. `NugetReport` next to the tests lists that app’s packages (txt/json) and is still info-only.
<!--#endif -->
<!--#if (WritePackageDocTemplate) -->

## Package documentation template

`src/prj/__SourceName__/Properties/NugetMetadata/docs/DocShell.html` is the offline documentation seed. It packs with the nupkg (`docs/` inside the package). Bootstrap the site from that file (vendor the local css/js/licenses next to it), then write package documentation for this app. Repository-root `docs/` is a separate site if present.
<!--#endif -->
<!--#if (PackAsNuGetTool) -->

## Additional pack as NuGet tool

`dotnet pack` writes a tool nupkg to `src/prj/__SourceName__/bin/Pack/`. Install from that folder (or nuget.org after publish). The command after install is `ToolCommandName` in the console app csproj (the project name). Change that property, or pass `-p:ToolCommandName=...` on pack. Publish remains available; it is a different output than the tool nupkg.

```bash
dotnet pack
```

Test and optional benchmark projects are not packed.
<!--#else -->

## Pack

`IsPackable` is `false`. Distribution is `dotnet publish`. To write a `.nupkg` to `src/prj/__SourceName__/bin/Pack/` for all selected target frameworks, set `IsPackable` to `true` and run:

```bash
dotnet pack
```

Test and optional benchmark projects are not packed.
<!--#endif -->

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
