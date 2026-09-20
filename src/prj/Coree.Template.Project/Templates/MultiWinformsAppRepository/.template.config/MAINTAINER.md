# `.template.config` (MultiWinformsAppRepository)

Product surface: **`.NET multi-winforms repository`**. `identity` is `CoreeTemplatesProjectMultiWinformsAppRepository`; CLI short name is `multiwinformsrepo-coree`. No `ChoosingPackageBoundaries.md`.

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. Host mapping: **CLI ↔ Visual Studio**. No `icon` property: see **Visual Studio template icon**. |
| `dotnetcli.host.json` | CLI long names; empty `shortName` for `InitRepoItems`, `InitAllRepoItems`, `Author`, `CSharpProjectOptions`, `ProgramSample`, `AdditionalInstaller`, `ProjectLicense`, `NerdbankGitVersioning`, `DocumentationTemplate`, `ProjectEditorGlobalConfig`, `AnalysisMode`, `TestCoverage`, `DirectoryMsBuildFiles`, `DotNetToolManifest`, and `Publish` so they do not steal single-letter aliases. |
| `icon.png` | **Intentionally absent.** Visual Studio then uses the template **package** icon. |
| `MAINTAINER.md` | This file. |

## Visual Studio template icon

Create a new project shows one icon per template. Two files can supply it; they are not the same surface.

| Source | Path | What uses it |
| --- | --- | --- |
| Template package | `src/prj/Coree.Template.Project/Properties/NugetAssets/Icon.png` (`PackageIcon` on `Coree.Template.Project.csproj`) | NuGet listing **and** the VS picker when this template does not declare its own icon. |
| This template | `.template.config/icon.png`, optional `ide.host.json` `"icon": "icon.png"` | VS picker for **this** template only. Overrides the package icon. |

Verified in Visual Studio (Create a new project, Recent project templates): a template with `.template.config/icon.png` showed that image; sibling Coree templates without one showed the package icon. Leave this template's picker icon **undefined** so the package icon is used. Ship per-template picker icons later; do not copy `Icon-128x128.png` here as a stand-in.

## Intended usage

The template bootstraps a **repository layout** for one or more publishable WinForms apps (1:n split of a too-large app). It does not `git init`. Same `--output` = combo repo; different `--output` = separate repos.

`Author` is required on every create. Everyday CLI is author, name, output; root files only on the first create into an empty folder.

Install from this folder (or from the packed `Coree.Template.Project` nupkg):

```powershell
dotnet new install "C:\dev\github.com\carsten-riedel\Coree.Template.Project\src\prj\Coree.Template.Project\Templates\MultiConsoleAppRepository" --force
```

Folder install is the local loop. Verify by generating into `%TEMP%`. Do not `dotnet build` `src/prj/__SourceName__/__SourceName__.csproj` in this tree: it is template source (every `<!--#if` branch still present). A C# design-time build of that stub is enough to run `GenerateAssemblyInfo`.

Combo repo, three apps, root files only once:

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.WinFormsApp1" --output "C:\Users\Valgrind\source\repos\MultiConsoleAppRepository-multisolution-optin" --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.ConsoleApp2" --output "C:\Users\Valgrind\source\repos\MultiConsoleAppRepository-multisolution-optin"
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.ConsoleApp3" --output "C:\Users\Valgrind\source\repos\MultiConsoleAppRepository-multisolution-optin"
```

### CLI use cases

`$out` is the same folder for a combo repo. Different `--output` = separate repos. `--NerdbankGitVersioning` default is **`Project`** (`Properties/version.json`, extra package). Combo-safe: later apps do not collide. `--NerdbankGitVersioning Off` keeps VersionPrefix. `--NerdbankGitVersioning Repo` is the shared root file.

**Default, one app**

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.WinFormsApp1" --output $out --InitAllRepoItems
```

Root: `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, `LICENSE`. App: Nerdbank **Project**, `Properties/version.json`.

**Default, multi-app (combo)**

```powershell
# combo gut
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --InitAllRepoItems
```

Call 1 writes the five root files. Call 2 only adds `src/prj` / `src/sln`. Both apps get `Properties/version.json`.

**Nerdbank repository, multi-app**

```powershell
# combo gut - Call 2 only wires the app; generate does not stamp version.json again
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json)
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

**Nerdbank project folder, multi-app** (this is the omit-the-switch default)

```powershell
# combo gut - same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out
```

**Two separate repos (not a combo)**

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.AppA" --output $outA --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.AppB" --output $outB --InitAllRepoItems
```

Each `--output` is its own first create.

After the first call in the default combo the repo root has `README.md`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, `.gitattributes`, `.gitignore`, and `LICENSE`. Each app gets `Properties/version.json`. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitAllRepoItems` or `--InitRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite. `--InitRepoItems SrcGlobalJson` is a separate first-create opt-in (`src/global.json`, default off).

`--InitAllRepoItems` is the CLI first-create set (same five files as Visual Studio). It does **not** add `version.json` or `src/global.json`. `--InitRepoItems` picks individual files. Values are separated by **spaces**. Repeating `--InitRepoItems` per value also works. A quoted `Readme|AIReleaseCheckpoint|GitAttributes|GitIgnore|RepoLicense` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "Organization.Domain.WinFormsApp1" --output "<repo>" --InitRepoItems AIReleaseCheckpoint
```

**Visual Studio:** first create uses the `ide.host.json` default (same five root files as `--InitAllRepoItems`; `src/global.json` unchecked). A second app in the IDE cannot omit the group: choose **None**. Later apps in the same folder are otherwise the CLI path above. Why the two hosts differ is in **CLI ↔ Visual Studio** below.

