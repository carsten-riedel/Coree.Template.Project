# __SourceName__

.NET multi-analyzer repository. The GitHub landing page is this file.
<!--#if (PlaceSolution == "SlnFolder") -->
Each package's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this package start in that folder.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This package's `.slnx` and notes sit next to the packable analyzer under `src/prj/__SourceName__/`. There is no `src/sln/` tree for this package. CLI commands start in `src/prj/__SourceName__/`.
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
src/global.json            .NET SDK pin (DebugHost TFM)
<!--#endif -->
<!--#if (PlaceSolution == "SlnFolder") -->
src/sln/__SourceName__/      this package's .slnx and notes
<!--#elseif (PlaceSolution == "BesideCsproj") -->
src/prj/__SourceName__/      packable analyzer package (netstandard2.0), .slnx, and notes
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
__SourceName__.generated.slnx  solution
<!--#else -->
__SourceName__.slnx          solution
<!--#endif -->
src/sln/__SourceName__/      optional notes / cross-project files
<!--#endif -->
<!--#if (PlaceSolution != "BesideCsproj") -->
src/prj/__SourceName__/      packable analyzer package (netstandard2.0)
<!--#endif -->
src/prj/__SourceName__/AnalyzerPackage/  consumer MSBuild props (packed to build/ and buildTransitive/)
src/prj/__SourceName__/Properties/Build/  MSBuild targets (not source)
src/prj/__SourceName__/Properties/NugetMetadata/  NuGet metadata (readme, icon, notes)
<!--#if (DotNetToolManifest) -->
src/prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
src/prj/__SourceName__.Tests/  tests (not packed)
src/prj/__SourceName__.DebugHost/  Visual Studio F5 compile target (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
Open a terminal in `src/prj/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution next to the analyzer project; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
<!--#if (WriteSrcGlobalJson) -->
`src/global.json` does not apply to those commands: the SDK muxer starts at the repository root and does not walk into `src/`.
<!--#endif -->
<!--#endif -->

<!--#if (WriteSrcGlobalJson) -->
`src/global.json` pins the .NET SDK to the selected DebugHost target framework (`rollForward: latestFeature`). `dotnet` finds it when the working directory is under `src/`. The analyzer package stays netstandard2.0.
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
