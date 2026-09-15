# AI-supported release checkpoint

Complete this once before the first PowerShell Gallery publish, then delete it.

1. The root README and each module readme describe the actual commands, supported PowerShell hosts, and installation path.
2. Every selected target is intentional. Do not ship redundant binaries merely because the template offered them; prefer either the portable cross-edition build or the host-specific builds needed by the product.
3. The staged module imports successfully in every advertised host (`powershell.exe` and/or the matching `pwsh` release), and its exported commands behave as documented.
4. `Properties/ModuleAssets/__SourceName__.psd1` has accurate description, tags, project URI, license URI, prerelease metadata, exports, compatible editions, and minimum PowerShell version.
5. Manifest `ModuleVersion`, assembly version, Nerdbank.GitVersioning output, release notes, and the eventual Gallery package version agree.
6. `Properties/ModuleAssets/License.txt`, the optional repository-root `LICENSE`, and published license metadata agree. Replace the scaffold icon if it should not ship.
7. Remove or replace `Get-SampleValue`, its formatter, tests, benchmark, and debug invocation with real product behavior.
8. The repeatable package command stages a clean Release module and produces a PowerShell Gallery-compatible `.nupkg` through `Publish-PSResource`; it must not publish a normal class-library NuGet layout.
9. Inspect the produced `.nupkg`: manifest and loader are at the module root, only selected target folders exist, host assemblies such as `System.Management.Automation.dll` are absent, and required third-party runtime dependencies are present.
10. If module or repository documentation seeds exist, replace them with useful product documentation at the same ownership boundary.
11. Delete this checkpoint.
