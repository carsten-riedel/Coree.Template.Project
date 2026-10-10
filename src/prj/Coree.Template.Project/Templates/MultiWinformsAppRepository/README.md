# __SourceName__

.NET multi-WinForms repository. The GitHub landing page is this file.
<!--#if (PlaceSolution == "SlnFolder") -->
Each app's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this app start in that folder.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This app's `.slnx` and notes sit next to the WinForms project under `src/prj/__SourceName__/`. There is no `src/sln/` tree for this app. CLI commands start in `src/prj/__SourceName__/`.
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
src/prj/__SourceName__/      WinForms app, .slnx, and notes
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
__SourceName__.generated.slnx  solution
<!--#else -->
__SourceName__.slnx          solution
<!--#endif -->
src/sln/__SourceName__/      optional notes / cross-project files
<!--#endif -->
<!--#if (PlaceSolution != "BesideCsproj") -->
src/prj/__SourceName__/      WinForms app
<!--#endif -->
src/prj/__SourceName__/Properties/Build/  MSBuild targets (not source)
<!--#if (DotNetToolManifest) -->
src/prj/__SourceName__/.config/dotnet-tools.json  project-scoped .NET tool manifest
<!--#endif -->
src/prj/__SourceName__.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/__SourceName__.Benchmark/  optional BenchmarkDotNet runner
<!--#endif -->
```

## Program sample

<!--#if (ProgramSample == "Basic") -->
This app uses `Basic`, the small Visual Studio-style `Form1` skeleton. It is the default `--ProgramSample`.
<!--#else -->
This app uses `GenericHost`: a fixed Generic Host setup with DI, logging, reloadable `appsettings.json`, English/German/Spanish/French/Italian/Portuguese/Simplified-Chinese localization, profile optimization, and single-instance protection. These capabilities are intentionally one cohesive sample rather than separate feature switches.
The sample `Settings:Subkey1:Value1` contains the `AppSettingsWindowTitle` resource key, so the visible title follows the process UI culture. Replacing it with a literal value still takes precedence.
<!--#endif -->

`--ProgramSample` is the maintained replacement for the older standalone `winforms-coree` and `winformsdi-coree` sample layouts. Those legacy templates remain available and unchanged.

## Optional WiX user installer

Use `--AdditionalInstaller WixUserInstaller` to add a separate Windows per-user WiX project. It packages the normal app publish output and writes `src/prj/__SourceName__/bin/Setup/__SourceName__.msi`; the technical project is named `__SourceName__.WixSetup`. The default `None` choice leaves the repository unchanged. Visual Studio requires the WiX Toolset HeatWave extension installed; the `dotnet` CLI uses the WiX SDK package.

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet publish`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
Open a terminal in `src/prj/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet publish`. The CLI finds the one solution next to the WinForms app project; you do not pass a `.slnx` or `.csproj` path.
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
`src/prj/__SourceName__/.config/dotnet-tools.json` provides project-scoped .NET tools.
<!--#if (UseNerdbankGitVersioning) -->
Run these commands from the main project folder:

```powershell
dotnet tool restore
<!--#if (NerdbankGitVersioning == "Project") -->
dotnet nbgv get-version -p Properties -v NuGetPackageVersion
<!--#elseif (NerdbankGitVersioning == "Repo") -->
dotnet nbgv get-version -v NuGetPackageVersion
<!--#endif -->
```

The manifest pins `nbgv` to the same version as the NerdBank package reference. Runtime roll-forward lets nbgv use a newer installed .NET runtime when .NET 8 is absent. Tool restore is an explicit step; build does not restore local tools.
<!--#else -->
The manifest starts empty. Add your own project-local tools with `dotnet tool install --local <package-id>`.
<!--#endif -->
<!--#if (PlaceSolution == "SlnFolder") -->
The default solution-folder working directory does not see this file.
<!--#endif -->
<!--#endif -->
<!--#if (PlaceSolution == "BesideCsproj") -->
Command details and layout notes: [src/prj/__SourceName__/Readme.md](src/prj/__SourceName__/Readme.md).
<!--#else -->
Command details and layout notes: [src/sln/__SourceName__/Readme.md](src/sln/__SourceName__/Readme.md).
<!--#endif -->
