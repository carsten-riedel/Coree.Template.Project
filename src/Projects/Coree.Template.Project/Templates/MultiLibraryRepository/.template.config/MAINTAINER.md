# `.template.config` (MultiLibraryRepository)

Product surface: **`.NET multi-library repository`**. `identity` is `CoreeTemplatesProjectMultiLibraryRepository`; CLI short name is `multilibraryrepo-coree`.

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. Host mapping: **CLI ↔ Visual Studio**. |
| `dotnetcli.host.json` | CLI long names; empty `shortName` for `InitRepoItems`, `InitAllRepoItems`, `CSharpProjectOptions`, `ProjectLicense`, `NerdbankGitVersioning`, `PublicApiAnalyzers`, and `DocumentationTemplate` so they do not steal single-letter aliases. |
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

### CLI use cases

`$out` is the same folder for a combo repo. Different `--output` = separate repos. `--NerdbankGitVersioning` default is `Off` (VersionPrefix, no extra package). Omit it unless you want Nerdbank.GitVersioning.

**Default, one library**

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output $out --InitAllRepoItems
```

Root: `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `LICENSE`. Library: VersionPrefix group. No `version.json`.

**Default, multi-library (combo)**

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

Call 1 writes the four root files. Call 2 only adds `src/prj` / `src/sln`. Both libraries stay on VersionPrefix.

**Nerdbank repository, multi-library**

```powershell
# combo gut — Call 2 only wires the library; generate does not stamp version.json again
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

**Nerdbank project folder, multi-library**

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Project

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md again; the per-library version.json paths do not collide)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
```

**Two separate repos (not a combo)**

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.LibA" --output $outA --InitAllRepoItems
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.LibB" --output $outB --InitAllRepoItems
```

Each `--output` is its own first create.

After the first call in the default combo the repo root has `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, and `LICENSE`. The library keeps the VersionPrefix group. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitAllRepoItems` or `--InitRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite.

`--InitAllRepoItems` is the CLI first-create set (same four files as Visual Studio). It does **not** add `version.json`. `--InitRepoItems` picks individual files. Values are separated by **spaces**. Repeating `--InitRepoItems` per value also works. A quoted `Readme|AIReleaseCheckpoint|GitAttributes|RepoLicense` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "<repo>" --InitRepoItems AIReleaseCheckpoint
```

**Visual Studio:** first create uses the `ide.host.json` default (same four root files as `--InitAllRepoItems`). A second library in the IDE cannot omit the group: choose **None**. Folgelibraries in the same folder are otherwise the CLI path above. Why the two hosts differ is in **CLI ↔ Visual Studio** below.

## CLI ↔ Visual Studio

The generated first-create product is meant to match. The **switches** cannot be identical, because the hosts do not have the same empty-set, default, or repeat-create rules. Do not “fix” this by making `template.json` `defaultValue` equal the Visual Studio default.

### Why CLI defaults stay empty

A combo repository is two or more `dotnet new` calls into the **same** `--output`. The template engine has **one** CLI default for every call. If `InitRepoItems` defaulted to the four root files, the second library would hit Exit 73 (collision) unless the caller passed `None` or `--force`. Visual Studio’s New Project dialog is a **first create** into an empty folder; it can check the four boxes by default. CLI later-libraries omit `--InitAllRepoItems` and `--InitRepoItems`. `NerdbankGitVersioning` defaults to **`Off`** on both hosts. `--NerdbankGitVersioning Repo` does not write the root `version.json` by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again without Exit 73.

### CLI → Visual Studio

| CLI | Visual Studio equivalent | Why |
| --- | --- | --- |
| `--InitAllRepoItems` | Leave **Repository root items** at the ide.host default (all four files checked) | Bool flag with no value list. The engine cannot treat a bare `--InitRepoItems` as “all”; that is Exit 127. The set is the VS first-create default, not a fifth checkbox. `version.json` is not in this set. |
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
- **`CSharpProjectOptions` / TFMs / `ProjectLicense` / `NerdbankGitVersioning` / `PublicApiAnalyzers` / `DocumentationTemplate`:** same defaults on both hosts (`NerdbankGitVersioning` `Off`, `PublicApiAnalyzers` `false`, `DocumentationTemplate` empty/`None`). `--NerdbankGitVersioning Repo` does not write the root file by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again. `--DocumentationTemplate Package` is safe on later libraries; `--DocumentationTemplate Repository` on a later library is Exit 73.
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

## `NerdbankGitVersioning`

Single choice (dropdown), same shape as `ProjectLicense`. CLI long name is **`--NerdbankGitVersioning`** so the extra NuGet dependency is visible. Default **`Off`**: the existing VersionPrefix group, no package. `Repo` or `Project` adds `Nerdbank.GitVersioning`. `--InitAllRepoItems` is the four root files only; it does **not** turn Nerdbank on and does **not** copy `version.json`. Tests and benchmark are not in this switch.

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

Call 1: four root files plus root `version.json`, library wire is Nerdbank repo. Call 2: `--NerdbankGitVersioning Repo`, **no** Init, **no** second root stamp (Exit 0). `--NerdbankGitVersioning Project` instead puts `version.json` in that library's `Properties/` folder (every create; paths do not collide). Omit `--NerdbankGitVersioning` for the VersionPrefix group.

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
      || InitRepoItems == RepoLicense)
```

