# ClassLibrary

.NET multi-library repository. The GitHub landing page is this file.
<!--#if (PlaceSolutionInSolutionFolder) -->
Each library's `.slnx` and notes live under `src/sln/ClassLibrary/`. CLI commands for this library start in that folder.
<!--#else -->
The solution lives at the repository root; projects live under `src/`. If this repository has more than one `.slnx` in that directory, pass the solution path to `dotnet`.
<!--#endif -->

```text
./                         repository root (this README)
<!--#if (WriteRepoVersionJson) -->
version.json               Nerdbank.GitVersioning (this repository)
<!--#endif -->
<!--#if (WriteRepoDocTemplate) -->
documentation/             offline documentation template (this repository)
<!--#endif -->
<!--#if (PlaceSolutionInSolutionFolder) -->
src/sln/ClassLibrary/      this library's .slnx and notes
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
ClassLibrary.generated.slnx  solution
<!--#else -->
ClassLibrary.slnx          solution
<!--#endif -->
src/sln/ClassLibrary/      solution notes
<!--#endif -->
src/prj/ClassLibrary/      packable class library
src/prj/ClassLibrary.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
```

<!--#if (PlaceSolutionInSolutionFolder) -->
Open a terminal in `src/sln/ClassLibrary/` and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the one solution in that folder; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, or `dotnet pack`. The CLI finds the solution only if this directory contains exactly one `.slnx`.
<!--#endif -->

Command details, test-report links, pack output, and CI notes: [src/sln/ClassLibrary/Readme.md](src/sln/ClassLibrary/Readme.md).
