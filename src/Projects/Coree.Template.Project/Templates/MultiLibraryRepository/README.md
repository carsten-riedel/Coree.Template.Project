# __SourceName__

.NET multi-library repository. The GitHub landing page is this file.
<!--#if (PlaceSolution == "SlnFolder") -->
Each library's `.slnx` and notes live under `src/sln/__SourceName__/`. CLI commands for this library start in that folder.
<!--#elseif (PlaceSolution == "BesideLibrary") -->
This library's `.slnx` sits next to the packable project under `src/prj/__SourceName__/`. `src/sln/__SourceName__/` still exists for solution-level or cross-project files; delete that notes folder if you do not need it. CLI commands start in `src/prj/__SourceName__/`.
<!--#else -->
The solution lives at the repository root; projects live under `src/`. `src/sln/__SourceName__/` still exists for solution-level or cross-project files; delete that notes folder if you do not need it. If this repository has more than one `.slnx` in the root directory, pass the solution path to `dotnet`.
<!--#endif -->

```text
./                         repository root (this README)
<!--#if (WriteRepoVersionJson) -->
version.json               Nerdbank.GitVersioning (this repository)
<!--#endif -->
<!--#if (WriteChoosingPackageBoundaries) -->
ChoosingPackageBoundaries.md  NuGet package and compatibility boundaries
<!--#endif -->
<!--#if (WriteRepoDocTemplate) -->
docs/                      offline documentation template (this repository)
<!--#endif -->
<!--#if (PlaceSolution == "SlnFolder") -->
src/sln/__SourceName__/      this library's .slnx and notes
<!--#elseif (PlaceSolution == "BesideLibrary") -->
src/prj/__SourceName__/__SourceName__.slnx  solution (beside the library project)
src/sln/__SourceName__/      optional notes / cross-project files
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
__SourceName__.generated.slnx  solution
<!--#else -->
__SourceName__.slnx          solution
<!--#endif -->
src/sln/__SourceName__/      optional notes / cross-project files
<!--#endif -->
src/prj/__SourceName__/      packable class library
src/prj/__SourceName__.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/__SourceName__.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideLibrary") -->
Open a terminal in `src/prj/__SourceName__/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution next to the library project; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
<!--#endif -->

Command details and layout notes: [src/sln/__SourceName__/Readme.md](src/sln/__SourceName__/Readme.md).
