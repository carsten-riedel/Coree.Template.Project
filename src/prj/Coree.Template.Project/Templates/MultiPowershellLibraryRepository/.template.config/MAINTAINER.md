# `.template.config` (MultiPowershellLibraryRepository)

Product surface: **multi-project repository for compiled PowerShell modules**. Identity: `CoreeTemplatesProjectMultiPowershellLibraryRepository`; CLI short name: `multipowershellrepo-coree`.

This template keeps the repository-composition UX of the other multi templates. Repeated creates into one output add another independently versioned module without restamping repository-root files.

## PowerShell target UX

`PowerShellTargets` is one multi-choice. Choice values are TFMs only because the join generator can write them directly into `TargetFrameworks`; display names are PowerShell hosts so users do not have to perform the mapping.

| UI choice | Value |
| --- | --- |
| Windows PowerShell 5.1 - broader compatibility (.NET Framework 4.6.2) | `net462` |
| PowerShell 7.4+ - broader compatibility (.NET 8) | `net8.0` |
| Windows PowerShell 5.1 (.NET Framework 4.8) | `net48` |
| PowerShell 7.6 (.NET 10) | `net10.0` |
| Cross-edition portable (.NET Standard 2.0) | `netstandard2.0` |

Default: `net462|net8.0`. `template.json` keeps `PowerShellTargets` `defaultValue` empty, like `InitRepoItems`. Visual Studio does not bind a pipe default in `template.json` to checkboxes: it shows the display names and leaves every box unchecked. The Visual Studio selection is `ide.host.json` `defaultValue`. CLI omit (and a VS create with no bound checkbox) sets `UseDefaultPowerShellTargets` and still writes `net462;net8.0`. Choice order keeps those two values as a contiguous prefix so a first-choice snap cannot land on portable. Choice `displayName` must not contain `|` or `,` (both are multi-choice separators; Visual Studio also joins the combo text with commas). The .NET Framework 4.6.2 binary is the broader Windows PowerShell 5.1 choice and remains loadable on .NET Framework 4.8; `net48` is opt-in for modules that need its newer API surface. The .NET 8 binary is the broader PowerShell 7 choice and is also loadable by PowerShell 7.6; `net10.0` is opt-in for modules that need its newer API surface. `netstandard2.0` is a deliberate one-binary alternative using `PowerShellStandard.Library`, not a third host family. Selecting redundant targets is allowed because `dotnet new` multi-choice parameters do not provide useful conflict validation; descriptions must keep the trade-off explicit. Keep `PowerShellTargets` explicitly optional (`isRequired: false`): Visual Studio otherwise treats the field as user-required while the displayed default is not a selected value.

`__TestTargetFramework__` resolves to the newest selected Core TFM, otherwise the selected Windows TFM, otherwise `net8.0` for a portable module. `src/global.json` pins SDK 10 only when `net10.0` is selected; all other combinations pin SDK 8.

The source model has two native host families, Desktop (`net462`, `net48`) and Core (`net8.0`, `net10.0`). `netstandard2.0` is their shared PowerShell Standard surface. Keep one multi-target module project and shared source files by default. Do not scaffold empty host-specific files or a second module project. Small real divergences can use `NETFRAMEWORK`, `NET8_0_OR_GREATER`, or `NET10_0_OR_GREATER`; larger divergences can use named folders plus conditional `Compile Remove` rules, because the SDK default compile glob already includes every `.cs` file.

## Project responsibilities

- `src/prj/{Name}` owns cmdlets, host references, manifest, loader, staging, and eventually Gallery packaging.
- `src/prj/{Name}.Tests` owns C# unit seams plus external-process import tests against installed real PowerShell hosts.
- `src/prj/{Name}.DebugHost` owns only Visual Studio F5 launch profiles for `powershell.exe` and `pwsh.exe`. It must not acquire module logic or packaging.
- The optional benchmark measures host-independent core logic. It is not a PowerShell host test.

The scaffold cmdlet adapter is marked `ExcludeFromCodeCoverage`; the external host tests exercise it, while the 100% Coverlet gate applies to the host-independent formatter. Product code may keep or change that boundary deliberately.

The DebugHost is intentionally separate from tests. F5 invokes the outer module build through its own MSBuild target, launches the real host, waits until the debugger is attached, imports the staged manifest, and calls the sample cmdlet. The default launch profile is PowerShell 7 whenever Core is selected: DebugHost then targets a Core TFM (`__TestTargetFramework__`), which matches `pwsh.exe`. The Windows PowerShell 5.1 profile still starts `powershell.exe` (.NET Framework); Visual Studio debugging a Core DebugHost does not bind managed breakpoints in that process. It must not use a `ProjectReference`: Visual Studio asks an outer cross-targeting reference for `GetTargetPath`, and a normal compatible reference would build only one selected binary. `DisableFastUpToDateCheck` ensures the empty launcher still runs this build on every F5. This prevents the old pattern where a test project also acted as an informal debug launcher.

## Module layout

`Properties/ModuleAssets/` is the source package root:

- `{Name}.psd1` is the Gallery manifest.
- `{Name}.psm1` selects the best compatible binary for the current host.
- `Readme.md`, `ReleaseNotes.txt`, `License.txt`, optional icon, and optional docs belong to the module distribution.

`Properties/Build/StagePowerShellModule.targets` stages a complete importable tree under `bin/Module/<Configuration>/{Name}/`, with one subfolder per selected TFM. It copies only the module assembly automatically. Product runtime dependencies must be added explicitly as `PowerShellModuleDependency`; PowerShell host assemblies must never be copied into the module.

The class-library NuGet pack/publish override, Web SDK option, and generic package-boundary guide were removed because they model the wrong product. A final packaging pass still needs a repeatable `Publish-PSResource` flow that creates a Gallery-compatible `.nupkg` from the staged Release module. Do not reintroduce normal SDK `dotnet pack` output as the distributable artifact.

## Template mechanics

- `InitRepoItems` defaults empty on CLI and to the five first-create root items in Visual Studio. `None` is the explicit Visual Studio empty set for later modules.
- `InitAllRepoItems` is CLI-only and writes README, release checkpoint, `.gitattributes`, `.gitignore`, and LICENSE.
- `PlaceSolution` retains `SlnFolder`, `RepoRoot`, and `BesideCsproj` behavior from the sibling multi templates.
- `specialCustomOperations` for `.targets` must remain because those files can contain generate-time branches. The `.psd1` operation is required for author/company copyright branches; the launch-settings operation removes profiles for unselected editions. Do not remove operations solely because the current sample looks simple.
- `KeepScaffoldUnusedUsings` does not belong here. Real module source has only required usings; implicit-usings behavior remains controlled by `CSharpProjectOptions`.

Local install and smoke generation:

```powershell
dotnet new install "C:\dev\github.com\carsten-riedel\Coree.Template.Project\src\prj\Coree.Template.Project\Templates\MultiPowershellLibraryRepository" --force
dotnet new multipowershellrepo-coree --Author "Example" --name "Organization.Domain.PowerShell" --output $out --InitAllRepoItems
```

For a second module in the same output, omit `--InitAllRepoItems` and all root-item choices.
