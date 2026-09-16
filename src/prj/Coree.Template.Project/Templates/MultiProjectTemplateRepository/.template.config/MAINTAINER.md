# `.template.config` (MultiLibraryRepository)

Product surface: **`.NET multi-library repository`**. `identity` is `CoreeTemplatesProjectMultiLibraryRepository`; CLI short name is `multilibraryrepo-coree`.

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. Host mapping: **CLI ↔ Visual Studio**. No `icon` property: see **Visual Studio template icon**. |
| `dotnetcli.host.json` | CLI long names; empty `shortName` for `InitRepoItems`, `InitAllRepoItems`, `Author`, `CSharpProjectOptions`, `ProjectLicense`, `NerdbankGitVersioning`, `DocumentationTemplate`, `NuGetAuditHighCriticalAsErrors`, `DirectoryMsBuildFiles`, and `DotNetToolManifest` so they do not steal single-letter aliases. |
| `icon.png` | **Intentionally absent.** Visual Studio then uses the template **package** icon. |
| `MAINTAINER.md` | This file. |

## Visual Studio template icon

Create a new project shows one icon per template. Two files can supply it; they are not the same surface.

| Source | Path | What uses it |
| --- | --- | --- |
| Template package | `src/prj/Coree.Template.Project/NugetAssets/Icon.png` (`PackageIcon` on `Coree.Template.Project.csproj`) | NuGet listing **and** the VS picker when this template does not declare its own icon. |
| This template | `.template.config/icon.png`, optional `ide.host.json` `"icon": "icon.png"` | VS picker for **this** template only. Overrides the package icon. |
| Generated library | `src/prj/{Name}/Properties/NugetAssets/Icon-128x128.png` | The **consumer** nupkg after `dotnet pack`. Not the template picker. |

Verified in Visual Studio (Create a new project, Recent project templates): a template with `.template.config/icon.png` showed that image; sibling Coree templates without one showed the package icon. Leave this template’s picker icon **undefined** so the package icon is used. Ship per-template picker icons later; do not copy `Icon-128x128.png` here as a stand-in.

## Intended usage

The template bootstraps a **repository layout** for one or more packable class libraries (1:n split of a too-large library). It does not `git init`. Same `--output` = combo repo; different `--output` = separate repos.

`Author` is required on every create. Everyday CLI is author, name, output; root files only on the first create into an empty folder.

Install from this folder (or from the packed `Coree.Template.Project` nupkg):

```powershell
dotnet new install "C:\dev\github.com\carsten-riedel\Coree.Template.Project\src\prj\Coree.Template.Project\Templates\MultiLibraryRepository" --force
```

Folder install is the local loop. Verify by generating into `%TEMP%`. Do not `dotnet build` `src/prj/__SourceName__/__SourceName__.csproj` in this tree: it is template source (every `<!--#if` branch still present). A C# design-time build of that stub is enough to validate the project shape.

Combo repo, three libraries, root files only once:

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.ClassLibrary1" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin" --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.ClassLibrary2" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.ClassLibrary3" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
```

### CLI use cases

`$out` is the same folder for a combo repo. Different `--output` = separate repos. `--NerdbankGitVersioning` default is **`Project`** (`Properties/version.json`, extra package). Combo-safe: later libraries do not collide. `--NerdbankGitVersioning Off` keeps VersionPrefix. `--NerdbankGitVersioning Repo` is the shared root file.

**Default, one library**

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.ClassLibrary1" --output $out --InitAllRepoItems
```

Root: `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, and `LICENSE`. Library: Nerdbank **Project**, `Properties/version.json`.

**Default, multi-library (combo)**

```powershell
# combo gut
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, and TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

Call 1 writes the five root files. Call 2 only adds `src/prj` / `src/sln`. Both libraries get `Properties/version.json`.

**Nerdbank repository, multi-library**

```powershell
# combo gut — Call 2 only wires the library; generate does not stamp version.json again
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json)
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

**Nerdbank project folder, multi-library** (this is the omit-the-switch default)

```powershell
# combo gut — same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out
```

**Two separate repos (not a combo)**

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.LibA" --output $outA --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.LibB" --output $outB --InitAllRepoItems
```

Each `--output` is its own first create.

