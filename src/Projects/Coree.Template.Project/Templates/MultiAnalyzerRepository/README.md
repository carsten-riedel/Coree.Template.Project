# ClassLibrary

.NET analyzer package repository. The GitHub landing page is this file.
<!--#if (PlaceSolution == "SlnFolder") -->
Each package's `.slnx` and notes live under `src/sln/ClassLibrary/`. CLI commands for this package start in that folder.
<!--#elseif (PlaceSolution == "BesideLibrary") -->
This package's `.slnx` sits next to the packable analyzer under `src/prj/ClassLibrary/`. `src/sln/ClassLibrary/` still exists for solution-level or cross-project files; delete that notes folder if you do not need it. CLI commands start in `src/prj/ClassLibrary/`.
<!--#else -->
The solution lives at the repository root; projects live under `src/`. `src/sln/ClassLibrary/` still exists for solution-level or cross-project files; delete that notes folder if you do not need it. If this repository has more than one `.slnx` in the root directory, pass the solution path to `dotnet`.
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
src/sln/ClassLibrary/      this package's .slnx and notes
<!--#elseif (PlaceSolution == "BesideLibrary") -->
src/prj/ClassLibrary/ClassLibrary.slnx  solution (beside the analyzer project)
src/sln/ClassLibrary/      optional notes / cross-project files
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
ClassLibrary.generated.slnx  solution
<!--#else -->
ClassLibrary.slnx          solution
<!--#endif -->
src/sln/ClassLibrary/      optional notes / cross-project files
<!--#endif -->
src/prj/ClassLibrary/      packable analyzer package (netstandard2.0)
src/prj/ClassLibrary.Tests/  tests (not packed)
src/prj/ClassLibrary.DebugHost/  Visual Studio F5 compile target (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

<!--#if (PlaceSolution == "SlnFolder") -->
Open a terminal in `src/sln/ClassLibrary/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#elseif (PlaceSolution == "BesideLibrary") -->
Open a terminal in `src/prj/ClassLibrary/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution next to the analyzer project; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
<!--#endif -->

Command details and layout notes: [src/sln/ClassLibrary/Readme.md](src/sln/ClassLibrary/Readme.md).
