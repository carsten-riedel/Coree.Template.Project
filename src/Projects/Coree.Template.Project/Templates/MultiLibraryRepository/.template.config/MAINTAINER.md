# `.template.config` (MultiLibraryRepository)

Product surface: **`.NET multi-library repository`**. `identity` is `CoreeTemplatesProjectMultiLibraryRepository`; CLI short name is `multilibraryrepo-coree`.

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. Host mapping: **CLI ↔ Visual Studio**. |
| `dotnetcli.host.json` | CLI long names; empty `shortName` for `InitRepoItems`, `InitAllRepoItems`, `CSharpProjectOptions`, and `ProjectLicense` so they do not steal single-letter aliases. |
| `MAINTAINER.md` | This file. |

## Intended usage

The template bootstraps a **repository layout** for one or more packable class libraries (1:n split of a too-large library). It does not `git init`. Same `--output` = combo repo; different `--output` = separate repos.

`PackageAuthor` is required on every create. Everyday CLI is author, name, output; root files only on the first create into an empty folder.

Install from this folder (or from the packed `Coree.Template.Project` nupkg):

```powershell
dotnet new install "C:\dev\github.com\carsten-riedel\Coree.Template.Project\src\Projects\Coree.Template.Project\Templates\MultiLibraryRepository" --force
```

Combo repo, three libraries, root files only once:

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin" --InitAllRepoItems
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary2" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary3" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
```

After the first call the repo root has `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, and `LICENSE`. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitAllRepoItems` or `--InitRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite.