After the first call in the default combo the repo root has `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, and `LICENSE`. Each library gets `Properties/version.json`. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitAllRepoItems` or `--InitRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite.

`--InitAllRepoItems` is the CLI first-create set (same five files as Visual Studio). It does **not** add `version.json`. `--InitRepoItems` picks individual files. Values are separated by **spaces**. Repeating `--InitRepoItems` per value also works. A quoted pipe-separated string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "Organization.Domain.ClassLibrary1" --output "<repo>" --InitRepoItems AIReleaseCheckpoint
```

**Visual Studio:** first create uses the `ide.host.json` default (same five root files as `--InitAllRepoItems`). A second library in the IDE cannot omit the group: choose **None**. Later libraries in the same folder are otherwise the CLI path above. Why the two hosts differ is in **CLI ↔ Visual Studio** below.

## CLI ↔ Visual Studio

The generated first-create product is meant to match. The **switches** cannot be identical, because the hosts do not have the same empty-set, default, or repeat-create rules. Do not “fix” this by making `template.json` `defaultValue` equal the Visual Studio default.

### Why CLI defaults stay empty

A combo repository is two or more `dotnet new` calls into the **same** `--output`. The template engine has **one** CLI default for every call. If `InitRepoItems` defaulted to the five root files, the second library would hit Exit 73 (collision) unless the caller passed `None` or `--force`. Visual Studio’s New Project dialog is a **first create** into an empty folder; it can check the five boxes by default. CLI later-libraries omit `--InitAllRepoItems` and `--InitRepoItems`. `NerdbankGitVersioning` defaults to **`Project`** on both hosts (per-library `Properties/version.json`; later creates do not collide). `--NerdbankGitVersioning Repo` does not write the root `version.json` by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again without Exit 73.

### CLI → Visual Studio

| CLI | Visual Studio equivalent | Why |
| --- | --- | --- |
| `--InitAllRepoItems` | Leave **Repository root items** at the ide.host default (all five files checked) | Bool flag with no value list. The engine cannot treat a bare `--InitRepoItems` as “all”; that is Exit 127. The set is the VS first-create default, not another checkbox in that group. `version.json` is not in this set. |
| `--InitRepoItems Readme …` (spaces) | Uncheck the files you do not want | Individual files. `|` is only legal in `ide.host.json` `defaultValue`, not on current `dotnet new`. |
| omit both switches | **None** | CLI may leave a multi-choice empty. Visual Studio may not. |
| `--InitRepoItems None` | **None** | Explicit empty set. Also wins over `--InitAllRepoItems` if both are passed. Everyday CLI later-libraries omit the switches instead. |
| `--InitAllRepoItems` hidden from the wizard | `ide.host.json` `isVisible: false` | A choice `All` inside the same VS group would sit next to `None` and the five files; you cannot hide one choice per host. The bool is CLI convenience only. |

### Visual Studio → CLI

| Visual Studio | CLI equivalent | Why |
| --- | --- | --- |
| First create, root items left at default | `--InitAllRepoItems` | Same five files. Do not translate the ide.host pipe-separated string onto the CLI. |
| Uncheck some root items | `--InitRepoItems` plus the remaining choice names | Subset. |
| Second library: **None** | omit `--InitAllRepoItems` and `--InitRepoItems` | The IDE requires at least one value; leftover checks from persistence would otherwise stamp root files again. `persistenceScope: none` still needs **None** as the empty-set control. CLI empty default is that None. |
| `InitAllRepoItems` not shown | do not look for it in Additional information | CLI-only. |

`persistenceScope: none` on the VS symbols that have custom defaults: the dialog must not reuse the last create (especially **None** or a subset) as the next “first create”.

Other host-only switch behavior (not root files, same class of reason):

