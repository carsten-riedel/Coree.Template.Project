# __SourceName__

Binary PowerShell module repository. Each template invocation adds one independently versioned module, its tests, and a separate Visual Studio debug host.

The target selector uses PowerShell names while the generated project uses the matching TFM:

| Selection | Build target | Intended host |
| --- | --- | --- |
| Windows PowerShell 5.1 - broader compatibility (.NET Framework 4.6.2) | `net462` | Windows PowerShell 5.1 on older supported .NET Framework installations |
| PowerShell 7.4+ - broader compatibility (.NET 8) | `net8.0` | PowerShell 7.4 and newer, including PowerShell 7.6 |
| Windows PowerShell 5.1 (.NET Framework 4.8) | `net48` | Windows PowerShell 5.1 |
| PowerShell 7.6 (.NET 10) | `net10.0` | PowerShell 7.6 when newer PowerShell or .NET APIs are required |
| Cross-edition portable (.NET Standard 2.0) | `netstandard2.0` | Windows PowerShell 5.1 and PowerShell 7 with the shared PowerShellStandard API |

The default is `Windows PowerShell 5.1 - broader compatibility (.NET Framework 4.6.2)` plus `PowerShell 7.4+ - broader compatibility (.NET 8)`. .NET Framework 4.8 can load the .NET Framework 4.6.2 binary, and PowerShell 7.6 can load the .NET 8 binary. Select `.NET Framework 4.8` or `.NET 10` only when the module needs their newer APIs. Select `Cross-edition portable (.NET Standard 2.0)` instead when the smaller shared API is enough and one binary is preferable.

The targets belong to two native host families: Desktop (`net462`, `net48`) and Core (`net8.0`, `net10.0`). `netstandard2.0` represents their shared PowerShell Standard surface, not a third host family. The scaffold therefore keeps one multi-target project and compiles the same cmdlets for every selected target. Add conditional code or host-specific files only when the module actually uses APIs that differ between those families or target versions.

<!--#if (PlaceSolution == "SlnFolder") -->
This module's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this module start in that folder.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This module's `.slnx` and notes sit next to the module project under `src/prj/__SourceName__/`.
<!--#else -->
The solution lives at the repository root; projects live under `src/`. If several `.slnx` files share the root, pass the solution path to `dotnet`.
<!--#endif -->

```text
./                                      repository root
<!--#if (WriteRepoVersionJson) -->
version.json                            repository-wide Nerdbank.GitVersioning
<!--#endif -->
<!--#if (WriteRepoDocTemplate) -->
docs/                                   repository documentation seed
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
src/global.json                         SDK pin required by the selected PowerShell targets
<!--#endif -->
src/prj/__SourceName__/                 binary module project and cmdlets
src/prj/__SourceName__/Properties/NugetMetadata/  NuGet/module metadata plus module files (readme, icon, notes, manifest, loader)
src/prj/__SourceName__.Tests/           unit tests plus real PowerShell host tests
src/prj/__SourceName__.DebugHost/       F5 profiles for powershell.exe and pwsh.exe
<!--#if (BenchmarkProject == true) -->
src/prj/__SourceName__.Benchmark/       optional host-independent core benchmark
<!--#endif -->
```

Run `dotnet build` to compile every selected target. The build stages an importable module at:

```text
src/prj/__SourceName__/bin/Module/<Configuration>/__SourceName__/
```

The root `__SourceName__.psm1` selects the best compatible DLL for the current PowerShell host. `dotnet pack -c Release` writes a PowerShell Gallery nupkg under `src/prj/__SourceName__/bin/Pack/` from that staged tree (manifest at the package root, not `lib/`). `dotnet test` keeps fast C# unit coverage and also imports the staged module in installed `powershell.exe` and `pwsh.exe` hosts. A missing host or an unselected edition is reported as an inconclusive host test, not a false failure.

For F5 debugging, set `__SourceName__.DebugHost` as the startup project. The default profile is `PowerShell 7` whenever that host is in the create (DebugHost is then a Core TFM, matching `pwsh.exe`). `Windows PowerShell 5.1` launches `powershell.exe`; Visual Studio's Core debugger does not bind cmdlet breakpoints there. The host waits until the debugger is attached, then imports the staged manifest and calls `Get-SampleValue`.

<!--#if (WriteSrcGlobalJson) -->
`src/global.json` pins .NET 10 when PowerShell 7.6 is selected and .NET 8 otherwise (`rollForward: latestFeature`). It applies to commands whose working directory is under `src/`.
<!--#endif -->
<!--#if (DotNetToolManifest) -->
`src/prj/__SourceName__/.config/dotnet-tools.json` is an empty local tool manifest for module-local build tooling.
<!--#endif -->
<!--#if (PlaceSolution == "BesideCsproj") -->
Additional command and layout notes: [src/prj/__SourceName__/Readme.md](src/prj/__SourceName__/Readme.md).
<!--#else -->
Additional command and layout notes: [src/sln/__SourceName__/Readme.md](src/sln/__SourceName__/Readme.md).
<!--#endif -->
