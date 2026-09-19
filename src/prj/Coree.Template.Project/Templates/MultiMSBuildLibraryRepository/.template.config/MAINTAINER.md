# `.template.config` (MultiMSBuildLibraryRepository)

Product surface: **`.NET multi-msbuild repository`**. `identity` is `CoreeTemplatesProjectMultiMSBuildLibraryRepository`; CLI short name is `multimsbuildrepo-coree`. No `ChoosingPackageBoundaries.md`.

Fork of MultiLibraryRepository. Combo, init, TemplateAssets, and host JSON stay. No UseWebSdk. No `ChoosingPackageBoundaries.md`. Task package is netstandard2.0, packs under `tasks/` with `build/` UsingTask, `Microsoft.Build.Utilities.Core` 15.9.20. Always-on DebugHost for Visual Studio F5 as a real MSBuild consumer. Tests, DebugHost, and optional benchmark share one DebugHost target framework from the wizard (select .NET 8, 9, or 10; not a library-style TFM matrix). Scaffold `AddTask` (Required/Output/Execute), `TaskNodeTask` (`IBuildEngine.ProjectFileOfTaskNode`), `HomeTask` (home directory + `ExcludeFromCodeCoverage` on OS helpers), and `DumpEnvVarsTask` (opt-in env dump) keep Coverlet compiling. Consumer `build/__SourceName__.props` is the PackageReference `UsingTask` surface. Consumer `build/__SourceName__.targets` holds the sample `BeforeTargets=CoreCompile` populate (`HomeTask` / `TaskNodeTask` → `$(HomeDirectory)` / task-node properties). `DumpEnvVarsTask` / `AddTask` stay registered for a consumer csproj or `Directory.Build.targets` to invoke. `DumpGlobalPropTask` / `BuildEngineExtensions` stay out (private MSBuild fields + `Microsoft.Build` engine reference).

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**, and **wizard order** (`symbolInfo` array; DebugHost target framework is first). `persistenceScope: none` so the New Project dialog does not reuse the last create. Host mapping: **CLI ↔ Visual Studio**. No `icon` property: see **Visual Studio template icon**. |
| `dotnetcli.host.json` | CLI long names; empty `shortName` for `InitRepoItems`, `InitAllRepoItems`, `DebugHostTargetFramework`, `Author`, `CSharpProjectOptions`, `ProjectLicense`, `NerdbankGitVersioning`, `PublicApiAnalyzers`, `DocumentationTemplate`, `ProjectEditorGlobalConfig`, `AnalysisMode`, `NuGetAuditHighCriticalAsErrors`, `TestCoverage`, `DirectoryMsBuildFiles`, and `DotNetToolManifest` so they do not steal single-letter aliases. |
| `icon.png` | **Intentionally absent.** Visual Studio then uses the template **package** icon. |
| `MAINTAINER.md` | This file. |

## Visual Studio template icon

Create a new project shows one icon per template. Two files can supply it; they are not the same surface.

| Source | Path | What uses it |
| --- | --- | --- |
| Template package | `src/prj/Coree.Template.Project/Properties/NugetAssets/Icon.png` (`PackageIcon` on `Coree.Template.Project.csproj`) | NuGet listing **and** the VS picker when this template does not declare its own icon. |
| This template | `.template.config/icon.png`, optional `ide.host.json` `"icon": "icon.png"` | VS picker for **this** template only. Overrides the package icon. |
| Generated library | `src/prj/{Name}/Properties/NugetMetadata/Icon-128x128.png` | The **consumer** nupkg after `dotnet pack`. Not the template picker. |

Verified in Visual Studio (Create a new project, Recent project templates): a template with `.template.config/icon.png` showed that image; sibling Coree templates without one showed the package icon. Leave this template's picker icon **undefined** so the package icon is used. Ship per-template picker icons later; do not copy `Icon-128x128.png` here as a stand-in.

## Intended usage

The template bootstraps a **repository layout** for one or more packable MSBuild task libraries. It does not `git init`. Same `--output` = combo repo; different `--output` = separate repos.

`Author` is required on every create. Everyday CLI is author, name, output; root files only on the first create into an empty folder.

Install from this folder (or from the packed `Coree.Template.Project` nupkg):

```powershell
dotnet new install "C:\dev\github.com\carsten-riedel\Coree.Template.Project\src\prj\Coree.Template.Project\Templates\MultiMSBuildLibraryRepository" --force
```

Folder install is the local loop. Verify by generating into `%TEMP%`. Do not `dotnet build` `src/prj/__SourceName__/__SourceName__.csproj` in this tree: it is template source (every `<!--#if` branch still present). A C# design-time build of that stub is enough to run `GenerateAssemblyInfo`. A real stub build also runs `InitializePublicApi` and can write empty `Properties/PublicAPI/*.txt` here.

Combo repo, three libraries, root files only once:

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.MSBuild1" --output "C:\Users\Valgrind\source\repos\MultiMSBuildLibraryRepository-multisolution-optin" --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.MSBuild2" --output "C:\Users\Valgrind\source\repos\MultiMSBuildLibraryRepository-multisolution-optin"
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.MSBuild3" --output "C:\Users\Valgrind\source\repos\MultiMSBuildLibraryRepository-multisolution-optin"
```

### CLI use cases

`$out` is the same folder for a combo repo. Different `--output` = separate repos. `--NerdbankGitVersioning` default is **`Project`** (`Properties/version.json`, extra package). Combo-safe: later libraries do not collide. `--NerdbankGitVersioning Off` keeps VersionPrefix. `--NerdbankGitVersioning Repo` is the shared root file.

**Default, one library**

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.MSBuild1" --output $out --InitAllRepoItems
```

Root: `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, `LICENSE`. Library: Nerdbank **Project**, `Properties/version.json`.

**Default, multi-library (combo)**

```powershell
# combo gut
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

Call 1 writes the five root files. Call 2 only adds `src/prj` / `src/sln`. Both libraries get `Properties/version.json`.

**Nerdbank repository, multi-library**

```powershell
# combo gut - Call 2 only wires the library; generate does not stamp version.json again
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json)
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

**Nerdbank project folder, multi-library** (this is the omit-the-switch default)

```powershell
# combo gut - same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out
```

**Two separate repos (not a combo)**

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.LibA" --output $outA --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.LibB" --output $outB --InitAllRepoItems
```