- **`PlaceSolution` `RepoRoot`:** CLI renames to `{Name}.slnx` at repo root. Visual Studio keeps `{Name}.generated.slnx` so it does not overwrite the `{Name}.slnx` the IDE always writes. `SlnFolder` and `BesideCsproj` rename on both hosts (paths are not the VS root file). Post-actions that open the handbook Readme and tell you to close/reopen are `HostIdentifier == "vs"` only. `primaryOutputs` index 0 is the surviving handbook path (`src/sln/{Name}/Readme.md` or `src/prj/{Name}/Readme.md` when `BesideCsproj`).
- **`CSharpProjectOptions` / `ProjectLicense` / `NerdbankGitVersioning` / `DocumentationTemplate` / `NuGetAuditHighCriticalAsErrors` / `DirectoryMsBuildFiles` / `DotNetToolManifest`:** same defaults on both hosts (`NerdbankGitVersioning` `Project`, `DocumentationTemplate` empty/`None`, `NuGetAuditHighCriticalAsErrors` `true`, `DirectoryMsBuildFiles` `false`, `DotNetToolManifest` `true`). `--NerdbankGitVersioning Repo` does not write the root file by itself (`WriteRepoVersionJson` does), so a later library can pass `--NerdbankGitVersioning Repo` again. `--DocumentationTemplate Package` is safe on later libraries; `--DocumentationTemplate Repository` on a later library is Exit 73.
- **`Author`:** required on both. CLI `--Author`.

## `InitRepoItems` / `InitAllRepoItems`

`InitRepoItems` is the multi-choice (`allowMultipleValues`), not N bools. Visual Studio shows **one group** of checkboxes. `InitAllRepoItems` is a CLI bool for that same first-create set; `ide.host.json` hides it.

| Host | Default | Empty set |
| --- | --- | --- |
| CLI `InitRepoItems` | none selected (`defaultValue` `""`) | omit the switch |
| CLI `InitAllRepoItems` | `false` | omit the switch |
| Visual Studio | `Readme\|AIReleaseCheckpoint\|GitAttributes\|GitIgnore\|RepoLicense` | not allowed; choose `None` |

`None` is first in the choice list. `sources` exclude each root file unless `InitAllRepoItems` is on or that choice is selected; `None` excludes all of them, including when `InitAllRepoItems` is on. `==` in conditions means the value is among the selected choices. `GitIgnore` is the same shape as `GitAttributes`: seed is the output name at the template root (`.gitignore`), same exclude condition, no rename. Nested `src/prj` / `src/sln` `.gitignore` files use those full paths; they are not this switch and stay on every create.

## AI-supported release checkpoint

The generated product still contains placeholders that can only become true **after implementation** — especially empty `src/prj/*/Properties/NugetAssets/Readme.md`. A human or an LLM can fill those from the code. That is a **gate before the first publish**, not a generate-time script and not standing agent rules.

**Not:** run-once / post-bootstrap right after `dotnet new`.
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

Language/debug values are **always written** (no omitted PropertyGroup) into the library csproj. `GenerateDocumentationFile` is in this same choice list and writes the library's XML documentation setting.

No `None`. CLI and VS default is the five product values. Visual Studio cannot leave a multi-choice empty; at least one box stays checked. CLI: omit the switch, or pass values with **spaces** (`--CSharpProjectOptions Nullable LangLatest DebugEmbedded`). `|` is only the host default separator.


## `ProjectLicense`

Single choice (dropdown, not a checkbox group). Project + NuGet only. Repository-root `LICENSE` is `InitRepoItems` choice `RepoLicense` (UI: **LICENSE file at repository root**) or `--InitAllRepoItems`, not a second VS bool.

| Choice | `Properties/NugetAssets/License.txt` | NuGet |
| --- | --- | --- |
| `MIT` (CLI/VS default) | MIT text on disk, not packed | `PackageLicenseExpression` `MIT` |
| `BSD3Clause` | BSD 3-Clause text on disk, not packed | `PackageLicenseExpression` `BSD-3-Clause` |
| `Apache2` | Apache 2.0 text on disk, not packed | `PackageLicenseExpression` `Apache-2.0` |
| `Custom` | copyright notice only; packed | `PackageLicenseFile` `License.txt` |

Do not set `PackageLicenseFile` together with an expression (NU5033). The glob excludes `License.txt` except for `Custom`.

Seeds live under `TemplateAssets/Licenses/` (`MIT.txt`, `BSD3Clause.txt`, `Apache2.txt`, `Custom.txt`). Extra sources copy the chosen seed to `src/prj/{Name}/Properties/NugetAssets/License.txt` on every create, and to repository-root `LICENSE` only when `WriteRepoLicense` is true. Do not leave a mega-file under `src/prj/__SourceName__/Properties/NugetAssets/`. DocShell extra sources must `exclude` `Licenses/**` and `Versioning/**`. Each seed may use a shallow `//#if (PackageCopyrightHolderIsSet)` / `//#else` / `//#endif`. Do not put `ProjectLicense` `#if` in the seed: extra sources pick the file.

