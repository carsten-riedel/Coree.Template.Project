# Writing .NET project templates

Someone will open the folder you emit and try to work. The template is for that moment: a tree that already makes sense, a first command that already runs, names that already look like theirs. `template.json` is only how you get there.

This is guidance — what the engine can do, how the pieces connect, working ways to try a result. It does not fix a product, a folder layout, or a default. Official maps: [custom templates](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates), [template.json schema](https://json.schemastore.org/template).

A template is a **generator**. `dotnet new` and Visual Studio copy a stub tree, then rename, replace, exclude, and drop lines. After that, MSBuild loads whatever `.csproj` you emitted — a second engine, with its own order. Most surprises are those two pipelines meeting. A little care in the stub (sample code that survives the defaults you stamp, licenses that read as English, a readme that knows where the terminal is) is cheaper than a clever wizard.

---

## What you can assemble

Three artifacts share files and have different consumers.

The **stub** is a normal project (or repo) with placeholder names (`MyLibrary`, `MyAuthor`). The engine only copies and rewrites; it does not fix restore or tests.

The **template** is that stub plus `.template.config/`. That folder is the host contract. It is not copied into the generated repo. Markdown next to `template.json` stays with the generator; a file at the stub root is content unless you exclude it.

The **template pack** is a nupkg (`PackageType` `Template`) that ships one or more template folders. The nupkg a *generated* library later publishes is a different package.

You can author the stub until it builds, then wrap the engine around it. You can also start from `template.json` and grow a stub; the first path usually fails less, because every create is then only a generator problem.

### Create pipeline (what each step can still see)

1. **Host collects parameters** — CLI switches or the New Project dialog. Missing values become `template.json` defaults, except where `ide.host.json` supplies a Visual Studio default. Two hosts can therefore feed different symbol values into the same template.
2. **Engine evaluates symbols** — parameters, computed predicates, generators (`join`, `switch`, `now`, `constant`), bind of `HostIdentifier`. Later conditions see these names, not raw CLI text.
3. **Each source is copied** — default stub tree, plus any extra sources. A file that is in no source never appears, no matter what `#if` you write.
4. **Modifiers exclude or rename** — exclude here means the file never reaches replace or `#if`. Rename here is the path `sourceName` will rewrite next.
5. **`sourceName` / `fileRename` / `replaces`** — string rewrite of paths and text that survived. Comments, licenses, and filenames are equal.
6. **Conditionals** — `#if` / `<!--#if` / `//#if` drop lines in file types the processor knows (built-in or `specialCustomOperations`). Unregistered types keep the tokens as literal text.
7. **Post-actions** — mostly Visual Studio (open a file, show a message). CLI can ignore them.

If a file should exist, it has to survive 3–6. If it should not, exclude it at 4 (whole file) or strip lines at 6. An MSBuild `Condition` still ships the XML.

---

## Anatomy you can use

```text
MyTemplate/
  .template.config/
    template.json          # identity, symbols, sources, post-actions
    ide.host.json          # Visual Studio labels, visibility, defaults
    dotnetcli.host.json    # CLI long names and short aliases
    icon.png               # optional picker icon for this template
  MyLibrary.csproj
  Class1.cs
```

`template.json` sections and what they are for:

- **Listing** — `identity` (install key; recycling it overwrites another template), `shortName` (`dotnet new` verb; a suffix avoids SDK names like `classlib`), `name` / `description` / `classifications` (dialog and `dotnet new list`).
- **Wizard shape** — `tags.language`, `tags.type` (`project` or `solution`), optional `editorTreatAs: solution`. This chooses the dialog and often the outer-folder behavior, not whether generate succeeds.
- **Naming** — `sourceName`, `defaultName` (suggested name; people copy it), `preferNameDirectory`, `forms`, `guids`.
- **Symbols** — inputs to every condition.
- **Sources** — which files exist at all. `#if` only edits files that were copied.
- **`specialCustomOperations`** — conditionals for extensions the host does not preprocess.
- **`primaryOutputs` / `postActions`** — after the tree exists. They do not create files.

`ide.host.json` and `dotnetcli.host.json` do not rewrite content by themselves. They change **which symbol values** the host supplies. Those values do change the tree.

**Icons are three surfaces**, if you use them: pack `PackageIcon` (NuGet listing and VS picker fallback), `.template.config/icon.png` (this template's picker; overrides the pack), generated project icon (consumer nupkg). Leaving the template icon absent is possible; the pack icon then shows for every template in the pack.

**Outer folder.** Visual Studio already creates a folder for a solution template. `preferNameDirectory: true` then yields `Name/Name/…`; `false` yields `Name/…`. From the CLI into an empty directory, `true` often keeps a project template from dumping files into the current directory. Same boolean, host-sensitive.

---

## Naming: what the rewrite can do

`sourceName` is replaced in **paths, file names, and contents**. If the stub folder, `.csproj`, and namespace all use that same string, one pass renames all three. If they do not, you add extra `replaces` / `fileRename` symbols.

`name` is what the user typed (`Organization.Domain.Widgets2`). Derived symbols can transform `name` without changing `sourceName`. That is how you get a folder `Widgets2` and a type `Widgets`, or a namespace with dots and an MSBuild property prefix without dots.

`replaces` is a global string replace. Tokens that also appear in real prose get rewritten there too. Working placeholders:

- Author: `MyAuthor` (a required parameter avoids shipping the literal).
- Year / date: `1975` / `1975-01-01` plus `generator: now` (`yyyy` / `yyyy-MM-dd`).
- Joined or switched MSBuild values: `__TargetFrameworks__`, `__TargetFramework__`.

```json
"forms": {
  "WithoutTrailingNumbers": {
    "identifier": "replace",
    "pattern": "\\d+$",
    "replacement": ""
  },
  "RemoveDots": {
    "identifier": "replace",
    "pattern": "\\.",
    "replacement": ""
  }
}
```

Chain derived symbols when you need both transforms. Order matters: stripping digits first on `Net8` drops the `8`.

`fileRename: "Default"` on a symbol whose value is the user name turns `Default.csproj` into that name when `sourceName` is already busy.

Optional company / copyright holder: string default `""` plus computed `HolderIsSet` (`(Holder != "")`). Files can then branch instead of emitting empty `<Company>`.

For a stub `.sln`, listing those GUIDs in `template.json` `"guids"` lets the host mint unique IDs per create. `.slnx` does not use that mechanism the same way.

---

## Symbols: jobs the wizard can express

Symbols are the wizard **and** the condition language. Sources and `#if` see symbol values, not raw CLI text.

Kinds:

- **parameter** — `bool`, `string`, `choice` (optional `allowMultipleValues`). The only kind that appears as `--Flag` or a dialog field.
- **computed** — predicate over other symbols. One name (`WriteRootLicense`) instead of the same expression in five excludes.
- **generated** — `now`, `join`, `switch`, `constant`, `guid`, … Often `replaces` / `fileRename`.
- **bind** — host values. `host:HostIdentifier` with `defaultValue` `dotnetcli` keeps CLI dry runs on the CLI branch. Preview is `dotnetcli-preview`; a check for only `dotnetcli` will miss it.
- **derived** — `name` (or another symbol) through a `form`.

A checkbox per MSBuild property is possible. It also allows combinations that are not products (a reporter with no coverage output). Alternatives: one combined choice, or `isEnabled` on the dependent field.

### `==` on a multi-choice

On a single choice, `License == MIT` is equality. On a multi-choice, `==` means **this value is among the selected items**. If the user ticked `Readme` and `License`, both `(Init == Readme)` and `(Init == License)` are true. “Write README” is then “None is not selected, and (all-set bool or `Init == Readme`)”.

Visual Studio cannot leave a multi-choice empty. The CLI can (`defaultValue` `""`). If “write nothing” is a job, an explicit **None** choice is possible; listing it first is a working snap if a host picks the first item. None can be made to win over leftover ticks and over an “all” bool.

`enableQuotelessLiterals` lets conditions read `License == MIT`.

### Wire vs stamp

One dropdown can mean two disk jobs: **wire** this unit (PackageReference, import) on every create, and **stamp** a shared file (`version.json` at repo root) on first create only. If one choice does both, the second create into the same folder collides (exit 73) or overwrites. Two computed flags split that:

```json
"UseRepoVersioning": {
  "type": "computed",
  "value": "(Versioning == \"Repo\")"
},
"WriteRepoVersionJson": {
  "type": "computed",
  "value": "(Versioning == \"Repo\") && (Init != \"None\") && InitAll"
}
```

Exit 73 on restamp is available as a brake. `--force` overwrites when that is intended.

### Join and switch

A multi-value TFM choice is a set. MSBuild wants a semicolon list:

```json
"TargetFrameworksValue": {
  "type": "generated",
  "generator": "join",
  "replaces": "__TargetFrameworks__",
  "parameters": {
    "symbols": [{ "type": "ref", "value": "TargetFrameworks" }],
    "separator": ";",
    "removeEmptyValues": true
  }
}
```

A single “highest TFM” (publish default, a helper project) is a `switch` with cases from highest to lowest. Because `==` on a multi-choice is “contains”, listing net8 first would win whenever net8 is in the set.

Always-written properties are possible: both branches of `#if` / `#else` so the csproj never omits `Nullable`. Omitted properties follow a later SDK default. The other option is to leave the property out and accept SDK evolution.

Some work is possible later instead of at create: empty `<Description>` filled after there is code; API baseline files on first build; a documentation bootstrap file instead of a vendored site.

---

## Sources: copy, skip, copy again, rename

Ask of each file: everyday product, optional tree, optional lines, same bytes in two places, or exclusive variants with one output name.

| Situation | Mechanism that can do it |
| --- | --- |
| Always copy with the stub | Default source |
| Optional whole project / folder | `modifiers.exclude` when the feature is off |
| Optional lines in a file that always exists | In-file `#if` (see next section) |
| Same seed → two destinations | Two extra sources, same `source`, different `target` and `condition` |
| MIT vs Apache, both become `License.txt` | Extra source per choice: exclude siblings, `rename` the winner |
| Seeds must not appear as `TemplateAssets/…` | Default source excludes `TemplateAssets/**` |
| First-create `README.md` | In the default tree, excluded unless Init says so — not next to extra-source seeds aimed at `./` |

```json
"sources": [
  {
    "modifiers": [
      {
        "condition": "(Benchmark == false)",
        "exclude": [ "bench/**" ]
      },
      {
        "condition": "IsCliHost",
        "rename": { "Widgets.generated.slnx": "Widgets.slnx" }
      }
    ]
  },
  {
    "source": "./TemplateAssets/Licenses/",
    "target": "./src/NugetAssets/",
    "condition": "(License == \"MIT\")",
    "modifiers": [
      { "exclude": [ "Apache2.txt", "Custom.txt" ] },
      { "rename": { "MIT.txt": "License.txt" } }
    ]
  }
]
```

A second extra source can copy the same MIT seed to `./LICENSE` when a first-create flag is on. Standing docs you stamp independently of that copy belong in the default tree; otherwise an extra source aimed at `./` takes them along.

`primaryOutputs` paths are after source rename, before `sourceName` replace. Open-file post-actions index that list (`"files": "0"`). If only CLI renames `.generated.slnx`, the listed path can stay the `.generated` name so both hosts share an index.

---

## Conditionals by file type

The engine always preprocesses **C#** (`.cs`) and **XML that looks like a project** (`.csproj`, often `.props`). Other extensions keep `#if` as text unless you register them.

Registration is `specialCustomOperations`: a glob, a token style, `wholeLine: true` (drop the entire line), `trim: true`, `evaluator: C++` (`&&`, `||`, `==`, `!=`). Tokens sit on **their own lines**. Nested `#if` is possible; close in reverse order.

A block you can copy and trim to the extensions you actually condition:

```json
"specialCustomOperations": {
  "**/*.csproj": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.props": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.targets": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.slnx": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.md": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.xaml": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["<!--#if"], "else": ["<!--#else"], "elseif": ["<!--#elseif"], "endif": ["<!--#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.txt": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["//#if"], "else": ["//#else"], "elseif": ["//#elseif"], "endif": ["//#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/LICENSE": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["//#if"], "else": ["//#else"], "elseif": ["//#elseif"], "endif": ["//#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.json": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["//#if"], "else": ["//#else"], "elseif": ["//#elseif"], "endif": ["//#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]},
  "**/*.sln": { "operations": [{ "type": "conditional", "configuration": {
    "if": ["//#if"], "else": ["//#else"], "elseif": ["//#elseif"], "endif": ["//#endif"],
    "trim": true, "wholeLine": true, "evaluator": "C++"
  }}]}
}
```

`.csproj` is listed in case a host is uneven; many hosts already preprocess it. `.targets` is the one that commonly needs the glob: generate-time SDK flavor inside `Import Sdk.targets` will otherwise stay as comments and break the import.

Two clocks: **generate-time** `#if` disappears at create. **MSBuild `Condition`** stays in the file and runs at build (`$(TargetFramework)`, `$(IsPackable)`). A generate-time `#if` cannot see an inner TFM; that value does not exist yet.

### `.cs` — built-in C# processor

Token: `#if( Symbol )` … `#endif` (parenthesis form is what the template processor reliably takes). The engine strips it at create, so the compiler never sees `Symbol` unless you *wanted* a real compilation symbol (`DEBUG`, `NET8_0`).

```csharp
#if( KeepScaffoldUnusedUsings )
using System;
using System.IO;

#endif
namespace MyLibrary
{
    public static class Class1
    {
        public static string Foo() => "123";
    }
}
```

Optional feature in the middle of a method:

```csharp
public void Run()
{
#if( DependencyInjection )
    var host = CreateHost();
#else
    Application.Run(new MainForm());
#endif
}
```

If the optional code is a whole file, `sources` exclude is available instead of wrapping the file in `#if`.

### `.csproj` / `.props` — XML comments

Token: `<!--#if (Symbol) -->` on its own line.

Always-write a property (both values explicit):

```xml
<!--#if (CSharpProjectOptions == "Nullable") -->
<Nullable>enable</Nullable>
<!--#else -->
<Nullable>disable</Nullable>
<!--#endif -->
```

Optional block (omit the whole group when off):

```xml
<!--#if (Coverlet) -->
<PropertyGroup>
  <CollectCoverage>true</CollectCoverage>
  <Threshold>100</Threshold>
</PropertyGroup>
<ItemGroup>
  <PackageReference Include="coverlet.msbuild" Version="6.0.4" PrivateAssets="all" />
</ItemGroup>
<!--#endif -->
```

SDK flavor at the top of the file:

```xml
<!--#if (UseWebSdk) -->
<Project>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk.Web" />
<!--#else -->
<Project>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
<!--#endif -->
```

Empty vs set string:

```xml
<!--#if (PackageCompany != '') -->
<Company>MyCompany</Company>
<!--#else -->
<Company>MyAuthor</Company>
<!--#endif -->
```

`MyCompany` / `MyAuthor` are then `replaces` targets.

### `.targets` — register it

Without `**/*.targets` in `specialCustomOperations`, `<!--#if -->` stays in the file. MSBuild then does not treat it as an import switch.

```xml
<Project>
  <!--#if (UseWebSdk) -->
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk.Web" />
  <!--#else -->
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
  <!--#endif -->
</Project>
```

Build-time remaining conditions use MSBuild, not hash-if:

```xml
<Import Project="PublishDefaultFramework.targets" Condition="'$(IsCrossTargetingBuild)' == 'true'" />
```

### `.slnx` — register `<!--#if -->`

Paths inside the solution depend on where the file will sit. Nested hash-if is possible:

```xml
<Folder Name="/prj/">
<!--#if (PlaceSolution == "SlnFolder") -->
    <Project Path="../../prj/MyLibrary/MyLibrary.csproj" />
<!--#elseif (PlaceSolution == "BesideCsproj") -->
    <Project Path="MyLibrary.csproj" />
<!--#else -->
    <Project Path="src/prj/MyLibrary/MyLibrary.csproj" />
<!--#endif -->
<!--#if (Benchmark) -->
<!--#if (PlaceSolution == "SlnFolder") -->
    <Project Path="../../prj/MyLibrary.Benchmark/MyLibrary.Benchmark.csproj" />
<!--#elseif (PlaceSolution == "BesideCsproj") -->
    <Project Path="../MyLibrary.Benchmark/MyLibrary.Benchmark.csproj" />
<!--#else -->
    <Project Path="src/prj/MyLibrary.Benchmark/MyLibrary.Benchmark.csproj" />
<!--#endif -->
<!--#endif -->
</Folder>
```

If the optional project is excluded from sources when `Benchmark` is false, the inner `#if` is only needed when the `.slnx` still lists it.

### `.sln` (classic) — `//#if`

Solution files are not XML. Line comments work:

```text
	ProjectSection(SolutionItems) = preProject
    //#if (AddGlobalsJson)
		global.json = global.json
    //#endif
		Readme.md = Readme.md
	EndProjectSection
```

Register `**/*.sln` if the host does not already strip those lines. `guids` in `template.json` still apply to the stub GUIDs in this file.

### `.md` — register `<!--#if -->`

HTML comments are invisible in rendered markdown and valid tokens for the processor:

```markdown
# MyLibrary
<!--#if ((HostIdentifier == "vs") && (PlaceSolution == "SlnFolder")) -->

Visual Studio also wrote a `.slnx` at the repository root. Open the `.slnx` in this folder and delete the extra root file if you do not need it.
<!--#endif -->

<!--#if (Benchmark) -->
Optional benchmark project: `src/prj/MyLibrary.Benchmark/`
<!--#endif -->
```

Host-specific paragraphs are possible this way. CLI does not need the Visual Studio sentence.

### `.txt` and `LICENSE` — `//#if`

Plain text has no XML comments. `//#if` is available once registered. `LICENSE` has no extension; glob `**/LICENSE` separately from `**/*.txt`.

Shallow branch (copyright line). Extra sources can pick MIT vs Apache so the seed does not contain a `ProjectLicense` tree:

```text
MIT License

//#if (PackageCopyrightHolderIsSet)
Copyright (c) 1975 MyCopyrightHolder
//#else
Copyright (c) 1975 MyAuthor
//#endif
```

`1975` and `MyAuthor` / `MyCopyrightHolder` are `replaces` targets.

### `.json` / `global.json` — `//#if`

JSONC (comments allowed) can use the same tokens. After processing, the comments are gone and the leftover JSON needs to be valid (commas, one `"version"` key):

```json
{
  "sdk": {
    "rollForward": "latestFeature",
    //#if (IsNet8)
    "version": "8.0.0"
    //#endif
    //#if (IsNet10)
    "version": "10.0.0"
    //#endif
  }
}
```

Computed bools per TFM (`IsNet8`) make this readable. `join` into one version string is another way if you only need one line.

### `.xaml` — register `<!--#if -->`

Attribute-sized branches: token lines around the attributes. The XAML has to remain well-formed after a branch is dropped (no dangling commas; XAML uses whitespace).

```xml
<UserControl
             <!--#if (ViewModel) -->
             xmlns:viewmodel="clr-namespace:MyApp.ViewModels"
             d:DataContext="{d:DesignInstance Type=viewmodel:NavbarViewModel}"
             <!--#endif -->
             d:DesignHeight="450">
    <mah:HamburgerMenu
                                <!--#if (ViewModel) -->
                                IsPaneOpen="{Binding Path=IsOpen, Mode=TwoWay}"
                                <!--#else -->
                                IsPaneOpen="False"
                                <!--#endif -->
                                DisplayMode="CompactInline" />
</UserControl>
```

Optional ViewModel *files* can still be `sources` exclude; this `#if` is for markup that stays in a file that always exists.

### `.gitignore` / `.gitattributes` / `.editorconfig`

`#` is already a comment in these formats. Template `#if` collides with that. Working options: **exclude the whole file**, or **extra source + rename** of a seed (`.project.editor.globalconfig.strict` → `.project.editor.globalconfig`). In-file hash-if is a poor fit.

A `.gitignore` you do want on disk can still be globbed into the project as a `None` item. `<None Remove=".gitignore" />` keeps the file and drops the item.

### What to prefer when several mechanisms fit

- Whole optional tree → exclude.
- Optional lines in a surviving file → hash-if for that extension.
- Same bytes, two paths → extra sources, not a file that tries to `#if` its own name.
- Exclusive full-file variants → extra source per choice + rename, not one mega-file of `#if License`.

---

## Two hosts: what they allow

`dotnet new` and Visual Studio share `template.json`. They do not share empty-set rules, alias assignment, persistence, or extra files they write beside yours.

**Empty multi-choice.** CLI can default to `""` (omit the switch = write nothing). Visual Studio requires at least one value. A first-create set can live in `ide.host.json` `defaultValue` (`Readme|License|…`). Later create in VS can be an explicit **None**. Matching *product* on first create is possible; matching *switches* is not always possible. Putting the VS first-create set into `template.json` `defaultValue` makes the second CLI create into the same `--output` restamp shared files (exit 73) unless the caller passes None or `--force`.

**CLI-only “all of that set”.** A bool (`InitAll`) is possible. `ide.host.json` `"isVisible": false` hides it. An `All` checkbox next to `None` in the same group can be ticked together; hiding the bool avoids that.

**Persistence.** `persistenceScope: none` on symbols whose default is a first-create set. Otherwise the next New Project dialog inherits yesterday's None or subset.

**Aliases.** CLI invents `-P` from `--PackageAuthor`. `dotnetcli.host.json` `"shortName": ""` turns that off per symbol.

**Separator.** `|` in `ide.host.json` defaults. Current `dotnet new` wants spaces (`--InitRepoItems Readme License`) or a repeated switch. The IDE string is not CLI input. The engine also splits multi-choice strings on `,` and cannot escape `|` or `,` inside a choice value ([multi-choice specifics](https://github.com/dotnet/templating/wiki/Reference-for-template.json)).

**Visual Studio ticks.** The generic multi-choice combo (any name other than the built-in `Framework` / `TargetFrameworks` picker) joins `displayName`s with commas for the closed caption, then splits that caption to tick boxes. A comma or pipe in `displayName` shows the right line and leaves every box unchecked; Create can still send the `ide.host.json` default. Hyphens are safe. `Framework` (exact casing) and `TargetFrameworks` use the native TFM control and only understand installed TFM tokens — a custom name whose values happen to look like `net8.0` still gets the generic combo. A pipe `defaultValue` in `template.json` for that combo is the same caption-without-ticks failure; keep `template.json` empty and put the VS set in `ide.host.json`, like Init. List the defaulted choices first as a contiguous prefix if a host snaps to the first item. `isRequired: false` keeps Create enabled when the caption is not a bound selection (`isRequired` is unreliable in VS: [templating#6870](https://github.com/dotnet/templating/issues/6870)).

**Solution file vs Visual Studio's extra `.slnx`.** VS often writes `{Name}.slnx` at the repo root. Possible responses: stub `{Name}.generated.slnx` and rename to `{Name}.slnx` only when `IsCliHost`; or always place the template solution under `src/sln/{Name}/` and mention the extra root file in a VS-only post-action.

**Post-actions.** Open file (`84C0DA21-51C8-4541-9940-6CA19AF04EE6`, `"files": "0"`) and message box (`AC1156F7-BB77-4DB8-B28F-24EEBCCA1E5C`) gated on `HostIdentifier == "vs"`.

```json
{
  "condition": "(HostIdentifier == \"vs\")",
  "actionId": "84C0DA21-51C8-4541-9940-6CA19AF04EE6",
  "args": { "files": "0" },
  "continueOnError": true
}
```

---

## Repeat create into the same folder

Possible when the product is “add another unit next to the first.” Not required for a one-shot project template.

Units use paths that include the unit name (`src/prj/{Name}/`). Shared files (`README`, `LICENSE`, root `.gitignore`) do not. Copying a shared path twice is a collision (exit 73) unless `--force`. Init (multi-choice + optional bool) plus computed stamp flags is a working split: call 1 stamps the roof, call 2 omits Init and only adds a unit.

CLI later-create: omit Init switches. Visual Studio later-create: None (empty is not allowed). Combo-safe feature switches write under `{Name}` (package docs, per-library `version.json`). Shared destinations (`docs/` at repo root) restamp unless gated like Init.

---

## Generated csproj techniques that are available

MSBuild loads the emitted csproj afterwards. The SDK reads properties and items **while it imports**. `<Project Sdk="…">` imports `Sdk.targets` at the **end of the file**. A custom `Publish`, `GlobalAnalyzerConfigFiles`, `AnalysisMode`, or `WarningsAsErrors` after that implicit import is XML that the SDK already missed.

Explicit close is possible:

```xml
<Project>
  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
  <!-- properties, items, your .targets -->
  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
</Project>
```

A wrapper `.targets` that *is* that last import keeps the csproj short. Putting `Sdk="…"` back on `<Project>` would import `Sdk.targets` again after the wrapper and overwrite a custom `Publish`.

**Multi-targeting.** `join` → `<TargetFrameworks>`. Packable libraries often use the plural so the nuspec gets per-TFM dependency groups. `dotnet publish` without `-f` on a multi-targeted project is NETSDK1129. Setting `TargetFramework` *and* `TargetFrameworks` to dodge `-f` makes MSBuild treat the project as single-TFM; pack and `ProjectReference` then omit frameworks. A targets file that dispatches one default TFM (the `switch` result) and still closes `Sdk.targets` last is a working publish path. Helpers that are truly one TFM can use `__TargetFramework__` alone.

Tests that multi-target can set `SetTargetFramework` on `ProjectReference`. `$([MSBuild]::NormalizeDirectory(...))` avoids a `\` in a property becoming a directory name on Linux.

**Analyzer config.** Scope follows the **directory of the file**. Next to the csproj is the compiler scope. A `Link` under `Properties\` is Solution Explorer only. `GlobalAnalyzerConfigFiles` (`Visible="false"`) is the compiler input; `None Remove` the glob first. Place it before `Sdk.targets`. `EnforceCodeStyleInBuild` is how naming rules participate in `dotnet build`. `OptimizeImplicitlyTriggeredBuild=false` is how Test Explorer / F5 keep the same analyzers as `dotnet test`.

**Roles.** `IsPackable` / `IsPublishable` can be explicit on every project. Test tooling (Coverlet, reporters) can live on the test project so the primary nupkg stays a library. `InternalsVisibleTo` can use `sourceName` so it tracks the test assembly name.

**Scaffold.** Generate-time gates apply to sample types too. Unused-using errors and a sample full of unused usings fail the first build; a computed `KeepScaffoldUnusedUsings` can omit those usings when the gate is on. Coverage 100% wants a sample that is covered.

---

## Packaging the pack

Available knobs: `PackageType` `Template`, no build output, `NoDefaultExcludes` true (otherwise `.gitignore` / `.editorconfig` inside templates do not pack), exclude `bin`/`obj`/`.vs` if someone opened a stub `.slnx`, exclude maintainer markdown under `.template.config` if it should stay in git only.

**Nested template** (outer pack emits a template). A real inner `.template.config/template.json` is seen as another template while you author the outer pack. Stub names plus `constant` + `fileRename` at create:

```json
"templateDirRename": {
  "type": "generated",
  "generator": "constant",
  "parameters": { "value": ".template.config" },
  "fileRename": "templateDir"
},
"templateJsonRename": {
  "type": "generated",
  "generator": "constant",
  "parameters": { "value": "template.json" },
  "fileRename": "templateJson.txt"
}
```

---

## Worked patterns (copy and rename)

### Optional extra project

Symbol: bool `Benchmark`, default false. Exclude the folder when off. List it in `.slnx` behind `<!--#if (Benchmark) -->` if the solution file always exists.

```json
{
  "condition": "(Benchmark == false)",
  "exclude": [
    "src/prj/MyLibrary.Benchmark/**"
  ]
}
```

### First-create root file

CLI: `Init` multi-choice default `""`, plus bool `InitAll` default false. VS: `ide.host.json` default `Readme|License`, `persistenceScope` `none`, `InitAll` hidden, **None** first in the choice list.

```json
{
  "condition": "((Init == \"None\") || ((InitAll == false) && (Init != \"Readme\")))",
  "exclude": [ "README.md" ]
}
```

`None` excludes even when `InitAll` is on.

### License seed → `License.txt` and optional root `LICENSE`

Seeds under `TemplateAssets/Licenses/`. Default source excludes `TemplateAssets/**`. Extra source per license to `NugetAssets/License.txt`. Another extra source to `./LICENSE` when `WriteRepoLicense`. Shallow `//#if (HolderIsSet)` inside the seed; no `ProjectLicense` `#if` in the text.

### Highest TFM for a helper

`switch` as above → `__TargetFramework__` in the benchmark csproj `<TargetFramework>`. Library keeps `__TargetFrameworks__`.

### CLI vs VS solution file at repo root

Stub `MyLibrary.generated.slnx`. Rename to `MyLibrary.slnx` when `IsCliHost` and the file would sit at repo root. VS keeps `.generated` so it does not overwrite the IDE's `{Name}.slnx`.

### Combined choice instead of two bools

Coverage HTML needs Coverlet files. One choice `Coverlet` / `CoverletAndReport` / `None`. Computed `CoverletMSBuild` / `ReportGenerator` keep `#if` names in the test csproj and the readme.

---

## Lookup

| You can… | With |
| --- | --- |
| Rename project, folder, namespace together | `sourceName` aligned with the stub |
| Extra identifier from `MyApp2` / dotted names | Derived + forms |
| Current year | `now` replacing `1975` |
| Required author | Parameter → `MyAuthor` |
| Optional company without empty tags | String + computed `*IsSet` + `#if` |
| Several TFMs | Multi-choice + `join` (or `#if` append) |
| One highest TFM | `switch`, highest case first |
| Drop an optional project | `exclude` |
| Drop optional lines | Hash-if for that extension |
| Same text in two paths | Extra sources |
| Exclusive files, one disk name | Extra source per choice + `rename` |
| First-create-only files | Empty CLI default, IDE default, None, stamp flag |
| Add a sibling unit later | Name in the path; omit Init |
| Different host defaults | Bind + `ide.host.json` + `persistenceScope: none` |
| Hide a CLI-only bool | `isVisible: false` |
| Stop CLI stealing `-P` | Empty `shortName` |
| Dependent wizard field | `isEnabled` or a combined choice |
| Override SDK `Publish` | `Sdk.props` … `Sdk.targets` last |
| Honor analyzer items | Same: before `Sdk.targets` |
| Avoid VS `.slnx` clash | `.generated.slnx` + CLI rename, or a per-unit folder |
| Open a file in VS after create | `primaryOutputs` + post-action |
| Emit a template from a template | Rename `templateDir` at create |
| Pack `.gitignore` in the template nupkg | `NoDefaultExcludes` |
| Keep `.gitignore` off the project glob | `<None Remove=".gitignore" />` |

---

## Trying it out

`template.json` parsing is not a test. The test is a created folder: both hosts, the defaults, a non-default choice, and — if units can be added later — a second create into the same directory.

Two install paths are available. They are not interchangeable for Visual Studio.

**Folder install** is the fast CLI loop:

```powershell
dotnet new install "C:\path\to\MyTemplate" --force
dotnet new my-short-name --name Organization.Domain.Widgets --output $out --PackageAuthor "Ada"
```

`--force` replaces the previous install. Create into a throwaway `$out`. `--dry-run` lists files without writing; it will not catch a csproj the SDK ignores, leftover `#if` in an unregistered extension, or Visual Studio's extra `.slnx`.

**Pack install** is what Visual Studio actually loads. The IDE caches template packs. Installing only the folder, then opening New Project, is a reliable way to debug yesterday's nupkg. A working local ritual:

1. Close Visual Studio (`devenv`). If it stays open, the template cache can keep the previous pack.
2. `dotnet new uninstall <PackageId>` when that id is already installed.
3. `dotnet pack` the template-pack csproj (Debug is enough).
4. `dotnet new install <the-nupkg> --force`.
5. Confirm `dotnet new list` shows the short name, *then* open Visual Studio and create.

This repository does that in `Test-LocalTemplatePackage.ps1`: refuse to run while `devenv` is up, uninstall `Coree.Template.Project`, pack Debug, install the single `*-local.nupkg`. After it prints ready, the New Project dialog and `dotnet new` share that build. The script is one way to keep CLI and VS on the same bits; any equivalent “close IDE, pack, reinstall nupkg” loop does the same job.

### What to look at in the created tree

Walk it like a guest. Open the folder, not the stub.

- **Names.** Folder, csproj, namespace, and `InternalsVisibleTo` match what was typed. No leftover `MyLibrary` / `MyAuthor` / `1975` / `__TargetFrameworks__`.
- **Tokens.** No `<!--#if`, `//#if`, or `#if( Symbol )` left in files you expected to preprocess. If they remain, that extension is not registered (or the token is not on its own line).
- **Seeds.** No `TemplateAssets/` folder, no `MIT.txt` beside `License.txt`, no `.generated.slnx` on CLI if you renamed it.
- **Defaults.** `dotnet restore` / `build` / `test` / `pack` (whatever the product is) on an omit-the-switch create. Scaffold that fails under its own gates looks like a broken template.
- **A flipped choice.** One bool off, one other license, one TFM dropped. Confirm exclude and `#else` branches, not only the happy path.
- **Visual Studio.** First create: dialog defaults, extra root `.slnx`, post-action readme, picker icon (pack vs `.template.config/icon.png`). Later create into the same folder, if that is a scenario: **None**, no restamp of `README`.
- **Second CLI create** into the same `--output`, if combo is a scenario: omit Init → exit 0 and a sibling unit; pass Init again → exit 73. `--force` is overwrite, not a pass.
- **The nupkg itself.** Unzip or `dotnet new` from the packed file and check `.gitignore` / `.editorconfig` are present (`NoDefaultExcludes`). `bin` / `obj` / `.vs` from an accidental stub open should not be inside.

CLI and Visual Studio disagree in ways that are easy to love once you have seen them once: empty multi-choice, `|` vs spaces, persistence, the extra `.slnx`. Testing one host is finishing half the generator.

### A sequence that tends to work

1. Get the stub green with the defaults you intend to stamp — including sample types that survive those defaults.
2. Add identity, `sourceName`, `tags.type`, `preferNameDirectory`.
3. Bind the host before defaults that differ by host.
4. Add parameters for jobs. Optional trees → exclude. Optional lines → hash-if for that extension (register it in the same change). Shared seeds → extra sources; exclude the seed folder from the default source.
5. Folder-install for CLI iteration. Pack-install (IDE closed) before trusting the New Project dialog.
6. Walk the created tree as above. If units can be added later, do the second create.

Done means a person can open what you emitted and start, not that `template.json` parses.
