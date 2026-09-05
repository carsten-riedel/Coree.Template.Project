# ClassLibrary

.NET multi-library repository. The GitHub landing page is this file.
<!--#if (PlaceSolutionInSrc) -->
The solution and CLI commands live under `src/`.
<!--#else -->
The solution lives at the repository root; projects live under `src/`.
<!--#endif -->

```text
./                         repository root (this README)
<!--#if (PlaceSolutionInSrc) -->
src/                       solution root
src/ClassLibrary.slnx      solution
<!--#else -->
<!--#if (HostIdentifier == "vs") -->
ClassLibrary.generated.slnx  solution
<!--#else -->
ClassLibrary.slnx          solution
<!--#endif -->
src/                       projects and solution notes
<!--#endif -->
src/prj/ClassLibrary/      packable class library
src/prj/ClassLibrary.Tests/  tests (not packed)
<!--#if (BenchmarkProject == true) -->
src/prj/ClassLibrary.Benchmark/  optional BenchmarkDotNet console app
<!--#endif -->
src/wrk/ClassLibrary/      solution notes
```

<!--#if (PlaceSolutionInSrc) -->
Open a terminal in `src/` and run `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet pack`, or `dotnet publish`. The CLI finds the solution; you do not pass a `.slnx` or `.csproj` path.
<!--#else -->
Open a terminal in the repository root and run `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet pack`, or `dotnet publish`. The CLI finds the solution; you do not pass a `.slnx` or `.csproj` path.
<!--#endif -->

Command details, test-report links, pack output, and CI notes: [src/wrk/ClassLibrary/Readme.md](src/wrk/ClassLibrary/Readme.md).