`RepoLicense` is off on CLI unless listed in `--InitRepoItems` or `--InitAllRepoItems` is on. Visual Studio includes it in the first-create default. `WriteRepoLicense` is `(InitRepoItems != None) && (InitAllRepoItems || InitRepoItems == RepoLicense)`. First create only; a later library with `RepoLicense` or `InitAllRepoItems` selected collides (Exit 73), same as root README. `None` excludes it even if leftover checks remain.

## `NerdbankGitVersioning`

Single choice (dropdown), same shape as `ProjectLicense`. CLI long name is **`--NerdbankGitVersioning`** so the extra NuGet dependency is visible. Default **`Project`**: `version.json` under this library's `Properties/` folder. `Repo` uses one root `version.json`. `Off` is the VersionPrefix group, no package. `--InitAllRepoItems` is the five root files only; it does **not** copy root `version.json`.

```powershell
# combo gut
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
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
      )
```

`UseNerdbankGitVersioning` is generate-time `<!--#if` in `__SourceName__.csproj`. The VersionPrefix group is the `#else` (`Off`). Both branches stay in the **template source**; `dotnet new` keeps one.

`None` wins over InitAll. A second **generate-time** write of root `version.json` is Exit 73. `--NerdbankGitVersioning Repo` without Init does not stamp the file at `dotnet new`. `Properties/Build/NerdbankRepositoryVersion.targets` (imported only for Repo **after generate**) copies `Properties/Build/Nerdbank.version.json` to the repository root **if it does not exist**, before Nerdbank reads it. A later library in the same folder therefore does not collide.

In **template source** those `<!--#if (NerdbankGitVersioning == "Repo") -->` markers are XML comments, so MSBuild always imports the targets. From `src/prj/__SourceName__`, `../../../version.json` is this template folder. The copy no-ops when `../../../.template.config` exists. A `version.json` beside this `MAINTAINER.md` is a failed host stamp — delete it, do not commit.

There is no `version.json` checkbox in `InitRepoItems`. Generate-time root file is `WriteRepoVersionJson` (`Repo` plus first-create Init). If that file is still missing, the Repo library writes it once at build (`if not exists`). Later VS library: **None** plus **This repository (root version.json)**.

Seeds live under `TemplateAssets/Versioning/`. `Project/version.json` uses `pathFilters` `[".."]` (height is the packable project folder next to `Properties/`). `Repo/version.json` uses `pathFilters` `["."]` (height is the whole repository). Extra sources copy `Versioning/Project/` to `Properties/` and `Versioning/Repo/` to the repository root. `Properties/Build/Nerdbank.version.json` is the same payload as `Versioning/Repo/version.json` (late first-build copy). `Project` is first in the choice list because it is the default.

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
| `Repo` | false | `Readme` / checkpoint / `.gitattributes` / `.gitignore` / `RepoLicense` (not `None`) | write |
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
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, and TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Project` | skip | no |
| 2 | false | `Project` | skip | no |

**Nerdbank repository, multi-library** (no collision on call 2)

```powershell
# combo gut
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) — Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Repo` | write | no |
| 2 | false | `Repo` | skip | no |

**Nerdbank project folder, multi-library** (this is the omit-the-switch default)

```powershell
# combo gut — same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out
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

## `NuGetAuditHighCriticalAsErrors`

Library-only bool, default **true**. UI label **Treat high/critical NuGet vulnerabilities as errors**. CLI long name **`--NuGetAuditHighCriticalAsErrors`**. Omit the switch → on. `--NuGetAuditHighCriticalAsErrors false` writes nothing. Combo-safe.

SDK restore already runs NuGetAudit (NU1901–NU1904 warnings). This switch only appends `<WarningsAsErrors>$(WarningsAsErrors);NU1903;NU1904</WarningsAsErrors>` on the packable library. Low (`NU1901`) and moderate (`NU1902`) stay warnings. Do not set `NuGetAudit` / `NuGetAuditMode` / `TreatWarningsAsErrors` here (`NuGetAudit` is already on).