Each `--output` is its own first create.

After the first call in the default combo the repo root has `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, and `LICENSE`. Each library gets `Properties/version.json`. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitAllRepoItems` or `--InitRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite. `--InitRepoItems SrcGlobalJson` is a separate first-create opt-in (`src/global.json`, default off).

`--InitAllRepoItems` is the CLI first-create set (same five files as Visual Studio). It does **not** add `version.json` or `src/global.json`. `--InitRepoItems` picks individual files. Values are separated by **spaces**. Repeating `--InitRepoItems` per value also works. A quoted `Readme|AIReleaseCheckpoint|GitAttributes|GitIgnore|RepoLicense` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "Organization.Domain.MSBuild1" --output "<repo>" --InitRepoItems AIReleaseCheckpoint
```

**Visual Studio:** first create uses the `ide.host.json` default (same five root files as `--InitAllRepoItems`; `src/global.json` unchecked). A second library in the IDE cannot omit the group: choose **None**. Later libraries in the same folder are otherwise the CLI path above. Why the two hosts differ is in **CLI ↔ Visual Studio** below.

## CLI ↔ Visual Studio

The generated first-create product is meant to match. The **switches** cannot be identical, because the hosts do not have the same empty-set, default, or repeat-create rules. Do not "fix" this by making `template.json` `defaultValue` equal the Visual Studio default.

### Why CLI defaults stay empty

A combo repository is two or more `dotnet new` calls into the **same** `--output`. The template engine has **one** CLI default for every call. If `InitRepoItems` defaulted to the five root files, the second library would hit Exit 73 (collision) unless the caller passed `None` or `--force`. Visual Studio's New Project dialog is a **first create** into an empty folder; it can check the five boxes by default. CLI later-libraries omit `--InitAllRepoItems` and `--InitRepoItems`. `NerdbankGitVersioning` defaults to **`Project`** on both hosts (per-library `Properties/version.json`; later creates do not collide). `--NerdbankGitVersioning Repo` does not write the root `version.json` by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again without Exit 73.

### CLI → Visual Studio

| CLI | Visual Studio equivalent | Why |
| --- | --- | --- |
| `--InitAllRepoItems` | Leave **Repository root items** at the ide.host default (all five files checked) | Bool flag with no value list. The engine cannot treat a bare `--InitRepoItems` as "all"; that is Exit 127. The set is the VS first-create default, not another checkbox in that group. `version.json` and `src/global.json` are not in this set. |
| `--InitRepoItems Readme …` (spaces) | Uncheck the files you do not want | Individual files. `|` is only legal in `ide.host.json` `defaultValue`, not on current `dotnet new`. |
| omit both switches | **None** | CLI may leave a multi-choice empty. Visual Studio may not. |
| `--InitRepoItems None` | **None** | Explicit empty set. Also wins over `--InitAllRepoItems` if both are passed. Everyday CLI later-libraries omit the switches instead. |
| `--InitAllRepoItems` hidden from the wizard | `ide.host.json` `isVisible: false` | A choice `All` inside the same VS group would sit next to `None` and the five files; you cannot hide one choice per host. The bool is CLI convenience only. |

### Visual Studio → CLI

| Visual Studio | CLI equivalent | Why |
| --- | --- | --- |
| First create, root items left at default | `--InitAllRepoItems` | Same five files. Do not translate the ide.host string `Readme\|…\|RepoLicense` onto the CLI. |
| Uncheck some root items | `--InitRepoItems` plus the remaining choice names | Subset. |
| Second library: **None** | omit `--InitAllRepoItems` and `--InitRepoItems` | The IDE requires at least one value; leftover checks from persistence would otherwise stamp root files again. `persistenceScope: none` still needs **None** as the empty-set control. CLI empty default is that None. |
| `InitAllRepoItems` not shown | do not look for it in Additional information | CLI-only. |

`persistenceScope: none` on the VS symbols that have custom defaults: the dialog must not reuse the last create (especially **None** or a subset) as the next "first create".

Other host-only switch behavior (not root files, same class of reason):

- **`PlaceSolution` `RepoRoot`:** CLI renames to `{Name}.slnx` at repo root. Visual Studio keeps `{Name}.generated.slnx` so it does not overwrite the `{Name}.slnx` the IDE always writes. `SlnFolder` and `BesideCsproj` rename on both hosts (paths are not the VS root file). Post-actions that open the handbook Readme and tell you to close/reopen are `HostIdentifier == "vs"` only. `primaryOutputs` index 0 is the surviving handbook path (`src/sln/{Name}/Readme.md` or `src/prj/{Name}/Readme.md` when `BesideCsproj`).
- **`CSharpProjectOptions` / TFMs / `ProjectLicense` / `NerdbankGitVersioning` / `PublicApiAnalyzers` / `DocumentationTemplate` / `TestCoverage` / `AnalysisMode` / `NuGetAuditHighCriticalAsErrors` / `DirectoryMsBuildFiles` / `DotNetToolManifest`:** same defaults on both hosts (`NerdbankGitVersioning` `Project`, `PublicApiAnalyzers` `false`, `DocumentationTemplate` empty/`None`, `TestCoverage` `Coverlet`, `AnalysisMode` `Recommended`, `ProjectEditorGlobalConfig` `Strict`, `NuGetAuditHighCriticalAsErrors` `true`, `DirectoryMsBuildFiles` `false`, `DotNetToolManifest` `true`). `--NerdbankGitVersioning Repo` does not write the root file by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again. `--DocumentationTemplate Package` is safe on later libraries; `--DocumentationTemplate Repository` on a later library is Exit 73.
- **`Author`:** required on both. CLI `--Author`.

## `InitRepoItems` / `InitAllRepoItems`

`InitRepoItems` is the multi-choice (`allowMultipleValues`), not N bools. Visual Studio shows **one group** of checkboxes (same shape as target frameworks). `InitAllRepoItems` is a CLI bool for that same first-create set; `ide.host.json` hides it.