`UseNerdbankGitVersioning` is generate-time `<!--#if` in `ClassLibrary.csproj`. The VersionPrefix group is the `#else` (`Off`). Both branches stay in the **template source**; `dotnet new` keeps one.

`None` wins over InitAll. A second **generate-time** write of root `version.json` is Exit 73. `--NerdbankGitVersioning Repo` without Init does not stamp the file at `dotnet new`. `Build/NerdbankRepositoryVersion.targets` (imported only for Repo) copies `Build/Nerdbank.version.json` to the repository root **if it does not exist**, before Nerdbank reads it. A later library in the same folder therefore does not collide.

There is no `version.json` checkbox in `InitRepoItems`. Generate-time root file is `WriteRepoVersionJson` (`Repo` plus first-create Init). If that file is still missing, the Repo library writes it once at build (`if not exists`). Later VS library: **None** plus **Repository version.json**.

Canonical JSON is `VersioningAssets/version.json` (`0.1.0`, `pathFilters` `["."]`). `Off` is first in the choice list because it is the default.

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
| `Repo` | false | `Readme` / checkpoint / `.gitattributes` / `RepoLicense` (not `None`) | write |
| `Repo` | true or false | `None` | skip |
| `Project` | true or false | any | skip |
| `Off` | true or false | any | skip |

### `NerdbankGitVersioning` → library tree (Init ignored except root json above)

| `NerdbankGitVersioning` | UI | VersionPrefix group | `GitVersionBaseDirectory` | `Properties/version.json` |
| --- | --- | --- | --- | --- |
| `Off` (default, first) | **Off (VersionPrefix in the library project)** | yes | no | skip |
| `Project` | **Properties/version.json** | no | `Properties` | write |
| `Repo` | **Repository version.json** | no | no | skip |

### Everyday CLI

**Default, multi-library**

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Off` | skip | yes |
| 2 | false | `Off` | skip | yes |

**Nerdbank repository, multi-library** (no collision on call 2)

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Repo` | write | no |
| 2 | false | `Repo` | skip | no |