## CLI ↔ Visual Studio

The generated first-create product is meant to match. The **switches** cannot be identical, because the hosts do not have the same empty-set, default, or repeat-create rules. Do not "fix" this by making `template.json` `defaultValue` equal the Visual Studio default.

### Why CLI defaults stay empty

A combo repository is two or more `dotnet new` calls into the **same** `--output`. The template engine has **one** CLI default for every call. If `InitRepoItems` defaulted to the five root files, the second app would hit Exit 73 (collision) unless the caller passed `None` or `--force`. Visual Studio's New Project dialog is a **first create** into an empty folder; it can check the five boxes by default. CLI later-apps omit `--InitAllRepoItems` and `--InitRepoItems`. `NerdbankGitVersioning` defaults to **`Project`** on both hosts (per-app `Properties/version.json`; later creates do not collide). `--NerdbankGitVersioning Repo` does not write the root `version.json` by itself (`WriteRepoVersionJson` does), so a later app can pass `--NerdbankGitVersioning Repo` again without Exit 73.

### CLI → Visual Studio

| CLI | Visual Studio equivalent | Why |
| --- | --- | --- |
| `--InitAllRepoItems` | Leave **Repository root items** at the ide.host default (all five files checked) | Bool flag with no value list. The engine cannot treat a bare `--InitRepoItems` as "all"; that is Exit 127. The set is the VS first-create default, not another checkbox in that group. `version.json` and `src/global.json` are not in this set. |
| `--InitRepoItems Readme …` (spaces) | Uncheck the files you do not want | Individual files. `|` is only legal in `ide.host.json` `defaultValue`, not on current `dotnet new`. |
| omit both switches | **None** | CLI may leave a multi-choice empty. Visual Studio may not. |
| `--InitRepoItems None` | **None** | Explicit empty set. Also wins over `--InitAllRepoItems` if both are passed. Everyday CLI later-apps omit the switches instead. |
| `--InitAllRepoItems` hidden from the wizard | `ide.host.json` `isVisible: false` | A choice `All` inside the same VS group would sit next to `None` and the five files; you cannot hide one choice per host. The bool is CLI convenience only. |

### Visual Studio → CLI

| Visual Studio | CLI equivalent | Why |
| --- | --- | --- |
| First create, root items left at default | `--InitAllRepoItems` | Same five files. Do not translate the ide.host string `Readme\|…\|RepoLicense` onto the CLI. |
| Uncheck some root items | `--InitRepoItems` plus the remaining choice names | Subset. |
| Second app: **None** | omit `--InitAllRepoItems` and `--InitRepoItems` | The IDE requires at least one value; leftover checks from persistence would otherwise stamp root files again. `persistenceScope: none` still needs **None** as the empty-set control. CLI empty default is that None. |
| `InitAllRepoItems` not shown | do not look for it in Additional information | CLI-only. |

`persistenceScope: none` on the VS symbols that have custom defaults: the dialog must not reuse the last create (especially **None** or a subset) as the next "first create".

Other host-only switch behavior (not root files, same class of reason):

- **`PlaceSolution` `RepoRoot`:** CLI renames to `{Name}.slnx` at repo root. Visual Studio keeps `{Name}.generated.slnx` so it does not overwrite the `{Name}.slnx` the IDE always writes. `SlnFolder` and `BesideCsproj` rename on both hosts (paths are not the VS root file). Post-actions that open the handbook Readme and tell you to close/reopen are `HostIdentifier == "vs"` only. `primaryOutputs` index 0 is the surviving handbook path (`src/sln/{Name}/Readme.md` or `src/prj/{Name}/Readme.md` when `BesideCsproj`).
- **`CSharpProjectOptions` / TFMs / `ProjectLicense` / `NerdbankGitVersioning` / `DocumentationTemplate` / `TestCoverage` / `AnalysisMode` / `DirectoryMsBuildFiles` / `DotNetToolManifest`:** same defaults on both hosts (`NerdbankGitVersioning` `Project`, `DocumentationTemplate` empty/`None`, `TestCoverage` `Coverlet`, `AnalysisMode` `Recommended`, `ProjectEditorGlobalConfig` `Strict`, `DirectoryMsBuildFiles` `false`, `DotNetToolManifest` `true`). `--NerdbankGitVersioning Repo` does not write the root file by itself (`WriteRepoVersionJson` does), so a later app can pass `--NerdbankGitVersioning Repo` again. `--DocumentationTemplate Repository` on a later app is Exit 73.
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

`SrcGlobalJson` is `src/global.json`. Opt-in, **default off**, not part of `--InitAllRepoItems` or the Visual Studio first-create checks. Exclude unless this choice is selected (`None` still wins). Seed lives at that output path (not under `TemplateAssets/`). `SdkPinVersion` is a highest-first switch like `TargetFrameworkValue` and replaces `0.0.0-sdk-pin` with `8.0.0` / `9.0.0` / `10.0.0`; `rollForward` is `latestFeature`. Later app with this box selected is Exit 73. Commands started at the repository root do not see this file.

## AI-supported release checkpoint

The generated product still contains placeholders that can only become true **after implementation** - especially empty `<Description>`. A human or an LLM can fill those from the code. That is a **gate before the first publish**, not a generate-time script and not standing agent rules.

**Not:** run-once / post-bootstrap right after `dotnet new`. The app may still be `Program`.  
**Not:** a forever queue in the GitHub `README.md`. That file is the customer landing page.  
**Not:** a template-stamped `AGENTS.md`. That would collide with the consumer's own agent file and would outlive the scaffold.