| Host | Default | Empty set |
| --- | --- | --- |
| CLI `InitRepoItems` | none selected (`defaultValue` `""`) | omit the switch |
| CLI `InitAllRepoItems` | `false` | omit the switch |
| Visual Studio | `Readme\|AIReleaseCheckpoint\|GitAttributes\|GitIgnore\|RepoLicense` | not allowed; choose `None` |

`None` is first in the choice list. `sources` exclude each root file unless `InitAllRepoItems` is on or that choice is selected; `None` excludes all of them, including when `InitAllRepoItems` is on. `==` in conditions means the value is among the selected choices. `GitIgnore` is the same shape as `GitAttributes`: seed is the output name at the template root (`.gitignore`), same exclude condition, no rename. Nested `src/prj` / `src/sln` `.gitignore` files use those full paths (see Benchmark exclude); they are not this switch and stay on every create.

This template does not stamp `ChoosingPackageBoundaries.md` (library-repo decision guide only).

`SrcGlobalJson` is `src/global.json`. Opt-in, **default off**, not part of `--InitAllRepoItems` or the Visual Studio first-create checks. Exclude unless this choice is selected (`None` still wins). Seed lives at that output path (not under `TemplateAssets/`). `SdkPinVersion` follows the selected **DebugHost** TFM (`8.0.0` / `9.0.0` / `10.0.0`), not netstandard2.0; `rollForward` is `latestFeature`. Later library with this box selected is Exit 73. Commands started at the repository root do not see this file.

## AI-supported release checkpoint

The generated product still contains placeholders that can only become true **after implementation** - especially empty `src/prj/*/Properties/NugetMetadata/Readme.md`. A human or an LLM can fill those from the code. That is a **gate before the first publish**, not a generate-time script and not standing agent rules.

**Not:** run-once / post-bootstrap right after `dotnet new`. The library may still be `AddTask`.  
**Not:** a forever queue in the GitHub `README.md`. That file is the customer landing page.  
**Not:** a template-stamped `AGENTS.md`. That would collide with the consumer's own agent file and would outlive the scaffold.

**Yes:** one repo-root file, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, stamped only when `InitAllRepoItems` is on or `InitRepoItems` includes `AIReleaseCheckpoint` (first create). `TEMPLATE-` marks it as delete-me scaffold; `AI` matches the VS choice. It is a **Template-Checkpoint-Release**: close template residue, then **self-dissolve**. After that, new chats read the libraries and the real NuGet docs. The file is written for a person; an assistant can fill it from the product. It is not a prompt and not standing agent rules.

Why the repo root, not `src/sln/{Name}/`: the first look at a combo repo is the customer surface; one fat checklist can say "update every NuGet readme in this repository" without a per-library marker. Libraries added later are in scope until the file is deleted.

The VS label **AI-supported release checkpoint** names the *job*, not a recurring agent run. The switch does not start a model. Someone later (person or LLM) works that file to 100% observable items, then deletes it. "AI-supported" belongs in the choice display name; it must not read as "edit with AI on every create."

The generated file is the contract (ten numbered, checkable items). Do not put free-form "run this shell" instructions in it (prompt injection). Do not mix standing style rules into it - those must not self-delete.

## `CSharpProjectOptions`

One multi-choice (`allowMultipleValues`), same VS checkbox combobox as TFMs and `InitRepoItems`. Not four/five separate dropdowns.

