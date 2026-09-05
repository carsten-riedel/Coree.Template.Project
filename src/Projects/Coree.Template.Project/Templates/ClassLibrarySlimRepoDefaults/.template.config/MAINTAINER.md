# `.template.config` (ClassLibrarySlimRepoDefaults)

Maintainer notes for this template host folder (`MAINTAINER.md`). Markdown here is **not** packed into `Coree.Template.Project` (`Templates\**\.template.config\**\*.md` is excluded). It is also **not** copied into a generated repository; only `template.json` / host JSON drive `dotnet new` and Visual Studio.

The generated root `README.md` lives beside this folder, one level up. That file **is** template content.

## Files

| File | Role |
| --- | --- |
| `template.json` | Identity, symbols, sources, post-actions. |
| `ide.host.json` | Visual Studio: visibility, labels, **defaults that differ from CLI**. `persistenceScope: none` so the New Project dialog does not reuse the last create. |
| `dotnetcli.host.json` | CLI long name for `InitDefaultRepoItems`; empty `shortName` avoids colliding short aliases. |
| `MAINTAINER.md` | This file. |

## `InitDefaultRepoItems`

Multi-choice (`allowMultipleValues`), not N bools. Visual Studio shows **one group** of checkboxes (same shape as target frameworks).

| Host | Default | Empty set |
| --- | --- | --- |
| CLI | none selected (`defaultValue` `""`) | omit the switch |
| Visual Studio | `Readme\|AIReleaseCheckpoint\|GitAttributes` | not allowed; choose `None` |

`None` is first in the choice list. If Visual Studio forces at least one value, `None` writes no root files. `None` also wins if other choices stay checked.

CLI (first library in a new folder, then more libraries into the same `--output`):

```powershell
dotnet new classlibslimrepodefaults-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary1" --output "<repo>" --InitDefaultRepoItems Readme AIReleaseCheckpoint GitAttributes
dotnet new classlibslimrepodefaults-coree --PackageAuthor "abcd" --name "Organization.Domain.ClassLibrary2" --output "<repo>"
```

One switch, values separated by spaces. A quoted `Readme|…` string is **not** valid CLI input on current `dotnet new`; `|` is only the host default separator (see `ide.host.json`).

`sources` exclude each root file unless its choice is selected. `None` excludes all of them.

## Other symbols worth not breaking

- **`PlaceSolutionInSrc`**: default true → `src/*.slnx`. False on CLI renames to a root `.slnx`; false in Visual Studio keeps `*.generated.slnx` so it does not overwrite VS’s conventional root `.slnx`.
- **`HostIdentifier` / `IsCliHost`**: bind + computed; used for that rename and for VS-only post-actions.
- **`PackageAuthor`**: required.
- Conditionals in `.md` / `.slnx` use `<!--#if` on their own lines (`specialCustomOperations`, `wholeLine`).
