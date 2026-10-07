# Self-reference the published analyzer package

Stand: 2026-10-07. Verbesserung für das packbare Analyzer-Projekt aus `MultiAnalyzerRepository`.

## Improvement

An analyzer project should be able to compile against the package it already published. That is the dogfood check: the released rules run on the next change, with the severity and limits the project chooses.

NuGet forbids that as a normal `PackageReference` when the reference id equals the project's `PackageId`. Restore fails with NU1108:

```text
error NU1108: Cycle detected.
error NU1108:   Coree.Analyzers.CodeClarity -> Coree.Analyzers.CodeClarity (>= 0.1.2).
```

This is by design. NuGet treats the project name and the package id as the same node. `PrivateAssets`, `DevelopmentDependency`, `ExcludeAssets`, and `NoWarn` do not bypass it. The open request is [NuGet/Home#6754](https://github.com/NuGet/Home/issues/6754). The same question for analyzers is [dotnet/roslyn#27339](https://github.com/dotnet/roslyn/issues/27339). [NU1108](https://learn.microsoft.com/en-us/nuget/reference/errors-and-warnings/nu1108) only says the package graph has a cycle.

A reference to a *different* package id is a normal `PackageReference`. Typography referencing CodeClarity does not need this workaround. Only the package referencing itself does.

## Why the template should grow this

These analyzer projects are created from the template. Each one will want the same check once it has a public version. The workaround is one imported file plus a `PackageReference`. It does not change the packed id.

`PackageDownload` also avoids the cycle, because it is not a dependency. It does not attach analyzers or props. Those have to be wired by hand. The `PackageReference` plus the id swap keeps the normal consumer shape.

## Mechanism

During evaluation, after the real `<PackageId>` is set, replace `PackageId` with a value that cannot match the published package. Restore then accepts the `PackageReference`. Register a target on `$(BeforePack)` that writes the real id back before the nupkg is written.

Do not use `BeforeTargets="$(PackDependsOn)"`. Current NuGet includes `_GetRestoreProjectStyle` in `PackDependsOn` ([NuGet.Client#6712](https://github.com/NuGet/NuGet.Client/pull/6712)). That target runs during Restore and would put the real id back before the graph is built, so NU1108 returns. `$(BeforePack)` stays pack-only. The form that matches this is the February 2026 update in [dotnet/symreader-converter#320](https://github.com/dotnet/symreader-converter/pull/320).

The fake id is still the `projectName` in `project.assets.json` for the whole build. `BeforePack` corrects the id that is written into the nupkg. The assembly name stays the project name.

## File

Place this next to the other project build files, for example `Properties/Build/SelfPackageId.targets`. Import it at the bottom of the packable csproj, after `<PackageId>` has been assigned. Each packable project gets its own copy. A shared import would couple packages the template keeps independent.

```xml
<Project>
  <!--
    NuGet rejects a PackageReference whose id equals this project's PackageId (NU1108).
    Evaluation uses a fake id so restore can take the published package.
    BeforePack puts the real id back, so the nupkg id is unchanged.
    BeforeTargets="$(PackDependsOn)" is wrong: that list runs during Restore and undoes the fake id.
  -->
  <PropertyGroup>
    <_ProjectDefinedPackageId>$(PackageId)</_ProjectDefinedPackageId>
    <PackageId>*fake_packageid_for_project_$(MSBuildProjectName)*</PackageId>
    <BeforePack>$(BeforePack);_UpdatePackageId</BeforePack>
  </PropertyGroup>

  <Target Name="_UpdatePackageId">
    <PropertyGroup>
      <PackageId>$(_ProjectDefinedPackageId)</PackageId>
    </PropertyGroup>
  </Target>
</Project>
```

```xml
<Import Project="Properties/Build/SelfPackageId.targets" />
```

## Package reference

Pin the published version. `PrivateAssets` keeps it out of the nupkg. `IncludeAssets` loads the analyzer and the build props. `SuppressDependenciesWhenPacking` on these analyzer projects already drops dependencies from the nuspec.

```xml
<PackageReference Include="Coree.Analyzers.CodeClarity" Version="0.1.2">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

Set the public properties in their own group. The comment must name that package. A second package's properties belong in a second group.

```xml
<!-- CodeClarity on this project is error, maximum 1. The package default severity is warning. -->
<PropertyGroup>
  <ReturnTypeComplexityAnalyzerSeverity>error</ReturnTypeComplexityAnalyzerSeverity>
  <ReturnTypeComplexityAnalyzerMaximum>1</ReturnTypeComplexityAnalyzerMaximum>
</PropertyGroup>
```

The shipped props set those values only when they are empty, so an unconditional assignment in the csproj wins.

## What was verified here

On `Coree.Analyzers.CodeClarity`, without the targets file, restore stopped at NU1108. With the file:

- Restore downloaded `Coree.Analyzers.CodeClarity` 0.1.2.
- The first build failed with `CCCRC001` on a return that calls two methods. That is the dogfood working at severity `error` and maximum 1. The calls were moved into locals. The return is `leftIsString || rightIsString`.
- `dotnet test` on the CodeClarity solution: 15 passed, Coverlet 100% line, branch, and method.
- `dotnet pack` wrote `Coree.Analyzers.CodeClarity.0.1.3.nupkg`. The nuspec id is `Coree.Analyzers.CodeClarity`. There is no dependency group, so the package does not depend on itself.
- `obj/project.assets.json` keeps `"projectName": "*fake_packageid_for_project_Coree.Analyzers.CodeClarity*"`. The dependency entry is still `Coree.Analyzers.CodeClarity >= 0.1.2`.

`Coree.Analyzers.Typography` uses the same targets file to reference Typography 1.0.0.5. It references CodeClarity 0.1.2 as a normal `PackageReference`, because the ids differ. Both severities on that project are `error`, and the CodeClarity maximum is 1. `dotnet test` on the Typography solution: 66 passed, Coverlet 100% line, branch, and method. The first build reported `CCCRC001` on returns with more than one step; those steps were moved into locals. Comment lines that contained the raw typographic characters were removed. The detection strings stay `\u` escapes. Two em dashes in the packed readme were rewritten so the self-check at severity `error` stays clean.

## Template notes

- Add `SelfPackageId.targets` only to the packable analyzer project, and only once that package exists on the feed the restore uses. A new package with no public version yet has nothing to reference.
- Import the file after the real `PackageId` property.
- Do not put the id swap inside a target. Restore reads the evaluated property.
- A project that references a different analyzer package does not import this for that reference.
- Projects that `ProjectReference` the analyzer are not packed here. They can see the fake id in the restore graph. That id must not leak into a packed nuspec. Confirm with `dotnet pack` and the nuspec `<id>`.
