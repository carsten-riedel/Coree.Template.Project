# `.template.config` (MultiLibraryRepository)

Product surface: **`.NET multi-library repository`**. `identity` is `CoreeTemplatesProjectMultiLibraryRepository`; CLI short name is `multilibraryrepo-coree`.

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. |
| `dotnetcli.host.json` | CLI long name for `InitDefaultRepoItems`; empty `shortName` avoids colliding short aliases. |
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
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin" --InitDefaultRepoItems Readme AIReleaseCheckpoint GitAttributes
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary2" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary3" --output "C:\Users\Valgrind\source\repos\MultiLibraryRepository-multisolution-optin"
```

After the first call the repo root has `README.md`, `TEMPLATE-RELEASE-CHECKPOINT.md`, and `.gitattributes`. Calls 2 and 3 add `src/prj` / `src/sln` trees only. Passing `--InitDefaultRepoItems Readme` again into the same folder is Exit 73 (collision); `--force` would overwrite.

One switch, values separated by **spaces**. Repeating `--InitDefaultRepoItems` per value also works. A quoted `Readme|AIReleaseCheckpoint|GitAttributes` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator in `ide.host.json`.

Subset on the first create (checkpoint only, no landing README):

```powershell
dotnet new multilibraryrepo-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "<repo>" --InitDefaultRepoItems AIReleaseCheckpoint
```

**Visual Studio:** `ide.host.json` selects Readme, checkpoint, and `.gitattributes` for a first create. The IDE requires at least one choice; for a second library into an existing repo choose **None** (it overrides leftover checks). Folgelibraries in the same folder are the CLI path above.

## AI-supported release checkpoint

The generated product still contains placeholders that can only become true **after implementation** — especially empty `src/prj/*/NugetAssets/Readme.md`. A human or an LLM can fill those from the code. That is a **gate before the first publish**, not a generate-time script and not standing agent rules.

**Not:** run-once / post-bootstrap right after `dotnet new`. The library may still be `Class1`.  
**Not:** a forever queue in the GitHub `README.md`. That file is the customer landing page.  
**Not:** a template-stamped `AGENTS.md`. That would collide with the consumer’s own agent file and would outlive the scaffold.

**Yes:** one repo-root file, `TEMPLATE-RELEASE-CHECKPOINT.md`, stamped only when `InitDefaultRepoItems` includes `AIReleaseCheckpoint` (first create). It is a **Template-Checkpoint-Release**: close template residue, then **self-dissolve**. After that, new chats read the libraries and the real NuGet docs.

Why the repo root, not `src/sln/{Name}/`: the first look at a combo repo is the customer surface; one fat checklist can say “update every NuGet readme in this repository” without a per-library marker. Libraries added later are in scope until the file is deleted.

The VS label **AI-supported release checkpoint** names the *job*, not a recurring agent run. The switch does not start a model. Someone later (person or LLM) works that file to 100% observable items, then deletes it. “AI-supported” belongs in the choice display name; it must not read as “edit with AI on every create.”

The generated file is the contract (ten numbered, checkable items). Do not put free-form “run this shell” instructions in it (prompt injection). Do not mix standing style rules into it — those must not self-delete.

## `InitDefaultRepoItems`

Multi-choice (`allowMultipleValues`), not N bools. Visual Studio shows **one group** of checkboxes (same shape as target frameworks).

| Host | Default | Empty set |
| --- | --- | --- |
| CLI | none selected (`defaultValue` `""`) | omit the switch |
| Visual Studio | `Readme\|AIReleaseCheckpoint\|GitAttributes` | not allowed; choose `None` |

`None` is first in the choice list. `sources` exclude each root file unless its choice is selected; `None` excludes all of them.

## Project roles and Git ignores

The library is packable and publishable. Tests and the optional BenchmarkDotNet executable explicitly set `IsPackable` and `IsPublishable` to `false`, including when automation calls each `.csproj` directly. The benchmark keeps one target framework (the highest selected) and runs with `dotnet run -c Release`.

**Why `PublishDefaultFramework` exists.** Generated libraries are used as `dotnet pack` and `dotnet publish` with no `-f` and no extra properties. Pack must include every selected TFM; publish must write one default TFM to `bin/Publish`. The SDK does the pack side from `TargetFrameworks` alone. It does **not** do the publish side: multi-targeting `Publish` is NETSDK1129 unless the caller passes a framework. The dispatch is that default (highest selected TFM, `__TargetFramework__`). Restore, build, test, pack, and `ProjectReference` stay on the stock SDK.

Do not put `TargetFramework` next to `TargetFrameworks` to avoid `-f`. That was the previous library: MSBuild saw a single TFM, pack needed `BuildForPack`, and a `net8.0` consumer could not reference the project. `_IsPublishing` on `TargetFramework` still fails when that consumer publishes (the flag is global).

Implementation (do not “simplify” into one always-imported file or back to `<Project Sdk="...">`): `ImportSdkTargets.targets` always closes `Sdk.targets`; `PublishDefaultFramework.targets` loads only when `IsCrossTargetingBuild` is true. `PublishRelease` keeps a direct project `dotnet publish` on Release. Tests may keep `SetTargetFramework`; external consumers must not need it. `<!--#if` in `.targets` is generate-time (`**/*.targets` in `specialCustomOperations`).

All three project files remove `.gitignore` from their `None` items so it stays on disk without appearing as a project item. The test ignore also covers generated `NugetReport/` output.

With `PlaceSolutionInSolutionFolder=true`, each `src/sln/{Name}/` gets its own `.gitignore` for `.vs/`. This creates no shared files on later library additions. The root-solution variant leaves repository-root ignore policy to the repository owner; it does not create or overwrite a shared root `.gitignore`. The same applies to the temporary root `.vs/` left by Visual Studio's extra solution.

## Other symbols worth not breaking

- **`PlaceSolutionInSolutionFolder`**: default true → `src/sln/ClassLibrary/ClassLibrary.slnx` (one `.slnx` per folder so `dotnet` / CI do not see sibling solutions). False on CLI renames to a root `.slnx`; false in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS’s conventional root `.slnx`. False also stacks every library’s `.slnx` in one directory.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`PackageAuthor`**: required.
- Conditionals in `.md` / `.slnx` / `.targets` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`). `.targets` is how `UseWebSdk` selects `Sdk.targets` in `ImportSdkTargets.targets`.
