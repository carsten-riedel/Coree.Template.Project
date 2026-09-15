# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder contains the module solution and solution-level files. Open a terminal here for the commands below.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This readme and the solution sit beside the binary module project. Tests, DebugHost, and the optional benchmark remain sibling projects.
<!--#else -->
The solution is at the repository root. This folder is available for module-wide notes or other solution items.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "SlnFolder")) -->

Visual Studio also created a conventional root solution. Close it, open `src/sln/__SourceName__/__SourceName__.slnx`, and remove the extra root solution if it is not needed.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "BesideCsproj")) -->

Visual Studio also created a conventional root solution. Close it, open `src/prj/__SourceName__/__SourceName__.slnx`, and remove the extra root solution if it is not needed.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "RepoRoot")) -->

Open `__SourceName__.generated.slnx` for the generated multi-project layout. Remove Visual Studio's extra conventional root solution if it is not needed.
<!--#endif -->

## Build and stage

```bash
dotnet restore
dotnet build
```

The module project builds every selected PowerShell target and stages one importable module tree under:

```text
src/prj/__SourceName__/bin/Module/<Configuration>/__SourceName__/
```

The root manifest loads `__SourceName__.psm1`; that loader chooses a compatible DLL in `net48`, `net462`, `net10.0`, `net8.0`, or `netstandard2.0` according to the current PowerShell host and the binaries selected at template creation.

## Shared and host-specific code

There are two native host families: Desktop (`net462`, `net48`) and Core (`net8.0`, `net10.0`). The `netstandard2.0` target is their shared PowerShell Standard surface, not a third family. All source files compile for every selected target by default, which is appropriate while the cmdlets stay within their common API surface.

For a small host-specific branch, use the SDK-defined `NETFRAMEWORK`, `NET8_0_OR_GREATER`, or `NET10_0_OR_GREATER` compile symbol in the existing file. When those branches become substantial, move them into clearly named host-specific files and exclude those files conditionally by `TargetFramework` in the project. The SDK already includes all `.cs` files by default, so use conditional `Compile Remove` rules rather than adding duplicate conditional `Compile Include` items. Keep the implementation in this one multi-target project unless the module develops an independently useful component with a genuinely separate responsibility.

## Test

```bash
dotnet test
```

The test project uses one runnable TFM. It keeps a fast in-process unit seam for host-independent logic and separately imports the staged module in the real `powershell.exe` and `pwsh.exe` processes. A host test is inconclusive when that host is unavailable or no compatible module target was selected.

<!--#if (PlaceSolution == "SlnFolder") -->
[Test results (trx)](../../prj/__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TestTargetFramework__.trx)
[Test results (html)](../../prj/__SourceName__.Tests/MSTestResults/result-__TestTargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../../prj/__SourceName__.Tests/CoverletOutput/coverage.__TestTargetFramework__.opencover.xml)
<!--#endif -->
<!--#else -->
[Test results (trx)](../__SourceName__.Tests/MSTestResults/__SourceName__.Tests-__TestTargetFramework__.trx)
[Test results (html)](../__SourceName__.Tests/MSTestResults/result-__TestTargetFramework__.html)
<!--#if (CoverletMSBuild == true) -->
[Coverlet output](../__SourceName__.Tests/CoverletOutput/coverage.__TestTargetFramework__.opencover.xml)
<!--#endif -->
<!--#endif -->

<!--#if (CoverletMSBuild == true) -->
Coverlet measures host-independent module logic in process. The sample cmdlet adapter is excluded from Coverlet because its behavior is exercised by the real-host import tests outside the test process.
<!--#endif -->

## Debug in Visual Studio

Set `__SourceName__.DebugHost` as the startup project. Choose the `Windows PowerShell 5.1` or `PowerShell 7` launch profile. F5 first builds and stages the module, then starts the real host and imports that staged manifest. Breakpoints in `GetSampleValueCommand` therefore run in the same kind of process as consumers.

The DebugHost owns no module logic and is never packaged.

<!--#if (NuGetAuditHighCriticalAsErrors) -->
Restore fails the module project on high (`NU1903`) and critical (`NU1904`) vulnerable dependencies. Low and moderate findings remain warnings.
<!--#endif -->
<!--#if (PublicApiAnalyzers) -->

## Public API baseline

The module assembly uses `Microsoft.CodeAnalysis.PublicApiAnalyzers`. The first real build writes `Properties/PublicAPI/PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt`. Commit the baseline and maintain it with public binary changes.
<!--#endif -->
<!--#if (WritePackageDocTemplate) -->

## Module documentation seed

`src/prj/__SourceName__/Properties/ModuleAssets/docs/DocShell.html` stages with this module. Replace the seed with documentation for the exported commands and supported PowerShell hosts.
<!--#endif -->
<!--#if (BenchmarkProject == true) -->

## Benchmarks

The optional BenchmarkDotNet project measures host-independent module core logic; it is not a substitute for real-host tests.

```bash
<!--#if (PlaceSolution == "SlnFolder") -->
dotnet run --project ../../prj/__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#else -->
dotnet run --project ../__SourceName__.Benchmark/__SourceName__.Benchmark.csproj -c Release
<!--#endif -->
```
<!--#endif -->
