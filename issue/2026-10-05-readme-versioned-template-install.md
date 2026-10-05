# Add an exact-version dotnet new install example to the README

## Current state

The README already contains the normal unversioned installation command:

```bash
dotnet new install Coree.Template.Project
```

It also already links to the NuGet package under `Resources`:

`https://www.nuget.org/packages/Coree.Template.Project`

So this issue is **not** about a missing NuGet reference.

## Documentation gap

The current NuGet release is `Coree.Template.Project 0.9.0.1`.

NuGet exposes the exact-version template installation command:

```bash
dotnet new install Coree.Template.Project@0.9.0.1
```

The README does not currently show an exact-version installation example.

The .NET CLI supports `<package-name>@<package-version>` for installing a specific template package version, while the unversioned command resolves the latest stable version.

## Proposed README adjustment

Keep the existing unversioned command for the normal installation path and add one pinned example near the first installation section:

```bash
# Latest stable
dotnet new install Coree.Template.Project

# Exact documented release
dotnet new install Coree.Template.Project@0.9.0.1
```

A nearby NuGet link may improve discoverability, but the existing `Resources` link is already valid and does not need to be duplicated unless the README layout benefits from it.

## README duplication

The repository contains two currently identical README copies:

- `README.md`
- `src/prj/Coree.Template.Project/Properties/NugetAssets/README.md`

The second one is packed as the NuGet README. Any documentation change should keep both copies synchronized or establish one canonical source.

## Acceptance criteria

- The existing unversioned installation command remains documented.
- One exact-version example is added using `Coree.Template.Project@0.9.0.1`.
- The README makes clear that the versioned command pins a specific release.
- The issue does not imply that the NuGet package link is currently missing.
- The repository README and packaged NuGet README remain synchronized.