Turn the switch off only for a dependency graph that cannot be cleaned without accepting high or critical advisories.

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --NuGetAuditHighCriticalAsErrors false
```

## `DocumentationTemplate`

Multi-choice, default **empty** (CLI) / **None** (Visual Studio). UI label **Documentation template**. CLI long name **`--DocumentationTemplate`**. Not part of `--InitAllRepoItems`. Same `TemplateAssets/DocShell.html` seed, two destinations. Extra sources copy that file only (`exclude` of `Versioning/**`, `Licenses/**`, and `DirectoryMsBuild/**`). Do **not** vendor the 25-file offline site in the template: the HTML file is the bootstrap contract, so a later checkpoint run acquires the versions that file pins then, not whatever was frozen in this pack. A newer DocShell release is a copy/replace of `TemplateAssets/DocShell.html` (keep that filename). Do not rewrite internal bootstrap paths such as `./documentation/css` here; those change in the DocShell product file itself.

| Choice | Path | When | Combo later library |
| --- | --- | --- | --- |
| `Package` | `src/prj/{Name}/Properties/NugetAssets/docs/DocShell.html` | every create | pass `Package` again |
| `Repository` | `docs/DocShell.html` | first create | omit `Repository` (Exit 73 if stamped again) |
| `None` | nothing | — | wins over the other choices |

```powershell
# combo gut
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Package Repository
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --DocumentationTemplate Package

# combo error (Exit 73) — Call 2 stamps docs/DocShell.html again
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DocumentationTemplate Repository
# dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --DocumentationTemplate Repository
```

`Properties/NugetAssets/docs` is packed with the nupkg (`PackagePath` empty, so `docs/` inside the package, not `Properties/`). Repo-root `docs/` is not packed. The checkpoint infers package vs repository documentation from `Properties/NugetAssets/docs` vs repo-root `docs/`; it does not name this switch.

## Project roles and Git ignores

The library is packable and `IsPublishable` is `false` (NuGet pack is the distribution path). `Properties/Build/` contains optional MSBuild targets. `Properties/NugetAssets/` is nupkg assets (readme, icon, notes, optional `docs/`). Both folders are on disk under `Properties/` so Explorer and Solution Explorer match; they are not source. Do not keep them at the project root and `Link` them. `.config/dotnet-tools.json` and `Directory.Build.*` stay next to the csproj. Versioning default is Nerdbank **Project** (`Properties/version.json`). **`--NerdbankGitVersioning`** `Off` is VersionPrefix; `Repo` is the shared root file.

The template does not add a custom publish dispatch. `dotnet pack` is the distribution path for this project-template package.

The SDK-style project supplies the standard SDK imports. `SourceControlState.targets` is a `BeforeTargets` hook on `GenerateAssemblyInfo` (SDK 8+ Source Link); `NerdbankRepositoryVersion.targets` is imported only for repository versioning. Both remain ordinary project imports. There are no generate-time conditionals in `.targets` files.

The packable library always sets `EnablePackageValidation` (no wizard). That is TFM/runtime consistency on `dotnet pack`, not a baseline against nuget.org. Do not stamp `PackageValidationBaselineVersion` at generate; after the first publish the consumer sets it to that version.

With `PlaceSolution` `SlnFolder` (default), each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/` next to that library's `.slnx` and handbook `Readme.md`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes (no nested `.gitignore`); delete that folder if you do not need it. `BesideCsproj` writes the `.slnx` and handbook next to the packable csproj and does **not** create `src/sln/{Name}/` (`src/prj/{Name}/.gitignore` already ignores `.vs/`). Repository-root `.gitignore` is `InitRepoItems` `GitIgnore` / `--InitAllRepoItems` (first create only). Later libraries omit Init and do not overwrite it. If root ignore is off, the `RepoRoot` variant and Visual Studio's extra root `.vs/` stay the owner's problem.

## `DirectoryMsBuildFiles`

Library + this library's `.slnx` only. Bool, default **false**. UI label **Empty Directory.Build and Directory.Solution files**. CLI long name **`--DirectoryMsBuildFiles`**. Omit the switch → nothing. Combo-safe on `SlnFolder` and `BesideCsproj` (paths include the library name). `RepoRoot` stacks `Directory.Solution.*` at the repository root like stacking `.slnx` files. Not an Init* item and not `Directory.Packages.props`.

Seeds live under `TemplateAssets/DirectoryMsBuild/`. Extra sources copy `Directory.Build.props` / `.targets` next to the library csproj, and `Directory.Solution.props` / `.targets` next to this library's `.slnx` (`src/sln/{Name}/` for `SlnFolder`, `src/prj/{Name}/` for `BesideCsproj`, repo root for `RepoRoot` — later library then collides, same as stacking `.slnx` files). DocShell extra sources must `exclude` `Versioning/**` and `DirectoryMsBuild/**`.

The files are almost empty `<Project>` stubs with comments. MSBuild auto-imports them from those directories. Do not move analyzer configuration or pack validation into them. The template does not `#if` properties into these files vs the csproj (possible, ugly). The library csproj `None Include`s the two `Directory.Build.*` files with `Link` under `Properties\` (Solution Explorer only). Do **not** move the files into `Properties/` on disk: auto-import follows the directory of the file. `Directory.Solution.*` stay beside the `.slnx`. When `PlaceSolution` is `BesideCsproj`, the library csproj also `Link`s those two solution files (same folder as the csproj) so they do not look like stray project items.

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems --DirectoryMsBuildFiles
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --DirectoryMsBuildFiles
```

## `DotNetToolManifest`

Library project only. Bool, default **true**. UI label **Empty local dotnet-tools.json**. CLI long name **`--DotNetToolManifest`**. Omit the switch → empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it. Combo-safe: path includes the library name. Not Init. `tools` is `{}`; no `dotnet tool restore` on build. `isRoot` is true so a later parent manifest does not merge in. `dotnet tool install --local` from the library folder fills the file. With `SlnFolder`, the handbook CWD (`src/sln/{Name}/`) does not see this manifest. With `BesideCsproj`, the handbook CWD is the library folder and does.

The library csproj `None Include`s the file with `Link` under `Properties\` (Solution Explorer only). Disk path stays `.config/`.

```powershell
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library1" --output $out --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "abcd" --name "...Library2" --output $out --DotNetToolManifest false
```

## Other symbols worth not breaking

- **`ProjectLicense` / `RepoLicense` / `InitAllRepoItems` / `GitIgnore`**: see section above. Default MIT. SPDX expression for standards; `PackageLicenseFile` only for `Custom`. Root `LICENSE` is the `RepoLicense` item or the CLI all-set. Root `.gitignore` is `GitIgnore` in that same first-create set.
- **`NerdbankGitVersioning` / `WriteRepoVersionJson`**: see section above. Default `Project`. Root `version.json` is first-create only (`Repo` plus Init).
- **`CSharpProjectOptions`**: see section above. Do not split back into per-property dropdowns.
- **`NuGetAuditHighCriticalAsErrors`**: see section above. Default `true`. Library only. NU1903/NU1904 as restore errors. Off for EOL/backport graphs.
- **`DocumentationTemplate` / `WritePackageDocTemplate` / `WriteRepoDocTemplate`**: see section above. Default empty/`None`. Seed only; not the vendored site.
- **`PlaceSolution`**: single choice, default `SlnFolder` → `src/sln/__SourceName__/__SourceName__.slnx` plus handbook `Readme.md` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). `RepoRoot` on CLI renames to a root `.slnx`; `RepoRoot` in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS’s conventional root `.slnx`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes and stacks every library’s `.slnx` in one directory. `BesideCsproj` writes `src/prj/{Name}/{Name}.slnx` and the handbook next to the packable csproj and does not create `src/sln/{Name}/`. The `.slnx` virtual folder `/sln/{Name}/` is omitted for `BesideCsproj`; `Readme.md` is a solution item beside the file.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`Author`**: required. CLI `--Author`.
- **`<Description>`**: not a template parameter. Generate leaves an empty CDATA block for multiline gallery text; the checkpoint fills it (assistant-supported).
- **`EnablePackageValidation`**: not a symbol. Always `true` on the packable library. No `PackageValidationBaselineVersion` at generate.
- **`DirectoryMsBuildFiles`**: see section above. Default `false`. Empty `Directory.Build.*` beside the library and `Directory.Solution.*` beside this `.slnx`. No `Directory.Packages.props`.
- **`DotNetToolManifest`**: see section above. Default `true`. Empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it.
- Conditionals in `.md` / `.slnx` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.txt` and the renamed root `LICENSE` use `//#if`. License seeds under `TemplateAssets/Licenses/` may use a shallow copyright `//#if` / `//#else`; extra sources pick the file so there is no `ProjectLicense` `#if` in the text.