**Yes:** one repo-root file, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, stamped only when `InitAllRepoItems` is on or `InitRepoItems` includes `AIReleaseCheckpoint` (first create). `TEMPLATE-` marks it as delete-me scaffold; `AI` matches the VS choice. It is a **Template-Checkpoint-Release**: close template residue, then **self-dissolve**. After that, new chats read the apps and the real docs. The file is written for a person; an assistant can fill it from the product. It is not a prompt and not standing agent rules.

Why the repo root, not `src/sln/{Name}/`: the first look at a combo repo is the customer surface; one fat checklist can say "fill every app description in this repository" without a per-app marker. Apps added later are in scope until the file is deleted.

The VS label **AI-supported release checkpoint** names the *job*, not a recurring agent run. The switch does not start a model. Someone later (person or LLM) works that file to 100% observable items, then deletes it. "AI-supported" belongs in the choice display name; it must not read as "edit with AI on every create."

The generated file is the contract (nine numbered, checkable items). Do not put free-form "run this shell" instructions in it (prompt injection). Do not mix standing style rules into it - those must not self-delete.

## `CSharpProjectOptions`

One multi-choice (`allowMultipleValues`), same VS checkbox combobox as TFMs and `InitRepoItems`. Not four/five separate dropdowns.

| Choice | Checked (default) | Unchecked |
| --- | --- | --- |
| `DisableImplicitUsings` | `disable` | `enable` |
| `Nullable` | `enable` | `disable` |
| `LangLatest` | `latest` | `default` (TFM C# version; valid compiler value) |
| `DebugEmbedded` | `embedded` | `none` |

Language/debug values are **always written** (no omitted PropertyGroup) into the WinForms app, tests, and benchmark csproj. There is no XML documentation file switch.

No `None`. CLI and VS default is the four language/debug values. Visual Studio cannot leave a multi-choice empty; at least one box stays checked. CLI: omit the switch, or pass values with **spaces** (`--CSharpProjectOptions Nullable LangLatest DebugEmbedded`). `|` is only the host default separator.

Benchmark previously hardcoded `ImplicitUsings` enable. It now follows the switch. The benchmark runner `Program.cs` has explicit `System` / `System.IO` / `System.Linq` usings so the default (`disable`) still compiles. The `Benchmarks` class is extra-sourced with `ProgramSample`.

## `ProgramSample`

Single choice, default **`MinimalClassic`**. UI label **Program sample**. CLI long name **`--ProgramSample`**. Empty CLI `shortName`. Combo-safe (each app has its own files). `HostedDi` is the maintained Generic Host/DI alternative.

Seeds live under `TemplateAssets/ProgramSamples/{choice}/App|Tests|Benchmark/`. Extra sources copy each folder onto the matching stub project (`App/` → `src/prj/{Name}/`, `Tests/` → tests, `Benchmark/` → benchmark). Later sample files drop into those folders without a per-file rename. `Benchmark/` is copied only when `BenchmarkProject` is on. DocShell extra sources must `exclude` `ProgramSamples/**` as well as `Versioning/**`, `CodeStyle/**`, `Licenses/**`, and `DirectoryMsBuild/**`. Do not leave a `Program.cs` under `src/prj/__SourceName__/`. The stub csproj stays one file (`WinExe`, `UseWindowsForms`); sample-specific package references can be `#if` there.

`MinimalClassic` is the Visual Studio WinForms skeleton: `[STAThread] static void Main()`, `ApplicationConfiguration.Initialize()`, `Application.Run(new Form1())`, plus `Form1` / `Form1.Designer.cs` / `Form1.resx`. Generate-time `#if (CSharpProjectOptions == "DisableImplicitUsings")` adds the usings those names need. When implicit usings are on, those lines are omitted. No `KeepScaffoldUnusedUsings` here.

`HostedDi` is based on the useful application behavior of the former `winforms-coree` and `winformsdi-coree` samples, not on their repository layout. It uses `Host.CreateApplicationBuilder`, constructor injection, debug logging, reloadable `appsettings.json`, profile optimization, and a lifetime-held single-instance mutex. `Main` stays synchronous and `[STAThread]`; host start/stop is synchronously bridged so no continuation can move the WinForms message loop to an MTA thread. Host and mutex are deterministic `using` lifetimes. Do not copy the legacy library project, console output mode, static service locator, async-void shutdown handler, or old top-level feature switches. The legacy templates remain unchanged.

| Choice | App | Tests | Benchmark |
| --- | --- | --- | --- |
| `MinimalClassic` | VS Form1 app (`Program` does not return a code) | Construct `Form1`; do not call `Main` (message loop) | Construct `Form1` and read `Text` |
| `HostedDi` | Generic Host plus injected `MainForm` and reloadable settings | Configuration, registrations, form state, and reload behavior | UI-independent window-title lookup |

## `AdditionalInstaller`

Single choice, default **`None`**. `WixUserInstaller` adds a Windows-only per-user WiX project under `src/prj/{Name}.WixSetup/`. The generated solution publishes the app to `bin/Publish/` and then writes the MSI to the app's `bin/Setup/`; Visual Studio requires the WiX Toolset HeatWave extension. The WiX SDK remains a project-local package reference, so the CLI can build it without the Visual Studio extension. `SharedProject.props` is the shared project-to-extension property source; it currently contains the bare-publish TFM. the WiX project imports it and calls the app's `PublishForInstaller` target, which returns the effective publish directory for WiX harvesting. `InstallerUpgradeCode` is generated per template invocation; never replace it with a shared fixed GUID.

Coverlet still gates 100% on the app assembly. `ExcludeByFile` skips `Program.cs` (untested message loop) and `**/*.g.cs` (WinForms `ApplicationConfiguration.Initialize` source generator). `ExcludeByAttribute` `CompilerGeneratedAttribute` matches the generated `ApplicationConfiguration` class (`[CompilerGenerated]` in the WinForms generator, not `GeneratedCodeAttribute`). Do not exclude a designer file: Coverlet can then drop the whole partial form type. Generated form disposal is marked `[ExcludeFromCodeCoverage]`; HostedDi also excludes only its UI-thread marshaling callback while testing the configuration provider and observable reload result. `MinimalClassic` construction does not need an STA thread. HostedDi form tests use a dedicated STA thread because the sample exercises configuration callbacks. The HostedDi benchmark is deliberately UI-independent.

## `ProjectLicense`

Single choice (dropdown, not a checkbox group). These apps are publish-only (not a NuGet tool). Repository-root `LICENSE` is `InitRepoItems` choice `RepoLicense` (UI: **LICENSE file at repository root**) or `--InitAllRepoItems`, not a second VS bool.

This dropdown only feeds repository-root `LICENSE` (`WriteRepoLicense`). No `Properties/NugetMetadata/License.txt` and no `PackageLicenseExpression` / `PackageLicenseFile`.

| Choice | Root `LICENSE` when `WriteRepoLicense` |
| --- | --- |
| `MIT` (CLI/VS default) | MIT |
| `BSD3Clause` | BSD 3-Clause |
| `Apache2` | Apache 2.0 |
| `Custom` | copyright notice only |

Seeds live under `TemplateAssets/Licenses/` (`MIT.txt`, `BSD3Clause.txt`, `Apache2.txt`, `Custom.txt`). Extra sources copy the chosen seed to repository-root `LICENSE` only when `WriteRepoLicense` is true. DocShell extra sources must `exclude` `Licenses/**` (same as `CodeStyle/**` and `Versioning/**`). Each seed may use a shallow `//#if (PackageCopyrightHolderIsSet)` / `//#else` / `//#endif`. Do not put `ProjectLicense` `#if` in the seed: extra sources pick the file.

`RepoLicense` is off on CLI unless listed in `--InitRepoItems` or `--InitAllRepoItems` is on. Visual Studio includes it in the first-create default. `WriteRepoLicense` is `(InitRepoItems != None) && (InitAllRepoItems || InitRepoItems == RepoLicense)`. First create only; a later app with `RepoLicense` or `InitAllRepoItems` selected collides (Exit 73), same as root README. `None` excludes it even if leftover checks remain.

## `NerdbankGitVersioning`

Single choice (dropdown), same shape as `ProjectLicense`. CLI long name is **`--NerdbankGitVersioning`** so the extra NuGet dependency is visible. Default **`Project`**: `version.json` under this app's `Properties/` folder. `Repo` uses one root `version.json`. `Off` is the VersionPrefix group, no package. `--InitAllRepoItems` is the five root files only; it does **not** copy root `version.json` or `src/global.json`. Tests and benchmark are not in this switch.

```powershell
# combo gut
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

Call 1: five root files plus root `version.json`, app wire is Nerdbank repo. Call 2: `--NerdbankGitVersioning Repo`, **no** Init, **no** second root stamp (Exit 0). Omit `--NerdbankGitVersioning` for **Project** (`Properties/version.json`). `--NerdbankGitVersioning Off` for the VersionPrefix group.

Two jobs, same split as root `LICENSE`:

- **`NerdbankGitVersioning`** wires **this** app csproj on every create.
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

`None` wins over InitAll. A second **generate-time** write of root `version.json` is Exit 73. `--NerdbankGitVersioning Repo` without Init does not stamp the file at `dotnet new`. `Properties/Build/NerdbankRepositoryVersion.targets` (imported only for Repo **after generate**) copies `Properties/Build/Nerdbank.version.json` to the repository root **if it does not exist**, before Nerdbank reads it. A later app in the same folder therefore does not collide.

In **template source** those `<!--#if (NerdbankGitVersioning == "Repo") -->` markers are XML comments, so MSBuild always imports the targets. From `src/prj/__SourceName__`, `../../../version.json` is this template folder. The copy no-ops when `../../../.template.config` exists. A `version.json` beside this `MAINTAINER.md` is a failed host stamp - delete it, do not commit.

There is no `version.json` checkbox in `InitRepoItems`. Generate-time root file is `WriteRepoVersionJson` (`Repo` plus first-create Init). If that file is still missing, the Repo app writes it once at build (`if not exists`). Later VS app: **None** plus **This repository (root version.json)**.

Seeds live under `TemplateAssets/Versioning/`. `Project/version.json` uses `pathFilters` `[".."]` (height is the packable project folder next to `Properties/`; tests and benchmark are siblings and do not bump). `Repo/version.json` uses `pathFilters` `["."]` (height is the whole repository). Extra sources copy `Versioning/Project/` to `Properties/` and `Versioning/Repo/` to the repository root. `Properties/Build/Nerdbank.version.json` is the same payload as `Versioning/Repo/version.json` (late first-build copy). `Project` is first in the choice list because it is the default.

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

### `NerdbankGitVersioning` → app tree (Init ignored except root json above)

| `NerdbankGitVersioning` | UI | VersionPrefix group | `GitVersionBaseDirectory` | `Properties/version.json` |
| --- | --- | --- | --- | --- |
| `Off` | **Off (VersionPrefix in the app project)** | yes | no | skip |
| `Project` (default, first) | **This app (Properties/version.json)** | no | `Properties` | write |
| `Repo` | **This repository (root version.json)** | no | no | skip |

### Everyday CLI

**Default, multi-app**

```powershell
# combo gut
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md again)
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --InitAllRepoItems
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Project` | skip | no |
| 2 | false | `Project` | skip | no |

**Nerdbank repository, multi-app** (no collision on call 2)

```powershell
# combo gut
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --NerdbankGitVersioning Repo

# combo error (Exit 73) - Call 2 also has --InitAllRepoItems (tries to write README.md, LICENSE, .gitattributes, .gitignore, TEMPLATE-AI-RELEASE-CHECKPOINT.md, and version.json again)
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --InitAllRepoItems --NerdbankGitVersioning Repo
```

| Call | InitAll | `NerdbankGitVersioning` | Root `version.json` | VersionPrefix |
| --- | --- | --- | --- | --- |
| 1 | true | `Repo` | write | no |
| 2 | false | `Repo` | skip | no |

**Nerdbank project folder, multi-app** (this is the omit-the-switch default)

```powershell
# combo gut - same as the default combo; `--NerdbankGitVersioning Project` is optional
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out
```

### Combos that look successful but are incomplete or mixed

`dotnet new` Exit 0 only means no file collision. It does not mean every app in `$out` uses the same versioning.

| Calls | Exit | What is wrong |
| --- | --- | --- |
| `--NerdbankGitVersioning Repo` on an empty folder, no Init | 0 | `dotnet new` does not stamp root `version.json`. First build/pack writes it if missing. |
| `--InitAllRepoItems --NerdbankGitVersioning Repo --InitRepoItems None` | 0 | `None` wins; five root files skipped and no root `version.json`. |
| Call 1 InitAll `--NerdbankGitVersioning Off`, call 2 `--NerdbankGitVersioning Repo` | 0 | App 1 VersionPrefix, app 2 Nerdbank walking up with no root json. |
| Call 1 InitAll `--NerdbankGitVersioning Repo`, call 2 omit the switch | 0 | App 1 Nerdbank + root json, app 2 **Project** (`Properties/version.json`). |
| Call 1 InitAll (default Project), call 2 `--NerdbankGitVersioning Repo` (or the reverse) | 0 | Mixed `Properties/version.json` and repo-style package with no matching root file. |

Keep the same `--NerdbankGitVersioning` value on every app in one `--output`. Root `version.json` only on the first create together with Init.

## `ProjectEditorGlobalConfig`

App-only single choice, default **`Strict`**. UI label **Code style rules for WinForms app project** (app project only; `is_global` is the analyzer-config technical term). CLI long name **`--ProjectEditorGlobalConfig`**. Stamps `src/prj/{Name}/.project.editor.globalconfig` and wires `GlobalAnalyzerConfigFiles`, `EnforceCodeStyleInBuild`, and `OptimizeImplicitlyTriggeredBuild=false`. `Off` writes nothing. Tests and benchmark do not get the file. Keep the symbol and generated disk name. Do not use a bare "project" label: **C# project options** already applies to app, tests, and benchmark.

Roslyn reads `GlobalAnalyzerConfigFiles` (`Visible="false"`). Visual Studio Solution Explorer uses a separate `None` item with `Link` under `Properties\` so the file is clickable next to `version.json`. `None Remove` first, or the SDK default glob also shows it at the project root. Do not use `AdditionalFiles` or `Content`. Do not set `CopyToOutputDirectory` (`None` already does not copy or pack). Do not replace `GlobalAnalyzerConfigFiles` with the `None` item. Do **not** move the file into `Properties/` on disk: analyzer-config scope follows the directory of the file, so it must stay next to the csproj. `Link` is UI-only.

Do not rename `Default` to Minimal: both seeds are the same full VS style dump. The split is **severity**, not breadth.

| Choice | Seed | What differs |
| --- | --- | --- |
| `Default` | `.project.editor.globalconfig.default` | Style dump. Naming stays **suggestion**. No CS1591 / nullable / IDE0005 overrides. |
| `Strict` | `.project.editor.globalconfig.strict` | Same dump. Naming **error**. Nullable (CS86xx) as **error**. Reserved identifiers (CA1716) as **error**. No CS1591 family and no IDE0005 (XML docs off; a later pack is a tool nupkg; IDE0005 needs `GenerateDocumentationFile` or every build warns `EnableGenerateDocumentationFile`). |
| `Off` | none | No file, no `EnforceCodeStyleInBuild`, no `OptimizeImplicitlyTriggeredBuild`. |

`Strict` needs the product C# default `Nullable`. Console `Program.cs` does not keep unused usings for the style dump (`KeepScaffoldUnusedUsings` is library/analyzer only). Needed `using` lines follow `DisableImplicitUsings`.

Seeds live under `TemplateAssets/CodeStyle/`. Each extra source copies that folder to the WinForms app project, **excludes** the other seed, and **renames** the chosen file to `.project.editor.globalconfig`. Do not leave a seed under `src/prj/__SourceName__/`: DocShell extra sources must `exclude` `Versioning/**`, `CodeStyle/**`, `Licenses/**`, `DirectoryMsBuild/**`, and `ProgramSamples/**`.

`UseProjectEditorGlobalConfig` is `(ProjectEditorGlobalConfig != "Off")`; the csproj `#if` does not need a new branch per dump.

`EnforceCodeStyleInBuild` is required or IDE naming stays IDE-only (SDK default is false). Compiler diagnostics in Strict (CS86xx) fail `dotnet build` without that flag; IDE1006 needs it. `OptimizeImplicitlyTriggeredBuild=false` is required with that same `#if`: Visual Studio skips analyzers on Test Explorer / F5 implicit builds (`IsImplicitlyTriggeredBuild`), so Run Tests can stay green while `dotnet test` fails the same IDE errors. Product assumption is VS MSBuild == `dotnet test` for app style gates. Do not add `EnableNETAnalyzers` / `RunAnalyzers*` `true` noise - those already default true on net8/net10. Coverlet `Threshold` still runs only on `dotnet test` (`coverlet.msbuild`), not Test Explorer.

This PropertyGroup/`GlobalAnalyzerConfigFiles` must appear **before** `ImportSdkTargets`. After that import the SDK has already loaded and ignores those items. The `None` Link is Solution Explorer only and can sit in the same `#if` block.

## `AnalysisMode`

App-only single choice, default **`Recommended`**. UI label **Code analysis mode**. CLI long name **`--AnalysisMode`**. Omit the switch → `Recommended`. Writes `<AnalysisMode>` on the WinForms app only (placeholder `__AnalysisMode__`). Tests and benchmark do not get the property. Combo-safe.

This is the SDK **CA** rule set, not code style. Do not merge it into `ProjectEditorGlobalConfig`. Do not add `None` or SDK `Default`: the WinForms app keeps analyzers on; `Minimum` / `Recommended` / `All` are the three product values. Do not add `AnalysisLevel` on the same switch (that pins a SDK rule version). Do not add `EnableNETAnalyzers` `true` noise.

Warnings only unless the consumer later sets `TreatWarningsAsErrors`. `All` is noisy on `Program`.

Place the PropertyGroup **before** `ImportSdkTargets`.

## `DocumentationTemplate`

Multi-choice, default **empty** (CLI) / **None** (Visual Studio). UI label **Documentation template**. CLI long name **`--DocumentationTemplate`**. Not part of `--InitAllRepoItems`. Same `TemplateAssets/DocShell.html` seed, repository-root `docs/` only (no package-docs destination: these apps are not a NuGet tool). Extra sources copy that file only (`exclude` of `Versioning/**`, `CodeStyle/**`, `Licenses/**`, `DirectoryMsBuild/**`, and `ProgramSamples/**`). Do **not** vendor the 25-file offline site in the template: the HTML file is the bootstrap contract, so a later checkpoint run acquires the versions that file pins then, not whatever was frozen in this pack. A newer DocShell release is a copy/replace of `TemplateAssets/DocShell.html` (keep that filename). Do not rewrite internal bootstrap paths such as `./documentation/css` here; those change in the DocShell product file itself.

| Choice | Path | When | Combo later app |
| --- | --- | --- | --- |
| `Repository` | `docs/DocShell.html` | first create | omit `Repository` (Exit 73 if stamped again) |
| `None` | nothing | - | wins over the other choices |

```powershell
# combo error (Exit 73) - Call 2 stamps docs/DocShell.html again
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --DocumentationTemplate Repository
# dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --DocumentationTemplate Repository
```

Repo-root `docs/` is not packed. `WriteRepoDocTemplate` is `Repository` and not `None`. The checkpoint infers repository documentation from repo-root `docs/`; it does not name this switch.

## Project roles and Git ignores

The app is publishable, not a NuGet tool. Output is `WinExe` with `UseWindowsForms`. Target frameworks are Windows TFMs (`net8.0-windows`, `net9.0-windows`, `net10.0-windows`; default `net8.0-windows` and `net10.0-windows`). Tests use the same `TargetFrameworks` list and `UseWindowsForms`; the optional benchmark uses the highest selected value as `TargetFramework` and `UseWindowsForms`. Publish RID is fixed `win-x64` (no wizard, no `--RuntimeIdentifier`, no x86). Restore and build stay RID-less. `IsPackable` is `false`, `PackAsTool` is `false`, no `ToolCommandName`, no NuGet-only csproj properties, no `Properties/NugetMetadata/` (primary exclude). Authors, Company, Copyright, and Description stay (assembly). `IsPublishable` is `true` so `PublishDefaultFramework` runs on bare `dotnet publish`. `Properties/Build/` is MSBuild (`ImportSdkTargets` last). `.project.editor.globalconfig`, `.config/dotnet-tools.json`, and `Directory.Build.*` stay next to the csproj. `Properties/AssemblyInfo.cs` grants `InternalsVisibleTo` the test assembly (`__SourceName__.Tests`) and the optional benchmark (`__SourceName__.Benchmark`). The entry point is `internal` `[STAThread] Main`; tests construct `Form1` and do not call `Main`. The test project's root `AssemblyInfo.cs` is only MSTest `Parallelize`. Tests and the optional BenchmarkDotNet executable explicitly set `IsPackable` and `IsPublishable` to `false`, including when automation calls each `.csproj` directly. The benchmark keeps one target framework (the highest selected) and runs with `dotnet run -c Release`. Versioning default is Nerdbank **Project** (`Properties/version.json`). Tests and benchmark are not packable. **`--NerdbankGitVersioning`** `Off` is VersionPrefix; `Repo` is the shared root file.

**Test project layout.** Mini-scopes in this order: general TFMs, language/debug (from `CSharpProjectOptions`), packaging, test configuration (`TestTfmsInParallel` + MSTest logger), Coverlet `#if` block, ReportGenerator `#if` block, NugetReport + `WriteNugetReport`, `.gitignore` hide, `ProjectReference`, then **External dependencies** `PackageReference`s last. Coverage is **`--TestCoverage`**: `Coverlet` (default), `CoverletAndReport`, `None`. Do not restore independent Coverlet/ReportGenerator bools: ReportGenerator consumes `@(CoverletReport)`. Coverlet is `coverlet.msbuild` + `CollectCoverage=true`; that **does** run on `dotnet test` (VSTest path, SDK 10) on Windows (`dotnet` ships MSBuild). The Coverlet `#if` also writes `Include` `[__SourceName__]*` (sourceName → the app assembly only), `ExcludeByFile` `**/Program.cs,**/*.g.cs`, `ExcludeByAttribute` `CompilerGeneratedAttribute`, and `Threshold` `100` / `line,branch,method` / `total`. Those are generate-time properties, not wizard fields: Visual Studio cannot show extra inputs only when Coverlet is selected. `--TestCoverage None` omits the whole PropertyGroup. Lower the threshold in the test csproj when 100% is not yet the gate. `Program.cs` and WinForms `ApplicationConfiguration.g.cs` are excluded so a first `dotnet test` still passes without starting the message loop. `Form1.Dispose` uses `[ExcludeFromCodeCoverage]` instead of excluding the designer file, because Coverlet `ExcludeByFile` on a partial drops the whole `Form1` type. Report/logger/NugetReport paths use `$([MSBuild]::NormalizeDirectory(...))` so a backslash path does not create a folder named `ReportGeneratorOutput\net10.0-windows`.

**Why `PublishDefaultFramework` exists.** When `IsPublishable` is `true` (this template's default), generated apps are used as `dotnet publish` with no `-f` and no extra properties. Publish must write one default TFM to `bin/Publish`. The SDK does **not** do that for multi-targeting: `Publish` is NETSDK1129 unless the caller passes a framework. The dispatch is that default (highest selected TFM, `__TargetFramework__`). Restore, build, test, pack, and `ProjectReference` stay on the stock SDK. These apps are not packed as a NuGet tool.

Do not put `TargetFramework` next to `TargetFrameworks` to avoid `-f`. That was the previous approach: MSBuild saw a single TFM, pack needed `BuildForPack`, and a `net8.0-windows` consumer could not reference the project. `_IsPublishing` on `TargetFramework` still fails when that consumer publishes (the flag is global).

Implementation (do not "simplify" into one always-imported file or back to `<Project Sdk="...">`): `Properties/Build/ImportSdkTargets.targets` always closes `Sdk.targets`; `PublishDefaultFramework.targets` loads only when `IsCrossTargetingBuild` is true. **`ImportSdkTargets` must be the last import in the WinForms app csproj.** That file *is* `Sdk.targets` plus the outer publish dispatch. The SDK reads properties and items while it loads (`EnforceCodeStyleInBuild`, `GlobalAnalyzerConfigFiles`, `AnalysisMode`, publish). Anything after that line is after the SDK and is ignored for those. `Project Sdk="..."` would append `Sdk.targets` after this file and overwrite the Publish override. `SourceControlState.targets` is a `BeforeTargets` hook on `GenerateAssemblyInfo` (SDK 8+ Source Link); it can sit just above the SDK close. `PublishRelease` keeps a direct project `dotnet publish` on Release. Tests may keep `SetTargetFramework`; external consumers must not need it. `<!--#if` in `.targets` is generate-time (`**/*.targets` in `specialCustomOperations`).

All three project files remove `.gitignore` from their `None` items so it stays on disk without appearing as a project item. The test ignore also covers generated `NugetReport/` output.

With `PlaceSolution` `SlnFolder` (default), each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/` next to that app's `.slnx` and handbook `Readme.md`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes (no nested `.gitignore`); delete that folder if you do not need it. `BesideCsproj` writes the `.slnx` and handbook next to the app csproj and does **not** create `src/sln/{Name}/` (`src/prj/{Name}/.gitignore` already ignores `.vs/`). Repository-root `.gitignore` is `InitRepoItems` `GitIgnore` / `--InitAllRepoItems` (first create only). Later apps omit Init and do not overwrite it. If root ignore is off, the `RepoRoot` variant and Visual Studio's extra root `.vs/` stay the owner's problem.

## `DirectoryMsBuildFiles`

App + this app's `.slnx` only. Bool, default **false**. UI label **Empty Directory.Build and Directory.Solution files**. CLI long name **`--DirectoryMsBuildFiles`**. Omit the switch → nothing. Combo-safe on `SlnFolder` and `BesideCsproj` (paths include the app name). `RepoRoot` stacks `Directory.Solution.*` at the repository root like stacking `.slnx` files. Not an Init* item and not `Directory.Packages.props` (CPM would not cover sibling tests). Tests and benchmark are not in this switch.

Seeds live under `TemplateAssets/DirectoryMsBuild/`. Extra sources copy `Directory.Build.props` / `.targets` next to the WinForms app csproj, and `Directory.Solution.props` / `.targets` next to this app's `.slnx` (`src/sln/{Name}/` for `SlnFolder`, `src/prj/{Name}/` for `BesideCsproj`, repo root for `RepoRoot` - later app then collides, same as stacking `.slnx` files). DocShell extra sources must `exclude` `Versioning/**` and `DirectoryMsBuild/**`.

The files are almost empty `<Project>` stubs with comments. MSBuild auto-imports them from those directories. Do not move `ImportSdkTargets` / analyzer config into them. The template does not `#if` properties into these files vs the csproj (possible, ugly). The WinForms app csproj `None Include`s the two `Directory.Build.*` files with `Link` under `Properties\` (Solution Explorer only). Do **not** move the files into `Properties/` on disk: auto-import follows the directory of the file, same as `.project.editor.globalconfig`. `Directory.Solution.*` stay beside the `.slnx`. When `PlaceSolution` is `BesideCsproj`, the WinForms app csproj also `Link`s those two solution files (same folder as the csproj) so they do not look like stray project items.

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems --DirectoryMsBuildFiles
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --DirectoryMsBuildFiles
```

## `DotNetToolManifest`

App project only. Bool, default **true**. UI label **Empty local dotnet-tools.json**. CLI long name **`--DotNetToolManifest`**. Omit the switch → empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it. Combo-safe: path includes the app name. Not Init. `tools` is `{}`; no `dotnet tool restore` on build. Tests and benchmark are not in this switch. `isRoot` is true so a later parent manifest does not merge in. `dotnet tool install --local` from the app folder fills the file. With `SlnFolder`, the handbook CWD (`src/sln/{Name}/`) does not see this manifest. With `BesideCsproj`, the handbook CWD is the app folder and does.

The WinForms app csproj `None Include`s the file with `Link` under `Properties\` (Solution Explorer only). Disk path stays `.config/`.

```powershell
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App1" --output $out --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "abcd" --name "...App2" --output $out --DotNetToolManifest false
```

## Other symbols worth not breaking

- **`ProjectLicense` / `RepoLicense` / `InitAllRepoItems` / `GitIgnore` / `SrcGlobalJson`**: see section above. Default MIT. Root `LICENSE` is the `RepoLicense` item or the CLI all-set (no package license metadata). Root `.gitignore` is `GitIgnore` in that same first-create set. This template does not stamp `ChoosingPackageBoundaries.md`. `src/global.json` is `SrcGlobalJson`: opt-in, default off, not in `--InitAllRepoItems`.
- **`NerdbankGitVersioning` / `WriteRepoVersionJson`**: see section above. Default `Project`. Root `version.json` is first-create only (`Repo` plus Init).
- **`CSharpProjectOptions`**: see section above. Do not split back into per-property dropdowns.
- **`ProjectEditorGlobalConfig`**: see section above. Default `Strict`. `Default` is suggestions-only. Seeds in `TemplateAssets/CodeStyle/`; generated disk name stays `.project.editor.globalconfig`. Csproj wire-up on the WinForms app only, before `ImportSdkTargets`.
- **`AnalysisMode`**: see section above. Default `Recommended`. App only. CA warnings, not build errors.
- **`DocumentationTemplate` / `WriteRepoDocTemplate`**: see section above. Default empty/`None`. Repository `docs/` only. Seed only; not the vendored site.
- **`PlaceSolution`**: single choice, default `SlnFolder` → `src/sln/__SourceName__/__SourceName__.slnx` plus handbook `Readme.md` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). `RepoRoot` on CLI renames to a root `.slnx`; `RepoRoot` in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS's conventional root `.slnx`. `RepoRoot` still writes `src/sln/{Name}/Readme.md` as notes and stacks every app's `.slnx` in one directory. `BesideCsproj` writes `src/prj/{Name}/{Name}.slnx` and the handbook next to the app csproj and does not create `src/sln/{Name}/`. The `.slnx` virtual folder `/sln/{Name}/` is omitted for `BesideCsproj`; `Readme.md` is a solution item beside the file.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`Author`**: required. CLI `--Author`.
- **`<Description>`**: not a template parameter. Generate leaves an empty CDATA block for multiline text; the checkpoint fills it (assistant-supported). Distribution is `dotnet publish`, not a NuGet tool nupkg.
- **`TargetFrameworks`**: Windows TFMs only (`net8.0-windows` / `net9.0-windows` / `net10.0-windows`). Default `net8.0-windows|net10.0-windows`. Joined into `__TargetFrameworks__` on the app and tests; highest selected is `__TargetFramework__` for benchmark and bare publish. Do not restore portable `net8.0` choices here.
- **Publish RID**: not a symbol. Always `win-x64` while `_IsPublishing`. No VS/CLI picker. Tests and benchmark stay RID-less.
- **`TestCoverage`**: single choice, default `Coverlet`. Replaces the two independent Coverlet/ReportGenerator bools. Coverage stats on `dotnet test` are opt-out; ReportGenerator is opt-in (`CoverletAndReport`). There is no Report-without-Coverlet. `--TestCoverage None` drops Coverlet too. Computed `CoverletMSBuild` / `ReportGenerator` drive the test csproj and sln-readme `#if`s.
- **`DirectoryMsBuildFiles`**: see section above. Default `false`. Empty `Directory.Build.*` beside the WinForms app and `Directory.Solution.*` beside this `.slnx`. No `Directory.Packages.props`.
- **`DotNetToolManifest`**: see section above. Default `true`. Empty `src/prj/{Name}/.config/dotnet-tools.json`. `--DotNetToolManifest false` skips it.
- Conditionals in `.md` / `.slnx` / `.targets` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.txt` and the renamed root `LICENSE` use `//#if`. License seeds under `TemplateAssets/Licenses/` may use a shallow copyright `//#if` / `//#else`; extra sources pick the file so there is no `ProjectLicense` `#if` in the text. This template does not use `UseWebSdk`; `ImportSdkTargets.targets` always closes `Microsoft.NET.Sdk`.