**Nerdbank project folder, multi-library**

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Project

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, TEMPLATE-AI-RELEASE-CHECKPOINT.md again; the per-library version.json paths do not collide)
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Project
```

### Combos that look successful but are incomplete or mixed

`dotnet new` Exit 0 only means no file collision. It does not mean every library in `$out` uses the same versioning.

| Calls | Exit | What is wrong |
| --- | --- | --- |
| `--NerdbankGitVersioning Repo` on an empty folder, no Init | 0 | `dotnet new` does not stamp root `version.json`. First build/pack writes it if missing. |
| `--InitAllRepoItems --NerdbankGitVersioning Repo --InitRepoItems None` | 0 | `None` wins; four root files skipped and no root `version.json`. |
| Call 1 InitAll (Off), call 2 `--NerdbankGitVersioning Repo` | 0 | Library 1 VersionPrefix, library 2 Nerdbank walking up with no root json. |
| Call 1 InitAll `--NerdbankGitVersioning Repo`, call 2 omit the switch | 0 | Library 1 Nerdbank + root json, library 2 VersionPrefix. |
| Call 1 InitAll `--NerdbankGitVersioning Project`, call 2 `--NerdbankGitVersioning Repo` (or the reverse) | 0 | Mixed `Properties/version.json` and repo-style package with no matching root file. |

Keep the same `--NerdbankGitVersioning` value on every library in one `--output`. Root `version.json` only on the first create together with Init.

## `ProjectEditorGlobalConfig`

Library-only bool, default **true**. UI label **Code style rules for library project** (packable class-library project only; `is_global` is the analyzer-config technical term). Drops `src/prj/{Name}/.project.editor.globalconfig` and wires `GlobalAnalyzerConfigFiles` plus `EnforceCodeStyleInBuild`. Tests and benchmark do not get the file. Keep the symbol and disk name. Do not use a bare "project" label: **C# project options** already applies to library, tests, and benchmark.

The file is a VS-exported style dump (`is_global = true`), not an always-fail naming probe. Naming rules in `.globalconfig` do **not** fail `dotnet build`. `EnforceCodeStyleInBuild` is the only non-default that matters (SDK default is false); without it the file stays IDE-only. Do not add `EnableNETAnalyzers` / `RunAnalyzers*` `true` noise — those already default true on net8/net10.

This PropertyGroup/ItemGroup must appear **before** `ImportSdkTargets`. After that import the SDK has already loaded and ignores the items.

## `PublicApiAnalyzers`

Library-only bool, default **false**. UI label **Public API analyzers**. CLI long name **`--PublicApiAnalyzers`**. Separate from repository-root Init* and from versioning. Tests and benchmark are not in this switch.

When on, the class library references `Microsoft.CodeAnalysis.PublicApiAnalyzers` 5.6.0 and sets `PublicApiDirectory` to `Properties/PublicAPI`. `dotnet new` does **not** stamp `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt`. `Build/InitializePublicApi.targets` (imported only when this switch is on, before `ImportSdkTargets`) writes those files on the first real build if they are missing, then runs `dotnet format analyzers --diagnostics RS0016`. AdditionalFiles are always listed so a first solution compile can see the paths. Nested format sets `PublicApiInitializing` and skips the target. Sibling inner TFMs wait until Unshipped has API lines. Combo later libraries pass `--PublicApiAnalyzers` again; paths are per library (`src/prj/{Name}/Properties/PublicAPI/`).

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --PublicApiAnalyzers
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --PublicApiAnalyzers
```

## `DocumentationTemplate`

Multi-choice, default **empty** (CLI) / **None** (Visual Studio). UI label **Documentation template**. CLI long name **`--DocumentationTemplate`**. Not part of `--InitAllRepoItems`. Same `DocumentationAssets/DocTemplate.html` seed, two destinations. Do **not** vendor the 25-file offline site in the template: the HTML file is the bootstrap contract, so a later checkpoint run acquires the versions that file pins then, not whatever was frozen in this pack.

| Choice | Path | When | Combo later library |
| --- | --- | --- | --- |
| `Package` | `src/prj/{Name}/NugetAssets/documentation/DocTemplate.html` | every create | pass `Package` again |
| `Repository` | `documentation/DocTemplate.html` | first create | omit `Repository` (Exit 73 if stamped again) |
| `None` | nothing | — | wins over the other choices |

```powershell
# combo gut
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Package Repository
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --DocumentationTemplate Package

# combo error (Exit 73) — Call 2 stamps documentation/DocTemplate.html again
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Repository
# dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "...Library2" --output $out --DocumentationTemplate Repository
```

`NugetAssets/documentation` is packed with the nupkg (`PackagePath` empty, so `documentation/` inside the package). Repo-root `documentation/` is not packed. The name is not GitHub Pages `docs/`. The checkpoint infers package vs repository documentation from those locations; it does not name this switch.

## Project roles and Git ignores

The library is packable. `IsPublishable` is `false` on the class library (NuGet pack is the distribution path). Set it `true` to use the existing `PublishDefaultFramework` dispatch. `Properties/AssemblyInfo.cs` grants `InternalsVisibleTo` the test assembly (`ClassLibrary.Tests` via `sourceName`). The test project’s root `AssemblyInfo.cs` is only MSTest `Parallelize`. Tests and the optional BenchmarkDotNet executable explicitly set `IsPackable` and `IsPublishable` to `false`, including when automation calls each `.csproj` directly. The benchmark keeps one target framework (the highest selected) and runs with `dotnet run -c Release`. Versioning default is the VersionPrefix group in the library csproj. Tests and benchmark are not packable. Optional Nerdbank is **`--NerdbankGitVersioning`** `Repo` or `Project`.

