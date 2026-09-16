# __SourceName__

.NET multi-project template repository. This template is currently a structural copy of the multi-library repository; project-template-specific adaptation follows.
<!--#if (PlaceSolution == "SlnFolder") -->
Each library's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this library start in that folder.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This library's `.slnx` and notes sit next to the packable project under `src/prj/__SourceName__/`. There is no `src/sln/` tree for this library. CLI commands start in `src/prj/__SourceName__/`.
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
<!--#if (PlaceSolution == "SlnFolder") -->
src/sln/__SourceName__/      this library's .slnx and notes
<!--#elseif (PlaceSolution == "BesideCsproj") -->
src/prj/__SourceName__/      packable project-template package, .slnx, and notes
src/prj/__SourceName__/Templates/  project-template folders
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
__SourceName__.generated.slnx  solution
<!--#else -->
__SourceName__.slnx          solution
<!--#endif -->
src/sln/__SourceName__/      optional notes / cross-project files
<!--#endif -->
<!--#if (PlaceSolution != "BesideCsproj") -->
src/prj/__SourceName__/      packable project-template package
src/prj/__SourceName__/Templates/  project-template folders
<!--#endif -->
src/prj/__SourceName__/Properties/Build/  MSBuild targets (not source)
src/prj/__SourceName__/Properties/NugetAssets/  nupkg assets (readme, icon, notes)
<!--#if (DotNetToolManifest) -->
src/prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
```

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/__SourceName__/` and run `dotnet restore`, `dotnet build`, or `dotnet pack`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
Open a terminal in `src/prj/__SourceName__/` and run `dotnet restore`, `dotnet build`, or `dotnet pack`. The CLI finds the one solution next to the library project; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, or `dotnet pack`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
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
