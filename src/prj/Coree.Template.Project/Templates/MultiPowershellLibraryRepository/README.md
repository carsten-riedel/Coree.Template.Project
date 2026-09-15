# __SourceName__

Binary PowerShell module repository. Each template invocation adds one independently versioned module, its tests, and a separate Visual Studio debug host.

The target selector uses PowerShell names while the generated project uses the matching TFM:

| Selection | Build target | Intended host |
| --- | --- | --- |
| Cross-edition portable (.NET Standard 2.0) | `netstandard2.0` | Windows PowerShell 5.1 and PowerShell 7 with the shared PowerShellStandard API |
| Windows PowerShell 5.1, broader compatibility (.NET Framework 4.6.2) | `net462` | Windows PowerShell 5.1 on older supported .NET Framework installations |
| Windows PowerShell 5.1 (.NET Framework 4.8) | `net48` | Windows PowerShell 5.1 |
| PowerShell 7.4+, broader compatibility (.NET 8) | `net8.0` | PowerShell 7.4 and newer, including PowerShell 7.6 |
| PowerShell 7.6 (.NET 10) | `net10.0` | PowerShell 7.6 when newer PowerShell or .NET APIs are required |

The default is `Windows PowerShell 5.1 (.NET Framework 4.8)` plus `PowerShell 7.4+, broader compatibility (.NET 8)`. PowerShell 7.6 can load that .NET 8 binary. Select its `.NET 10` target only when the module needs newer APIs. Select `Cross-edition portable (.NET Standard 2.0)` instead when the smaller shared API is enough and one binary is preferable.

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
src/prj/__SourceName__/Properties/ModuleAssets/
                                        manifest, loader, license, readme, and release notes
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

The root `__SourceName__.psm1` selects the best compatible DLL for the current PowerShell host. `dotnet test` keeps fast C# unit coverage and also imports the staged module in installed `powershell.exe` and `pwsh.exe` hosts. A missing host or an unselected edition is reported as an inconclusive host test, not a false failure.

For F5 debugging, set `__SourceName__.DebugHost` as the startup project and choose either `Windows PowerShell 5.1` or `PowerShell 7`. The selected executable imports the same staged module used by integration tests, so breakpoints in compiled cmdlets exercise the real host path.

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