**Test project layout.** Mini-scopes in this order: general TFMs, language/debug (from `CSharpProjectOptions`), packaging, test configuration (`TestTfmsInParallel` + MSTest logger), Coverlet `#if` block, ReportGenerator `#if` block, NugetReport + `ListVulnerable`, `.gitignore` hide, `ProjectReference`, then **External dependencies** `PackageReference`s last. Coverlet is `coverlet.msbuild` + `CollectCoverage=true`; that **does** run on `dotnet test` (VSTest path, SDK 10), including Linux/WSL (`dotnet` ships MSBuild). Report/logger/NugetReport paths use `$([MSBuild]::NormalizeDirectory(...))` so Linux does not create a folder named `ReportGeneratorOutput\net10.0`.

**Why `PublishDefaultFramework` exists.** When `IsPublishable` is `true`, generated libraries are used as `dotnet pack` and `dotnet publish` with no `-f` and no extra properties. Pack must include every selected TFM; publish must write one default TFM to `bin/Publish`. The SDK does the pack side from `TargetFrameworks` alone. It does **not** do the publish side: multi-targeting `Publish` is NETSDK1129 unless the caller passes a framework. The dispatch is that default (highest selected TFM, `__TargetFramework__`). Restore, build, test, pack, and `ProjectReference` stay on the stock SDK.

Do not put `TargetFramework` next to `TargetFrameworks` to avoid `-f`. That was the previous library: MSBuild saw a single TFM, pack needed `BuildForPack`, and a `net8.0` consumer could not reference the project. `_IsPublishing` on `TargetFramework` still fails when that consumer publishes (the flag is global).

Implementation (do not “simplify” into one always-imported file or back to `<Project Sdk="...">`): `ImportSdkTargets.targets` always closes `Sdk.targets`; `PublishDefaultFramework.targets` loads only when `IsCrossTargetingBuild` is true. **`ImportSdkTargets` must be the last import in the library csproj.** That file *is* `Sdk.targets` plus the outer publish dispatch. The SDK reads properties and items while it loads (`EnforceCodeStyleInBuild`, `GlobalAnalyzerConfigFiles`, publish). Anything after that line is after the SDK and is ignored for those. `Project Sdk="..."` would append `Sdk.targets` after this file and overwrite the Publish override. `SourceControlState.targets` is a `BeforeTargets` hook on `GenerateAssemblyInfo` (SDK 8+ Source Link); it can sit just above the SDK close. `InitializePublicApi.targets` is imported only when `PublicApiAnalyzers` is on, also just above the SDK close. `PublishRelease` keeps a direct project `dotnet publish` on Release. Tests may keep `SetTargetFramework`; external consumers must not need it. `<!--#if` in `.targets` is generate-time (`**/*.targets` in `specialCustomOperations`).

All three project files remove `.gitignore` from their `None` items so it stays on disk without appearing as a project item. The test ignore also covers generated `NugetReport/` output.

With `PlaceSolutionInSolutionFolder=true`, each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/`. This creates no shared files on later library additions. The root-solution variant leaves repository-root ignore policy to the repository owner; it does not create or overwrite a shared root `.gitignore`. The same applies to the temporary root `.vs/` left by Visual Studio's extra solution.

## Other symbols worth not breaking

- **`ProjectLicense` / `RepoLicense` / `InitAllRepoItems`**: see section above. Default MIT. SPDX expression for standards; `PackageLicenseFile` only for `Custom`. Root `LICENSE` is the `RepoLicense` item or the CLI all-set.
- **`NerdbankGitVersioning` / `WriteRepoVersionJson`**: see section above. Default `Off`. Root `version.json` is first-create only.
- **`CSharpProjectOptions`**: see section above. Do not split back into per-property dropdowns.
- **`ProjectEditorGlobalConfig`**: see section above. Keep the file and the csproj wire-up on the library only, before `ImportSdkTargets`.
- **`PublicApiAnalyzers`**: see section above. Default `false`. Library only. Baseline files are first-build, not generate-time.
- **`DocumentationTemplate` / `WritePackageDocTemplate` / `WriteRepoDocTemplate`**: see section above. Default empty/`None`. Seed only; not the vendored site.
- **`PlaceSolutionInSolutionFolder`**: default true → `src/sln/ClassLibrary/ClassLibrary.slnx` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). False on CLI renames to a root `.slnx`; false in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS’s conventional root `.slnx`. False also stacks every library’s `.slnx` in one directory.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`PackageAuthor`**: required.
- Conditionals in `.md` / `.slnx` / `.targets` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.targets` is how `UseWebSdk` selects `Sdk.targets` in `ImportSdkTargets.targets`.