`--InitAllRepoItems` is the CLI first-create set (same four files as Visual Studio). `--InitRepoItems` picks individual files. Values are separated by **spaces**. Repeating `--InitRepoItems` per value also works. A quoted `Readme|AIReleaseCheckpoint|GitAttributes|RepoLicense` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "<repo>" --InitRepoItems AIReleaseCheckpoint
```

**Visual Studio:** first create uses the `ide.host.json` default (same four root files as `--InitAllRepoItems`). A second library in the IDE cannot omit the group: choose **None**. Folgelibraries in the same folder are otherwise the CLI path above. Why the two hosts differ is in **CLI ↔ Visual Studio** below.

## CLI ↔ Visual Studio

The generated first-create product is meant to match. The **switches** cannot be identical, because the hosts do not have the same empty-set, default, or repeat-create rules. Do not “fix” this by making `template.json` `defaultValue` equal the Visual Studio default.

### Why CLI defaults stay empty

A combo repository is two or more `dotnet new` calls into the **same** `--output`. The template engine has **one** CLI default for every call. If `InitRepoItems` defaulted to the four root files, the second library would hit Exit 73 (collision) unless the caller passed `None` or `--force`. Visual Studio’s New Project dialog is a **first create** into an empty folder; it can check the four boxes by default. CLI later-libraries are the empty default: omit `--InitAllRepoItems` and `--InitRepoItems`.

### CLI → Visual Studio

| CLI | Visual Studio equivalent | Why |
| --- | --- | --- |
| `--InitAllRepoItems` | Leave **Repository root items** at the ide.host default (all four files checked) | Bool flag with no value list. The engine cannot treat a bare `--InitRepoItems` as “all”; that is Exit 127. The set is the VS first-create default, not a fifth checkbox. |
| `--InitRepoItems Readme …` (spaces) | Uncheck the files you do not want | Individual files. `|` is only legal in `ide.host.json` `defaultValue`, not on current `dotnet new`. |
| omit both switches | **None** | CLI may leave a multi-choice empty. Visual Studio may not. |
| `--InitRepoItems None` | **None** | Explicit empty set. Also wins over `--InitAllRepoItems` if both are passed. Everyday CLI later-libraries omit the switches instead. |
| `--InitAllRepoItems` hidden from the wizard | `ide.host.json` `isVisible: false` | A choice `All` inside the same VS group would sit next to `None` and the four files; you cannot hide one choice per host. The bool is CLI convenience only. |

### Visual Studio → CLI

| Visual Studio | CLI equivalent | Why |
| --- | --- | --- |
| First create, root items left at default | `--InitAllRepoItems` | Same four files. Do not translate the ide.host string `Readme\|…\|RepoLicense` onto the CLI. |
| Uncheck some root items | `--InitRepoItems` plus the remaining choice names | Subset. |
| Second library: **None** | omit `--InitAllRepoItems` and `--InitRepoItems` | The IDE requires at least one value; leftover checks from persistence would otherwise stamp root files again. `persistenceScope: none` still needs **None** as the empty-set control. CLI empty default is that None. |
| `InitAllRepoItems` not shown | do not look for it in Additional information | CLI-only. |

`persistenceScope: none` on the VS symbols that have custom defaults: the dialog must not reuse the last create (especially **None** or a subset) as the next “first create”.

Other host-only switch behavior (not root files, same class of reason):

- **`PlaceSolutionInSolutionFolder` false:** CLI renames to `{Name}.slnx` at repo root. Visual Studio keeps `{Name}.generated.slnx` so it does not overwrite the `{Name}.slnx` the IDE always writes. Post-actions that open the sln readme and tell you to close/reopen are `HostIdentifier == "vs"` only.
- **`CSharpProjectOptions` / TFMs / `ProjectLicense`:** same defaults on both hosts. They do not write shared files that collide on a later library, so they do not need the empty-CLI / full-VS split.
- **`PackageAuthor`:** required on both.

## `InitRepoItems` / `InitAllRepoItems`

`InitRepoItems` is the multi-choice (`allowMultipleValues`), not N bools. Visual Studio shows **one group** of checkboxes (same shape as target frameworks). `InitAllRepoItems` is a CLI bool for that same first-create set; `ide.host.json` hides it.

| Host | Default | Empty set |
| --- | --- | --- |
| CLI `InitRepoItems` | none selected (`defaultValue` `""`) | omit the switch |
| CLI `InitAllRepoItems` | `false` | omit the switch |
| Visual Studio | `Readme\|AIReleaseCheckpoint\|GitAttributes\|RepoLicense` | not allowed; choose `None` |

`None` is first in the choice list. `sources` exclude each root file unless `InitAllRepoItems` is on or that choice is selected; `None` excludes all of them, including when `InitAllRepoItems` is on. `==` in conditions means the value is among the selected choices.

## AI-supported release checkpoint

The generated product still contains placeholders that can only become true **after implementation** — especially empty `src/prj/*/NugetAssets/Readme.md`. A human or an LLM can fill those from the code. That is a **gate before the first publish**, not a generate-time script and not standing agent rules.

**Not:** run-once / post-bootstrap right after `dotnet new`. The library may still be `Class1`.  
**Not:** a forever queue in the GitHub `README.md`. That file is the customer landing page.  
**Not:** a template-stamped `AGENTS.md`. That would collide with the consumer’s own agent file and would outlive the scaffold.

**Yes:** one repo-root file, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, stamped only when `InitAllRepoItems` is on or `InitRepoItems` includes `AIReleaseCheckpoint` (first create). `TEMPLATE-` marks it as delete-me scaffold; `AI` matches the VS choice. It is a **Template-Checkpoint-Release**: close template residue, then **self-dissolve**. After that, new chats read the libraries and the real NuGet docs. The file is written for a person; an assistant can fill it from the product. It is not a prompt and not standing agent rules.

Why the repo root, not `src/sln/{Name}/`: the first look at a combo repo is the customer surface; one fat checklist can say “update every NuGet readme in this repository” without a per-library marker. Libraries added later are in scope until the file is deleted.

The VS label **AI-supported release checkpoint** names the *job*, not a recurring agent run. The switch does not start a model. Someone later (person or LLM) works that file to 100% observable items, then deletes it. “AI-supported” belongs in the choice display name; it must not read as “edit with AI on every create.”

The generated file is the contract (ten numbered, checkable items). Do not put free-form “run this shell” instructions in it (prompt injection). Do not mix standing style rules into it — those must not self-delete.

## `CSharpProjectOptions`

One multi-choice (`allowMultipleValues`), same VS checkbox combobox as TFMs and `InitRepoItems`. Not four/five separate dropdowns.

| Choice | Checked (default) | Unchecked |
| --- | --- | --- |
| `DisableImplicitUsings` | `disable` | `enable` |
| `Nullable` | `enable` | `disable` |
| `LangLatest` | `latest` | `default` (TFM C# version; valid compiler value) |
| `DebugEmbedded` | `embedded` | `none` |
| `GenerateDocumentationFile` | `true` | `false` |

Language/debug values are **always written** (no omitted PropertyGroup) into the library, tests, and benchmark csproj. `GenerateDocumentationFile` is in this same choice list but only the **library** writes it (own PropertyGroup). Tests and benchmark have no XML-docs surface; default `true` there is CS1591 on public types without comments.

No `None`. CLI and VS default is the five product values. Visual Studio cannot leave a multi-choice empty; at least one box stays checked. CLI: omit the switch, or pass values with **spaces** (`--CSharpProjectOptions Nullable LangLatest DebugEmbedded`). `|` is only the host default separator.

Benchmark previously hardcoded `ImplicitUsings` enable. It now follows the switch. `Program.cs` has explicit `System` / `System.IO` / `System.Linq` usings so the default (`disable`) still compiles.

## `ProjectLicense`

Single choice (dropdown, not a checkbox group). Project + NuGet only. Repository-root `LICENSE` is `InitRepoItems` choice `RepoLicense` (UI: **LICENSE file at repository root**) or `--InitAllRepoItems`, not a second VS bool.

| Choice | `NugetAssets/License.txt` | NuGet |
| --- | --- | --- |
| `MIT` (CLI/VS default) | MIT text on disk, not packed | `PackageLicenseExpression` `MIT` |
| `BSD3Clause` | BSD 3-Clause text on disk, not packed | `PackageLicenseExpression` `BSD-3-Clause` |
| `Apache2` | Apache 2.0 text on disk, not packed | `PackageLicenseExpression` `Apache-2.0` |
| `Custom` | copyright notice only; packed | `PackageLicenseFile` `License.txt` |

Do not set `PackageLicenseFile` together with an expression (NU5033). The glob excludes `License.txt` except for `Custom`.

`RepoLicense` is off on CLI unless listed in `--InitRepoItems` or `--InitAllRepoItems` is on. Visual Studio includes it in the first-create default. A second source then stamps the same text as repository-root `LICENSE` (GitHub convention, no extension). First create only; a later library with `RepoLicense` or `InitAllRepoItems` selected collides (Exit 73), same as root README. `None` excludes it even if leftover checks remain.

## `ProjectEditorGlobalConfig`

Library-only bool, default **true**. UI label **Code style rules for library project** (packable class-library project only; `is_global` is the analyzer-config technical term). Drops `src/prj/{Name}/.project.editor.globalconfig` and wires `GlobalAnalyzerConfigFiles` plus `EnforceCodeStyleInBuild`. Tests and benchmark do not get the file. Keep the symbol and disk name. Do not use a bare "project" label: **C# project options** already applies to library, tests, and benchmark.

The file is a VS-exported style dump (`is_global = true`), not an always-fail naming probe. Naming rules in `.globalconfig` do **not** fail `dotnet build`. `EnforceCodeStyleInBuild` is the only non-default that matters (SDK default is false); without it the file stays IDE-only. Do not add `EnableNETAnalyzers` / `RunAnalyzers*` `true` noise — those already default true on net8/net10.

This PropertyGroup/ItemGroup must appear **before** `ImportSdkTargets`. After that import the SDK has already loaded and ignores the items.

## Project roles and Git ignores

The library is packable. `IsPublishable` is `false` on the class library (NuGet pack is the distribution path). Set it `true` to use the existing `PublishDefaultFramework` dispatch. Tests and the optional BenchmarkDotNet executable explicitly set `IsPackable` and `IsPublishable` to `false`, including when automation calls each `.csproj` directly. The benchmark keeps one target framework (the highest selected) and runs with `dotnet run -c Release`. Versioning (`VersionPrefix` / `PackageVersion` / …) stays **library-only**; tests and benchmark are not packable.

**Test project layout.** Mini-scopes in this order: general TFMs, language/debug (from `CSharpProjectOptions`), packaging, test configuration (`TestTfmsInParallel` + MSTest logger), Coverlet `#if` block, ReportGenerator `#if` block, NugetReport + `ListVulnerable`, `.gitignore` hide, `ProjectReference`, then **External dependencies** `PackageReference`s last. Coverlet is `coverlet.msbuild` + `CollectCoverage=true`; that **does** run on `dotnet test` (VSTest path, SDK 10), including Linux/WSL (`dotnet` ships MSBuild). Report/logger/NugetReport paths use `$([MSBuild]::NormalizeDirectory(...))` so Linux does not create a folder named `ReportGeneratorOutput\net10.0`.

**Why `PublishDefaultFramework` exists.** When `IsPublishable` is `true`, generated libraries are used as `dotnet pack` and `dotnet publish` with no `-f` and no extra properties. Pack must include every selected TFM; publish must write one default TFM to `bin/Publish`. The SDK does the pack side from `TargetFrameworks` alone. It does **not** do the publish side: multi-targeting `Publish` is NETSDK1129 unless the caller passes a framework. The dispatch is that default (highest selected TFM, `__TargetFramework__`). Restore, build, test, pack, and `ProjectReference` stay on the stock SDK.

Do not put `TargetFramework` next to `TargetFrameworks` to avoid `-f`. That was the previous library: MSBuild saw a single TFM, pack needed `BuildForPack`, and a `net8.0` consumer could not reference the project. `_IsPublishing` on `TargetFramework` still fails when that consumer publishes (the flag is global).

Implementation (do not “simplify” into one always-imported file or back to `<Project Sdk="...">`): `ImportSdkTargets.targets` always closes `Sdk.targets`; `PublishDefaultFramework.targets` loads only when `IsCrossTargetingBuild` is true. **`ImportSdkTargets` must be the last import in the library csproj.** That file *is* `Sdk.targets` plus the outer publish dispatch. The SDK reads properties and items while it loads (`EnforceCodeStyleInBuild`, `GlobalAnalyzerConfigFiles`, publish). Anything after that line is after the SDK and is ignored for those. `Project Sdk="..."` would append `Sdk.targets` after this file and overwrite the Publish override. `SourceControlState.targets` is a `BeforeTargets` hook on `GenerateAssemblyInfo` (SDK 8+ Source Link); it can sit just above the SDK close. `PublishRelease` keeps a direct project `dotnet publish` on Release. Tests may keep `SetTargetFramework`; external consumers must not need it. `<!--#if` in `.targets` is generate-time (`**/*.targets` in `specialCustomOperations`).

All three project files remove `.gitignore` from their `None` items so it stays on disk without appearing as a project item. The test ignore also covers generated `NugetReport/` output.

With `PlaceSolutionInSolutionFolder=true`, each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/`. This creates no shared files on later library additions. The root-solution variant leaves repository-root ignore policy to the repository owner; it does not create or overwrite a shared root `.gitignore`. The same applies to the temporary root `.vs/` left by Visual Studio's extra solution.

## Other symbols worth not breaking

- **`ProjectLicense` / `RepoLicense` / `InitAllRepoItems`**: see section above. Default MIT. SPDX expression for standards; `PackageLicenseFile` only for `Custom`. Root `LICENSE` is the `RepoLicense` item or the CLI all-set.
- **`CSharpProjectOptions`**: see section above. Do not split back into per-property dropdowns.
- **`ProjectEditorGlobalConfig`**: see section above. Keep the file and the csproj wire-up on the library only, before `ImportSdkTargets`.
- **`PlaceSolutionInSolutionFolder`**: default true → `src/sln/ClassLibrary/ClassLibrary.slnx` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). False on CLI renames to a root `.slnx`; false in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS’s conventional root `.slnx`. False also stacks every library’s `.slnx` in one directory.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`PackageAuthor`**: required.
- Conditionals in `.md` / `.slnx` / `.targets` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.targets` is how `UseWebSdk` selects `Sdk.targets` in `ImportSdkTargets.targets`.