| Choice | Checked (default) | Unchecked |
| --- | --- | --- |
| `DisableImplicitUsings` | `disable` | `enable` |
| `Nullable` | `enable` | `disable` |
| `LangLatest` | `latest` | `default` (TFM C# version; valid compiler value) |
| `DebugEmbedded` | `embedded` | `none` |
| `GenerateDocumentationFile` | `true` | `false` |

Language/debug values are **always written** (no omitted PropertyGroup) into the library, tests, DebugHost, and benchmark csproj. `GenerateDocumentationFile` is in this same choice list but only the **library** writes it (own PropertyGroup). Tests, DebugHost, and benchmark have no XML-docs surface; default `true` there is CS1591 on public types without comments.

No `None`. CLI and VS default is the five product values. Visual Studio cannot leave a multi-choice empty; at least one box stays checked. CLI: omit the switch, or pass values with **spaces** (`--CSharpProjectOptions Nullable LangLatest DebugEmbedded`). `|` is only the host default separator.

Benchmark previously hardcoded `ImplicitUsings` enable. It now follows the switch. `Program.cs` has explicit `System` / `System.IO` / `System.Linq` usings so the default (`disable`) still compiles.

## `ProjectLicense`

Single choice (dropdown, not a checkbox group). Project + NuGet only. Repository-root `LICENSE` is `InitRepoItems` choice `RepoLicense` (UI: **LICENSE file at repository root**) or `--InitAllRepoItems`, not a second VS bool.

| Choice | Project `Properties/NugetMetadata/License.txt` | NuGet |
| --- | --- | --- |
| `MIT` (CLI/VS default) | none | `PackageLicenseExpression` `MIT` |
| `BSD3Clause` | none | `PackageLicenseExpression` `BSD-3-Clause` |
| `Apache2` | none | `PackageLicenseExpression` `Apache-2.0` |
| `Custom` | generated and packed | `PackageLicenseFile` `License.txt` |

Do not set `PackageLicenseFile` together with an expression (NU5033). The glob excludes `License.txt` except for `Custom`.

Seeds live under `TemplateAssets/Licenses/` (`MIT.txt`, `BSD3Clause.txt`, `Apache2.txt`, `Custom.txt`). Extra sources copy only the `Custom` seed to `src/prj/{Name}/Properties/NugetMetadata/License.txt`; standard choices create no project license file. The separate root `LICENSE` source copies the selected seed for every choice when `WriteRepoLicense` is true. Do not leave a mega-file under `src/prj/__SourceName__/Properties/NugetMetadata/`. DocShell extra sources must `exclude` `Licenses/**` (same as `CodeStyle/**` and `Versioning/**`). Each seed may use a shallow `//#if (PackageCopyrightHolderIsSet)` / `//#else` / `//#endif`. Do not put `ProjectLicense` `#if` in the seed: extra sources pick the file.

`RepoLicense` is off on CLI unless listed in `--InitRepoItems` or `--InitAllRepoItems` is on. Visual Studio includes it in the first-create default. `WriteRepoLicense` is `(InitRepoItems != None) && (InitAllRepoItems || InitRepoItems == RepoLicense)`. First create only; a later library with `RepoLicense` or `InitAllRepoItems` selected collides (Exit 73), same as root README. `None` excludes it even if leftover checks remain.

## `NerdbankGitVersioning`

Single choice (dropdown), same shape as `ProjectLicense`. CLI long name is **`--NerdbankGitVersioning`** so the extra NuGet dependency is visible. Default **`Project`**: `version.json` under this library's `Properties/` folder. `Repo` uses one root `version.json`. `Off` is the VersionPrefix group, no package. `--InitAllRepoItems` is the five root files only; it does **not** copy root `version.json` or `src/global.json`. Tests, DebugHost, and benchmark are not in this switch.

```powershell
# combo gut
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

Call 1: five root files plus root `version.json`, library wire is Nerdbank repo. Call 2: `--NerdbankGitVersioning Repo`, **no** Init, **no** second root stamp (Exit 0). Omit `--NerdbankGitVersioning` for **Project** (`Properties/version.json`). `--NerdbankGitVersioning Off` for the VersionPrefix group.

Two jobs, same split as root `LICENSE`:

- **`NerdbankGitVersioning`** wires **this** library csproj on every create.
- **`WriteRepoVersionJson`** copies the **repository-root** `version.json` only on a first create.

If `Repo` itself stamped the root file, call 2 would be Exit 73.

```text
UseNerdbankGitVersioning  = (NerdbankGitVersioning == Project || NerdbankGitVersioning == Repo)

WriteRepoVersionJson      =
  (NerdbankGitVersioning == Repo)
  && (InitRepoItems != None)
  && (InitAllRepoItems
      || InitRepoItems == Readme
      || InitRepoItems == AIReleaseCheckpoint
      || InitRepoItems == GitAttributes
      || InitRepoItems == GitIgnore
      || InitRepoItems == RepoLicense
      || InitRepoItems == SrcGlobalJson)
```

`UseNerdbankGitVersioning` is generate-time `<!--#if` in `__SourceName__.csproj`. The VersionPrefix group is the `#else` (`Off`). Both branches stay in the **template source**; `dotnet new` keeps one.

`None` wins over InitAll. A second **generate-time** write of root `version.json` is Exit 73. `--NerdbankGitVersioning Repo` without Init does not stamp the file at `dotnet new`. `Properties/Build/NerdbankRepositoryVersion.targets` (imported only for Repo **after generate**) copies `Properties/Build/Nerdbank.version.json` to the repository root **if it does not exist**, before Nerdbank reads it. A later library in the same folder therefore does not collide.

In **template source** those `<!--#if (NerdbankGitVersioning == "Repo") -->` markers are XML comments, so MSBuild always imports the targets. From `src/prj/__SourceName__`, `../../../version.json` is this template folder. The copy no-ops when `../../../.template.config` exists. A `version.json` beside this `MAINTAINER.md` is a failed host stamp - delete it, do not commit.

There is no `version.json` checkbox in `InitRepoItems`. Generate-time root file is `WriteRepoVersionJson` (`Repo` plus first-create Init). If that file is still missing, the Repo library writes it once at build (`if not exists`). Later VS library: **None** plus **This repository (root version.json)**.

Seeds live under `TemplateAssets/Versioning/`. `Project/version.json` uses `pathFilters` `[".."]` (height is the packable project folder next to `Properties/`; tests, DebugHost, and benchmark are siblings and do not bump). `Repo/version.json` uses `pathFilters` `["."]` (height is the whole repository). Extra sources copy `Versioning/Project/` to `Properties/` and `Versioning/Repo/` to the repository root. `Properties/Build/Nerdbank.version.json` is the same payload as `Versioning/Repo/version.json` (late first-build copy). `Project` is first in the choice list because it is the default.

### What each symbol stamps

| Output | On when | Off when |
| --- | --- | --- |
| Root `version.json` | `WriteRepoVersionJson` | any other combination |
| `src/prj/{Name}/Properties/version.json` | `NerdbankGitVersioning == Project` | `Off` or `Repo` |
| VersionPrefix group | `NerdbankGitVersioning == Off` | `Project` or `Repo` |
| `GitVersionBaseDirectory` (`Properties`) | `NerdbankGitVersioning == Project` | `Off` or `Repo` |

### `NerdbankGitVersioning` × Init → root `version.json`

| `NerdbankGitVersioning` | `--InitAllRepoItems` | `InitRepoItems` | Root `version.json` |
| --- | --- | --- | --- |
| `Repo` | true | not `None` | write |
| `Repo` | false | omit | skip |
| `Repo` | false | `Readme` / checkpoint / `.gitattributes` / `.gitignore` / `RepoLicense` / `SrcGlobalJson` (not `None`) | write |
| `Repo` | true or false | `None` | skip |
| `Project` | true or false | any | skip |
| `Off` | true or false | any | skip |

### `NerdbankGitVersioning` → library tree (Init ignored except root json above)

| `NerdbankGitVersioning` | UI | VersionPrefix group | `GitVersionBaseDirectory` | `Properties/version.json` |
| --- | --- | --- | --- | --- |
| `Off` | **Off (VersionPrefix in the library project)** | yes | no | skip |
| `Project` (default, first) | **This library (Properties/version.json)** | no | `Properties` | write |
| `Repo` | **This repository (root version.json)** | no | no | skip |

### Everyday CLI

**Default, multi-library**

```powershell
# combo gut
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Project` | skip | no |
| 2 | false | `Project` | skip | no |

**Nerdbank repository, multi-library** (no collision on call 2)

```powershell
# combo gut
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Repo` | write | no |
| 2 | false | `Repo` | skip | no |

**Nerdbank project folder, multi-library** (this is the omit-the-switch default)

```powershell
# combo gut - same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out
```

### Combos that look successful but are incomplete or mixed

`dotnet new` Exit 0 only means no file collision. It does not mean every library in `$out` uses the same versioning.

| Calls | Exit | What is wrong |
| --- | --- | --- |
| `--NerdbankGitVersioning Repo` on an empty folder, no Init | 0 | `dotnet new` does not stamp root `version.json`. First build/pack writes it if missing. |
| `--InitAllRepoItems --NerdbankGitVersioning Repo --InitRepoItems None` | 0 | `None` wins; five root files skipped and no root `version.json`. |
| Call 1 InitAll `--NerdbankGitVersioning Off`, call 2 `--NerdbankGitVersioning Repo` | 0 | Library 1 VersionPrefix, library 2 Nerdbank walking up with no root json. |
| Call 1 InitAll `--NerdbankGitVersioning Repo`, call 2 omit the switch | 0 | Library 1 Nerdbank + root json, library 2 **Project** (`Properties/version.json`). |
| Call 1 InitAll (default Project), call 2 `--NerdbankGitVersioning Repo` (or the reverse) | 0 | Mixed `Properties/version.json` and repo-style package with no matching root file. |

Keep the same `--NerdbankGitVersioning` value on every library in one `--output`. Root `version.json` only on the first create together with Init.

## `ProjectEditorGlobalConfig`

Library-only single choice, default **`Strict`**. UI label **Code style rules for library project** (packable task-library project only; `is_global` is the analyzer-config technical term). CLI long name **`--ProjectEditorGlobalConfig`**. Stamps `src/prj/{Name}/.project.editor.globalconfig` and wires `GlobalAnalyzerConfigFiles`, `EnforceCodeStyleInBuild`, and `OptimizeImplicitlyTriggeredBuild=false`. `Off` writes nothing. Tests, DebugHost, and benchmark do not get the file. Keep the symbol and generated disk name. Do not use a bare "project" label: **C# project options** already applies to library, tests, DebugHost, and benchmark.

Roslyn reads `GlobalAnalyzerConfigFiles` (`Visible="false"`). Visual Studio Solution Explorer uses a separate `None` item with `Link` under `Properties\` so the file is clickable next to `version.json` / Public API. `None Remove` first, or the SDK default glob also shows it at the project root. Do not use `AdditionalFiles` or `Content`. Do not set `CopyToOutputDirectory` (`None` already does not copy or pack). Do not replace `GlobalAnalyzerConfigFiles` with the `None` item. Do **not** move the file into `Properties/` on disk: analyzer-config scope follows the directory of the file, so it must stay next to the csproj. `Link` is UI-only.

Do not rename `Default` to Minimal: both seeds are the same full VS style dump. The split is **severity**, not breadth.

| Choice | Seed | What differs |
| --- | --- | --- |
| `Default` | `.project.editor.globalconfig.default` | Style dump. Naming stays **suggestion**. No CS1591 / nullable / IDE0005 overrides. |
| `Strict` | `.project.editor.globalconfig.strict` | Same dump. Naming **error**. Public-release compiler gates: XML docs (CS1591 family), nullable (CS86xx), unused usings (IDE0005), reserved identifiers (CA1716). |
| `Off` | none | No file, no `EnforceCodeStyleInBuild`, no `OptimizeImplicitlyTriggeredBuild`. |

`Strict` needs the product C# defaults (`GenerateDocumentationFile`, `Nullable`, `DisableImplicitUsings`) or those errors fire on every build for the wrong reason. The scaffold `AddTask`, `TaskNodeTask`, `HomeTask`, and `DumpEnvVarsTask` already have XML docs so a first Strict build can pass. Do not generate unused usings to demonstrate IDE0005. Needed `using` lines follow `CSharpProjectOptions == "DisableImplicitUsings"` when a type actually requires them. Sample tasks use `Microsoft.Build.Framework` and a fully specified `Microsoft.Build.Utilities.Task` base so implicit usings (`System.Threading.Tasks.Task`) do not collide. `HomeTask` keeps `Execute()` in Coverlet and puts `[ExcludeFromCodeCoverage]` on the OS probe and the missing-home warning (one testhost cannot hit every branch). `DumpEnvVarsTask` is registered in consumer props; `HomeTask` / `TaskNodeTask` run from consumer `.targets` before `CoreCompile` on PackageReference consumers.

Seeds live under `TemplateAssets/CodeStyle/`. Each extra source copies that folder to the library project, **excludes** the other seed, and **renames** the chosen file to `.project.editor.globalconfig`. Do not leave a seed under `src/prj/__SourceName__/`: DocShell extra sources must `exclude` `Versioning/**`, `CodeStyle/**`, `Licenses/**`, and `DirectoryMsBuild/**`.

`UseProjectEditorGlobalConfig` is `(ProjectEditorGlobalConfig != "Off")`; the csproj `#if` does not need a new branch per dump.

`EnforceCodeStyleInBuild` is required or IDE naming stays IDE-only (SDK default is false). Compiler diagnostics in Strict (CS1591, CS86xx) fail `dotnet build` without that flag; IDE1006 needs it. `OptimizeImplicitlyTriggeredBuild=false` is required with that same `#if`: Visual Studio skips analyzers on Test Explorer / F5 implicit builds (`IsImplicitlyTriggeredBuild`), so Run Tests can stay green while `dotnet test` fails the same IDE errors. Product assumption is VS MSBuild == `dotnet test` for library style gates. Do not add `EnableNETAnalyzers` / `RunAnalyzers*` `true` noise - those already default true on net8/net10. Coverlet `Threshold` still runs only on `dotnet test` (`coverlet.msbuild`), not Test Explorer.

The `None` Link is Solution Explorer only and can sit in the same `#if` block.

## `AnalysisMode`

Library-only single choice, default **`Recommended`**. UI label **Code analysis mode**. CLI long name **`--AnalysisMode`**. Omit the switch → `Recommended`. Writes `<AnalysisMode>` on the task library only (placeholder `__AnalysisMode__`). Tests, DebugHost, and benchmark do not get the property. Combo-safe.

This is the SDK **CA** rule set, not code style and not Public API analyzers. Do not merge it into `ProjectEditorGlobalConfig`. Do not add `None` or SDK `Default`: a packable library keeps analyzers on; `Minimum` / `Recommended` / `All` are the three product values. Do not add `AnalysisLevel` on the same switch (that pins a SDK rule version). Do not add `EnableNETAnalyzers` `true` noise.

Warnings only unless the consumer later sets `TreatWarningsAsErrors`. `All` is noisy on `AddTask`.

The PropertyGroup sits in the packable csproj with the other library-only properties.

## `NuGetAuditHighCriticalAsErrors`

Library-only bool, default **true**. UI label **Treat high/critical NuGet vulnerabilities as errors**. CLI long name **`--NuGetAuditHighCriticalAsErrors`**. Omit the switch → on. `--NuGetAuditHighCriticalAsErrors false` writes nothing. Tests, DebugHost, and benchmark are not in this switch. Combo-safe.

SDK restore already runs NuGetAudit (NU1901–NU1904 warnings). This switch only appends `<WarningsAsErrors>$(WarningsAsErrors);NU1903;NU1904</WarningsAsErrors>` on the packable library. Low (`NU1901`) and moderate (`NU1902`) stay warnings. Do not set `NuGetAudit` / `NuGetAuditMode` / `TreatWarningsAsErrors` here (`NuGetAudit` is already on). Do not promote the test-project `WriteNugetReport` `dotnet list package` files: that command's exit code is not a findings gate.

Turn the switch off for an EOL or backport graph (for example net8 after support ends) that cannot be cleaned without dropping a TFM. Do not put this on tests: MSTest/Coverlet CVEs must not fail the nupkg restore.

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --NuGetAuditHighCriticalAsErrors false
```

## `PublicApiAnalyzers`

Library-only bool, default **false**. UI label **Public API analyzers**. CLI long name **`--PublicApiAnalyzers`**. Separate from repository-root Init* and from versioning. Tests, DebugHost, and benchmark are not in this switch.

When on, the task library references `Microsoft.CodeAnalysis.PublicApiAnalyzers` 5.6.0 and sets `PublicApiDirectory` to `Properties/PublicAPI`. `dotnet new` does **not** stamp `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt`. `Properties/Build/InitializePublicApi.targets` (imported only when this switch is on **after generate**) writes those files on the first real build if they are missing, then runs `dotnet format analyzers --diagnostics RS0016`. AdditionalFiles are always listed so a first solution compile can see the paths. Nested format sets `PublicApiInitializing` and skips the target. Combo later libraries pass `--PublicApiAnalyzers` again; paths are per library (`src/prj/{Name}/Properties/PublicAPI/`).

In **template source** the `<!--#if (PublicApiAnalyzers) -->` markers are XML comments, so MSBuild always imports the targets (default after generate is still off). Design-time skips the target; a real stub `dotnet build` does not. The target no-ops when `../../../.template.config` exists. `src/prj/__SourceName__/Properties/PublicAPI/` in this host is a failed stamp - delete it, do not commit.

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --PublicApiAnalyzers
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --PublicApiAnalyzers
```

## `DocumentationTemplate`

Multi-choice, default **empty** (CLI) / **None** (Visual Studio). UI label **Documentation template**. CLI long name **`--DocumentationTemplate`**. Not part of `--InitAllRepoItems`. Same `TemplateAssets/DocShell.html` seed, two destinations. Extra sources copy that file only (`exclude` of `Versioning/**`, `CodeStyle/**`, `Licenses/**`, and `DirectoryMsBuild/**`). Do **not** vendor the 25-file offline site in the template: the HTML file is the bootstrap contract, so a later checkpoint run acquires the versions that file pins then, not whatever was frozen in this pack. A newer DocShell release is a copy/replace of `TemplateAssets/DocShell.html` (keep that filename). Do not rewrite internal bootstrap paths such as `./documentation/css` here; those change in the DocShell product file itself.

| Choice | Path | When | Combo later library |
| --- | --- | --- | --- |
| `Package` | `src/prj/{Name}/Properties/NugetMetadata/docs/DocShell.html` | every create | pass `Package` again |
| `Repository` | `docs/DocShell.html` | first create | omit `Repository` (Exit 73 if stamped again) |
| `None` | nothing | - | wins over the other choices |

```powershell
# combo gut
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Package Repository
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --DocumentationTemplate Package

# combo error (Exit 73) - Call 2 stamps docs/DocShell.html again
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Repository
# dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --DocumentationTemplate Repository
```

`Properties/NugetMetadata/docs` is packed with the nupkg (`PackagePath` empty, so `docs/` inside the package, not `Properties/`). Repo-root `docs/` is not packed. The checkpoint infers package vs repository documentation from `Properties/NugetMetadata/docs` vs repo-root `docs/`; it does not name this switch.

## Project roles and Git ignores

The library is packable. `IsPublishable` is `false` (NuGet pack is the distribution path). Single `TargetFramework` `netstandard2.0`, stock `<Project Sdk="Microsoft.NET.Sdk">`. There is no `PublishDefaultFramework` / `ImportSdkTargets` pair. `Properties/Build/` is authoring MSBuild (`PackAsMsBuildTask` last). `MSBuildTaskPackage/` is the authored consumer surface; its props and targets are packed to `build/`. `Properties/NugetMetadata/` is display/package metadata (readme, icon, notes, optional `docs/`). All three folders use their real on-disk locations so Explorer and Solution Explorer match; do not `Link` them elsewhere. `.project.editor.globalconfig`, `.config/dotnet-tools.json`, and `Directory.Build.*` stay next to the csproj. `Properties/AssemblyInfo.cs` grants `InternalsVisibleTo` the test assembly (`__SourceName__.Tests` via `sourceName`). The test project's root `AssemblyInfo.cs` is only MSTest `Parallelize`. DebugHost is always generated: in-repo MSBuild consumer (`OutputType` Library, no `Program.cs`). `Properties/launchSettings.json` lives on DebugHost (Executable `dotnet msbuild -t:RunDebugHostTask -p:BuildProjectReferences=false -nodeReuse:false`). `ReferenceOutputAssembly` is false. `UsingTask` is the fully specified `__SourceName__.AddTask`, `__SourceName__.TaskNodeTask`, `__SourceName__.HomeTask`, and `__SourceName__.DumpEnvVarsTask` against `bin/$(Configuration)/netstandard2.0/__SourceName__.dll`. `RunDebugHostTask` is an explicit target, not `AfterTargets="Build"`. Tests, DebugHost, and the optional BenchmarkDotNet executable explicitly set `IsPackable` and `IsPublishable` to `false`. Tests, DebugHost, and benchmark share one runnable TFM from the wizard (default `net10.0`). The task library itself is always `netstandard2.0` and does not multi-target. Versioning default is Nerdbank **Project** (`Properties/version.json`). Tests, DebugHost, and benchmark are not packable. **`--NerdbankGitVersioning`** `Off` is VersionPrefix; `Repo` is the shared root file.

**Test project layout.** Mini-scopes in this order: runnable TFM, language/debug (from `CSharpProjectOptions`), packaging, test configuration (MSTest logger), Coverlet `#if` block, ReportGenerator `#if` block, NugetReport + `WriteNugetReport`, `.gitignore` hide, `Resources/TestScript.msbuild` copy, `ProjectReference`, then **External dependencies** `PackageReference`s last. Coverage is **`--TestCoverage`**: `Coverlet` (default), `CoverletAndReport`, `None`. Do not restore independent Coverlet/ReportGenerator bools: ReportGenerator consumes `@(CoverletReport)`. Coverlet is `coverlet.msbuild` + `CollectCoverage=true`; that **does** run on `dotnet test` (VSTest path, SDK 10), including Linux/WSL (`dotnet` ships MSBuild). The Coverlet `#if` also writes `Include` `[__SourceName__]*` (sourceName → the task assembly), `Exclude` `[__SourceName__.DebugHost]*` (same glob would match the host assembly name if it were loaded), and `Threshold` `100` / `line,branch,method` / `total`. Those are generate-time properties, not wizard fields: Visual Studio cannot show extra inputs only when Coverlet is selected. `--TestCoverage None` omits the whole PropertyGroup. Lower the threshold in the test csproj when 100% is not yet the gate. The scaffold `AddTask`, `TaskNodeTask`, `HomeTask.Execute()`, and `DumpEnvVarsTask.Execute()` are covered in-process by `FunctionalTests` so a first `dotnet test` still passes. `HomeTask` OS probes and the missing-home warning use `[ExcludeFromCodeCoverage]`. `IntegrationTests` plus `Resources/TestScript.msbuild` are the automated MSBuild-consumer tests (not DebugHost). That subprocess does not count toward Coverlet. Do not `ProjectReference` DebugHost from tests. Tests and the optional benchmark compile against `Microsoft.Build.Utilities.Core` 15.9.20 (runtime included) so in-process `Execute()` can see the `Task` base type; do not add `Microsoft.Build` (the engine) unless a later test needs `ProjectInstance`. DebugHost does not take that package: it loads the task DLL through MSBuild. Report/logger/NugetReport paths use `$([MSBuild]::NormalizeDirectory(...))` so Linux does not create a folder named `ReportGeneratorOutput\net10.0`.

**Why there is no `PublishDefaultFramework`.** The task library is one TFM (`netstandard2.0`). Stock `Project Sdk="Microsoft.NET.Sdk"` is enough. Tests do not `SetTargetFramework` on the task `ProjectReference`. `SourceControlState.targets` is a `BeforeTargets` hook on `GenerateAssemblyInfo` (SDK 8+ Source Link). `InitializePublicApi.targets` is imported only when `PublicApiAnalyzers` is on. `PackAsMsBuildTask.targets` follows the official custom-task pack layout: `BuildOutputTargetFolder` `tasks`, `CopyLocalLockFileAssemblies`, `GenerateDependencyFile`, runtime assets of private dependencies next to the DLL, `deps.json` in the package. Consumer `build/{Name}.props` is the PackageReference `UsingTask` surface: `$(MSBuildThisFileName).dll` under `../tasks/netstandard2.0/` with a fully specified `TaskName`. Consumer `build/{Name}.targets` holds the sample `BeforeTargets=CoreCompile` populate. Compile against `Microsoft.Build.Utilities.Core` **15.9.20** (`PrivateAssets` all, `ExcludeAssets` runtime) so VS 2017+ / MSBuild 15 and current `dotnet` can load the task; do not bump that version without raising the minimum host. Do not reference `Microsoft.Build` (the engine) unless a later sample needs `ProjectInstance`. `IsTool` is not used. `DevelopmentDependency` and `SuppressDependenciesWhenPacking` are on. `NoWarn` includes `NU5128` (no `lib/` group) and `NU5100` (assemblies under `tasks/` are not library dependencies). `NU5102` is unused without `IsTool`. Distribution is `dotnet pack`; `IsPublishable` stays `false`.

Library, tests, DebugHost, and optional benchmark each remove `.gitignore` from their `None` items so it stays on disk without appearing as a project item. The test ignore also covers generated `NugetReport/` output.

The packable library sets `EnablePackageValidation` to `false` (no `lib/` TFMs). Tests, DebugHost, and benchmark are not packable.

With `PlaceSolution` `SlnFolder` (default), each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/` next to that library's `.slnx` and handbook `Readme.md`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes (no nested `.gitignore`); delete that folder if you do not need it. `BesideCsproj` writes the `.slnx` and handbook next to the packable csproj and does **not** create `src/sln/{Name}/` (`src/prj/{Name}/.gitignore` already ignores `.vs/`). Repository-root `.gitignore` is `InitRepoItems` `GitIgnore` / `--InitAllRepoItems` (first create only). Later libraries omit Init and do not overwrite it. If root ignore is off, the `RepoRoot` variant and Visual Studio's extra root `.vs/` stay the owner's problem.

## `DirectoryMsBuildFiles`

Library + this library's `.slnx` only. Bool, default **false**. UI label **Empty Directory.Build and Directory.Solution files**. CLI long name **`--DirectoryMsBuildFiles`**. Omit the switch → nothing. Combo-safe on `SlnFolder` and `BesideCsproj` (paths include the library name). `RepoRoot` stacks `Directory.Solution.*` at the repository root like stacking `.slnx` files. Not an Init* item and not `Directory.Packages.props` (CPM would not cover sibling tests). Tests, DebugHost, and benchmark are not in this switch.

Seeds live under `TemplateAssets/DirectoryMsBuild/`. Extra sources copy `Directory.Build.props` / `.targets` next to the library csproj, and `Directory.Solution.props` / `.targets` next to this library's `.slnx` (`src/sln/{Name}/` for `SlnFolder`, `src/prj/{Name}/` for `BesideCsproj`, repo root for `RepoRoot` - later library then collides, same as stacking `.slnx` files). DocShell extra sources must `exclude` `Versioning/**` and `DirectoryMsBuild/**`.

The files are almost empty `<Project>` stubs with comments. MSBuild auto-imports them from those directories. Do not move `PackAsMsBuildTask` / analyzer config into them. The template does not `#if` properties into these files vs the csproj (possible, ugly). The library csproj `None Include`s the two `Directory.Build.*` files with `Link` under `Properties\` (Solution Explorer only). Do **not** move the files into `Properties/` on disk: auto-import follows the directory of the file, same as `.project.editor.globalconfig`. `Directory.Solution.*` stay beside the `.slnx`. When `PlaceSolution` is `BesideCsproj`, the library csproj also `Link`s those two solution files (same folder as the csproj) so they do not look like stray project items.

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DirectoryMsBuildFiles
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --DirectoryMsBuildFiles
```

## `DotNetToolManifest`

Library project only. Bool, default **true**. UI label **Empty local dotnet-tools.json**. CLI long name **`--DotNetToolManifest`**. Omit the switch → empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it. Combo-safe: path includes the library name. Not Init. `tools` is `{}`; no `dotnet tool restore` on build. Tests, DebugHost, and benchmark are not in this switch. `isRoot` is true so a later parent manifest does not merge in. `dotnet tool install --local` from the library folder fills the file. With `SlnFolder`, the handbook CWD (`src/sln/{Name}/`) does not see this manifest. With `BesideCsproj`, the handbook CWD is the library folder and does.

The library csproj `None Include`s the file with `Link` under `Properties\` (Solution Explorer only). Disk path stays `.config/`.

```powershell
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "abcd" --name "...Library2" --output $out --DotNetToolManifest false
```

## Other symbols worth not breaking

- **`ProjectLicense` / `RepoLicense` / `InitAllRepoItems` / `GitIgnore` / `SrcGlobalJson`**: see section above. Default MIT. SPDX expression for standards; `PackageLicenseFile` only for `Custom`. Root `LICENSE` is the `RepoLicense` item or the CLI all-set. Root `.gitignore` is `GitIgnore` in that same first-create set. This template does not stamp `ChoosingPackageBoundaries.md`. `src/global.json` is `SrcGlobalJson`: opt-in, default off, not in `--InitAllRepoItems`; pin follows DebugHost TFM, not netstandard2.0.
- **`NerdbankGitVersioning` / `WriteRepoVersionJson`**: see section above. Default `Project`. Root `version.json` is first-create only (`Repo` plus Init).
- **`CSharpProjectOptions`**: see section above. Do not split back into per-property dropdowns.
- **`ProjectEditorGlobalConfig`**: see section above. Default `Strict`. `Default` is suggestions-only. Seeds in `TemplateAssets/CodeStyle/`; generated disk name stays `.project.editor.globalconfig`. Csproj wire-up on the library only.
- **`AnalysisMode`**: see section above. Default `Recommended`. Library only. CA warnings, not build errors.
- **`NuGetAuditHighCriticalAsErrors`**: see section above. Default `true`. Library only. NU1903/NU1904 as restore errors. Off for EOL/backport graphs.
- **`PublicApiAnalyzers`**: see section above. Default `false`. Library only. Baseline files are first-build, not generate-time.
- **`DocumentationTemplate` / `WritePackageDocTemplate` / `WriteRepoDocTemplate`**: see section above. Default empty/`None`. Seed only; not the vendored site.
- **`PlaceSolution`**: single choice, default `SlnFolder` → `src/sln/__SourceName__/__SourceName__.slnx` plus handbook `Readme.md` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). `RepoRoot` on CLI renames to a root `.slnx`; `RepoRoot` in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS's conventional root `.slnx`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes and stacks every library's `.slnx` in one directory. `BesideCsproj` writes `src/prj/{Name}/{Name}.slnx` and the handbook next to the packable csproj (not tests, DebugHost, or benchmark) and does not create `src/sln/{Name}/`. The `.slnx` virtual folder `/sln/{Name}/` is omitted for `BesideCsproj`; `Readme.md` is a solution item beside the file.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`Author`**: required. CLI `--Author`. `isRequired` is a JSON boolean (`true`), not the string `"true"`.
- **`TargetFrameworks`**: single-select DebugHost/tests/benchmark TFM. CLI long name `--DebugHostTargetFramework`. Do not rename the symbol: copy-templates share this name. `TargetFrameworksValue` still joins `__TargetFrameworks__` even though this scaffold uses singular `__TargetFramework__`.
- **`<Description>`**: not a template parameter. Generate leaves an empty CDATA block for multiline gallery text; the checkpoint fills it (assistant-supported).
- **`EnablePackageValidation`**: not a symbol. Always `false` on the task library (no `lib/` TFMs).
- **`TestCoverage`**: single choice, default `Coverlet`. Replaces the two independent Coverlet/ReportGenerator bools. Coverage stats on `dotnet test` are opt-out; ReportGenerator is opt-in (`CoverletAndReport`). There is no Report-without-Coverlet. `--TestCoverage None` drops Coverlet too. Computed `CoverletMSBuild` / `ReportGenerator` drive the test csproj and sln-readme `#if`s.
- **`DirectoryMsBuildFiles`**: see section above. Default `false`. Empty `Directory.Build.*` beside the library and `Directory.Solution.*` beside this `.slnx`. No `Directory.Packages.props`.
- **`DotNetToolManifest`**: see section above. Default `true`. Empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it.
- Conditionals in `.md` / `.slnx` / `.targets` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.txt` and the renamed root `LICENSE` use `//#if`. License seeds under `TemplateAssets/Licenses/` may use a shallow copyright `//#if` / `//#else`; extra sources pick the file so there is no `ProjectLicense` `#if` in the text. This template does not use `UseWebSdk`. Stock `Project Sdk="Microsoft.NET.Sdk"`. .targets stays registered so later generate-time hash-if in `Properties/Build/*.targets` still evaluate.
