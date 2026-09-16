# __SourceName__

<!--#if (PlaceSolution == "SlnFolder") -->
This folder is the per-library area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` lives here; keep the folder while that is true. You can still add extra solution items here.
<!--#elseif (PlaceSolution == "BesideCsproj") -->
This folder is the packable class library. The `.slnx` and this readme sit next to the `.csproj`. There is no `src/sln/` tree for this library.
<!--#else -->
This folder is the per-library area under `src/sln/` for solution-level or cross-project files that should not sit next to a single `.csproj`.
The `.slnx` is written elsewhere (`PlaceSolution`). Use this folder for shared notes or extra solution items, or delete it if you do not need it.
<!--#endif -->
<!--#if ((HostIdentifier == "vs") && (PlaceSolution != "RepoRoot")) -->

Visual Studio created an automatically generated root `.slnx`. The correct template solution overwrote its contents, but the root `.slnx` remains as an extra conventional solution. Use the `.slnx` in the selected solution location and delete the extra root `.slnx` if you do not need it.
<!--#endif -->
<!--#if (PlaceSolution == "RepoRoot") -->

The solution file lives at the repository root. Open a terminal there for `dotnet restore`, `dotnet build`, and `dotnet pack`. If more than one `.slnx` sits in that directory, pass the solution path to `dotnet`.
<!--#else -->

<!--#if (PlaceSolution == "SlnFolder") -->
The `.slnx` and this readme live in this folder. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other libraries keep their own `.slnx` under `src/sln/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#else -->
The `.slnx` and this readme live in this folder next to the packable library. Open a terminal here for the commands below. The CLI finds the one solution in this directory; you do not pass a `.slnx` or `.csproj` path. Other libraries keep their own `.slnx` under `src/prj/<name>/`, so `dotnet` does not ask you to specify a solution.
<!--#endif -->

```text
<!--#if (PlaceSolution == "SlnFolder") -->
./                         you are here (this readme + __SourceName__.slnx)
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
<!--#endif -->
../../prj/__SourceName__/    packable project-template package
../../prj/__SourceName__/Templates/  project-template folders
<!--#if (DirectoryMsBuildFiles) -->
../../prj/__SourceName__/Directory.Build.props  optional empty library MSBuild landing file
../../prj/__SourceName__/Directory.Build.targets
<!--#endif -->
<!--#if (DotNetToolManifest) -->
../../prj/__SourceName__/.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
<!--#else -->
./                         you are here (this readme + __SourceName__.slnx + __SourceName__.csproj)
./Templates/               project-template folders
<!--#if (DirectoryMsBuildFiles) -->
./Directory.Solution.props  optional empty solution MSBuild landing file
./Directory.Solution.targets
./Directory.Build.props     optional empty library MSBuild landing file
./Directory.Build.targets
<!--#endif -->
<!--#if (DotNetToolManifest) -->
./.config/dotnet-tools.json  empty local tool manifest
<!--#endif -->
<!--#endif -->
```

Package metadata, license, icon, and release notes live in `src/prj/__SourceName__/Properties/NugetAssets/`.

`--tl:off` is optional. Without it the CLI shows the compact terminal logger. Add `--tl:off` for the classic per-project log. The commands work either way.

## Restore and build

```bash
dotnet restore
dotnet build
```

## Pack

```bash
dotnet pack
```

Creates one `.nupkg` in `src/prj/__SourceName__/bin/Pack/` containing the project-template package.
<!--#if (NuGetAuditHighCriticalAsErrors) -->

Restore fails this class library on high (`NU1903`) and critical (`NU1904`) vulnerable packages. Low and moderate stay warnings.
<!--#endif -->
<!--#if (WritePackageDocTemplate) -->

## Package documentation template

`src/prj/__SourceName__/Properties/NugetAssets/docs/DocShell.html` is the offline documentation seed. It packs with the nupkg (`docs/` inside the package). Bootstrap the site from that file (vendor the local css/js/licenses next to it), then write package documentation for this library. Repository-root `docs/` is a separate site if present.
<!--#endif -->

## CI

Use `-m:1` for the build so a pipeline does not depend on machine load. Run these commands from this folder so each library has exactly one `.slnx` in the working directory.

```bash
dotnet restore
dotnet build --no-restore -m:1
dotnet pack
```
<!--#endif -->
