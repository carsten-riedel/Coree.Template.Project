# __SourceName__

.NET multi-console repository. The GitHub landing page is this file.

## Optional WiX user installer

Use `--AdditionalInstaller WixUserInstaller` to add a separate Windows per-user WiX project. It packages the normal app publish output and writes `src/prj/__SourceName__/bin/Setup/__SourceName__.msi`; the technical project is named `__SourceName__.WixSetup`. The default `None` choice leaves the repository unchanged. Visual Studio requires the WiX Toolset HeatWave extension installed; the `dotnet` CLI uses the WiX SDK package.
The Windows executable uses `src/prj/__SourceName__/Properties/AppIcon.ico`. With `WixUserInstaller`, the separate installer icon at `src/prj/__SourceName__.WixSetup/SetupAssets/Icon.ico` appears in Windows Installed apps. Replace either file independently for your app. The installer displays the selected project license from `SetupAssets/License.rtf`. Review that file before distribution, especially when `ProjectLicense` is `Custom`: its generated text contains only a copyright notice until you add your terms.
With `WixUserInstaller`, the generated `.wixproj` controls three installer choices: run the app after installation, add a desktop shortcut, and add the install folder to the user's `PATH`. Each `Offer...` property includes or removes its choice; its matching `...SelectedByDefault` property sets the initial selection. By default, only the user `PATH` choice is offered and initially selected. The other two choices are disabled but can be enabled in the `.wixproj`. Desktop shortcut and user `PATH` use WiX's feature selection page; the run choice appears on the final dialog when enabled.
<!--#if (PlaceSolution == "SlnFolder") -->
Each app's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this app start in that folder.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This app's `.slnx` and notes sit next to the console project under `src/prj/__SourceName__/`. There is no `src/sln/` tree for this app. CLI commands start in `src/prj/__SourceName__/`.
<!--#else -->
The solution lives at the repository root; projects live under `src/`. `src/sln/__SourceName__/` still exists for solution-level or cross-project files; delete that notes folder if you do not need it. If this repository has more than one `.slnx` in the root directory, pass the solution path to `dotnet`.
<!--#endif -->

```text
./                         repository root (this README)
<!--#if (WriteRepoVersionJson) -->
version.json               Nerdbank.GitVersioning (this repository)
<!--#endif -->
<!--#if (WriteRepoDocTemplate) -->
docs/                      offline documentation template (this repository)
<!--#endif -->
<!--#if (WriteSrcGlobalJson) -->
src/global.json            .NET SDK pin (highest selected TFM)
<!--#endif -->
<!--#if (PlaceSolution == "SlnFolder") -->
src/sln/__SourceName__/      this app's .slnx and notes
<!--#elseif (PlaceSolution == "BesideCsproj") -->
src/prj/__SourceName__/      console app, .slnx, and notes
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
__SourceName__.generated.slnx  solution
<!--#else -->
__SourceName__.slnx          solution
<!--#endif -->
src/sln/__SourceName__/      optional notes / cross-project files
<!--#endif -->
<!--#if (PlaceSolution != "BesideCsproj") -->
src/prj/__SourceName__/      console app
<!--#endif -->
src/prj/__SourceName__/Properties/Build/  MSBuild targets (not source)
src/prj/__SourceName__/Properties/PublishAssets/  files copied beside the published app
src/prj/__SourceName__/Properties/AppIcon.ico  embedded Windows executable icon
<!--#if (AdditionalInstaller == "WixUserInstaller") -->
src/prj/__SourceName__.WixSetup/SetupAssets/  installer-only license RTF and icon
<!--#endif -->
<!--#if (PackAsNuGetTool) -->
src/prj/__SourceName__/Properties/NugetMetadata/  NuGet metadata (readme, icon, notes)
<!--#endif -->
<!--#if (DotNetToolManifest) -->
src/prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
src/prj/__SourceName__.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet publish`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
Open a terminal in `src/prj/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet publish`. The CLI finds the one solution next to the console app project; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet publish`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
<!--#if (WriteSrcGlobalJson) -->
`src/global.json` does not apply to those commands: the SDK muxer starts at the repository root and does not walk into `src/`.
<!--#endif -->
<!--#endif -->

<!--#if (WriteSrcGlobalJson) -->
`src/global.json` pins the .NET SDK to the highest selected target framework (`rollForward: latestFeature`). `dotnet` finds it when the working directory is under `src/`.
<!--#endif -->
<!--#if (DotNetToolManifest) -->
`src/prj/__SourceName__/.config/dotnet-tools.json` is an empty local tool manifest. Run `dotnet tool install --local` from that project folder.
<!--#if (PlaceSolution == "SlnFolder") -->
The default solution-folder working directory does not see this file.
<!--#endif -->
<!--#endif -->
<!--#if (PlaceSolution == "BesideCsproj") -->
Command details and layout notes: [src/prj/__SourceName__/Readme.md](src/prj/__SourceName__/Readme.md).
<!--#else -->
Command details and layout notes: [src/sln/__SourceName__/Readme.md](src/sln/__SourceName__/Readme.md).
<!--#endif -->
