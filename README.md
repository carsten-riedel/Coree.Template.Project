# Coree.Template.Project

![brand](https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/assets/images/brand.png)

Welcome to the Coree.Template.Project repository! This project offers a comprehensive suite of templates for Visual Studio and dotnet CLI, designed to streamline the creation of various .NET projects. From MSBuild tasks to class libraries and WPF applications, this repository serves as a one-stop resource for developers looking to enhance their .NET development workflow.

## Preparation (Prerequisites)
  
### Windows Setup

Normal install and download procedure.
  1. [Download/Install dotnet SDK](https://dotnet.microsoft.com/en-us/download)
  2. [Download/Install Visual Studio Code](https://code.visualstudio.com/)
  3. [Download/Install Powershell](https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-windows)
  4. [Download/Install git](https://git-scm.com/download/win)

#### User-Space Installation of .NET and PowerShell Core on Windows.

To install dotnet and powershell core from cmd in Windows:
  1. Open the Command Prompt.
  2. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 8.0"` to install the .NET 8.0 SDK.
  3. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 9.0"` to install the .NET 9.0 SDK.
  4. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 10.0"` to install the .NET 10.0 SDK.
  5. Execute: `powershell -NoProfile -ExecutionPolicy Unrestricted -Command "& {[Environment]::SetEnvironmentVariable('DOTNET_ROOT', \"$env:localappdata\Microsoft\dotnet\", 'User')}"` to set the environment variables.
  6. Execute: `powershell -NoProfile -ExecutionPolicy Unrestricted -Command "& {[Environment]::SetEnvironmentVariable('PATH', \"$($env:path);$env:localappdata\Microsoft\dotnet\", 'User')}"`  to set the environment variables.
  7. Execute: `SET "DOTNET_ROOT=%localappdata%\Microsoft\dotnet" & SET "PATH=%PATH%;%localappdata%\Microsoft\dotnet"`  to set the current session environment variables.
  8. Execute: `dotnet tool install --global Powershell --no-cache` 

### WSL Setup

WSL (Windows Subsystem for Linux) enables running Linux environments on Windows. This section covers its setup, essential for cross-platform development.

To set up WSL on Windows:
  1. Open the Command Prompt or PowerShell as an **administrator**.
  2. Execute: `wsl --install --no-distribution`.
  3. Restart your computer when prompted.

To install a wsl image e.g Ubuntu on Windows:
  1. Open the Command Prompt or PowerShell.
  2. Execute: `wsl --update` to update your wsl to the latest version.
  3. Execute: `wsl --set-default-version 2` to set the WSL version to WSL2.
  4. Execute: `wsl --list --online` to list all online availible wsl image versions.
  5. Execute: `wsl --install Ubuntu-24.04 --web-download` to install a online version as webdownload in the case the store is blocked.
  6. Enter your username. If you get an error use lowercase and numbers only.
  7. Enter your password.
  8. Confirm your password.

To uninstall a wsl image e.g Ubuntu on Windows:
  1. Open the Command Prompt or PowerShell.
  2. Execute: `wsl --list` to see a list of your local installed wsl images.
  3. Execute: `wsl --unregister Ubuntu-24.04` to remove a installed wsl image.

### Setting up .NET, PowerShell, and Visual Studio Code on Ubuntu

To install dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute:  `sudo apt-get update && sudo apt-get -y upgrade ` to upgrade the linux distrobution to the latest state.
  3. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0` to install the .NET 8.0 SDK.
  4. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 9.0` to install the .NET 9.0 SDK.
  5. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 10.0` to install the .NET 10.0 SDK.
  6. Execute:  `export DOTNET_ROOT=$HOME/.dotnet ; export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools ; echo 'export DOTNET_ROOT=$HOME/.dotnet' >> $HOME/.bashrc && echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> $HOME/.bashrc` to add the dotnet root and path to the environment and shell startup.
  7. Execute:  `dotnet tool install --global PowerShell` if you want to use Powershell Core.
  8. Execute:  `sudo wget --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868 && sudo apt install -y ./code.deb && rm -f ./code.deb` to install Visual Studio code.
  9. Execute:  `export DONT_PROMPT_WSL_INSTALL=1 ; echo 'export DONT_PROMPT_WSL_INSTALL=1' >> $HOME/.bashrc ; mkdir -p "$HOME/source/repos" ; mkdir -p "$HOME/source/packages"` to get rid of the Visual Studio code prompt, and to create some default directories.
 10. Optional:  Start visual studio code, Execute: `code`
 11. Optional:  In the case visual studio code flickers shutdown wsl inside windows command prompt and start wsl again. Execute: `wsl --shutdown & wsl export DONT_PROMPT_WSL_INSTALL=1 ; code`

To uninstall dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute: `sudo apt-get remove -y code` to uninstall Visual Studio Code.
  3. Execute: `dotnet tool uninstall --global PowerShell` to uninstall Powershell.
  4. Manual:  Remove entries $HOME/.bashrc and delete the .dotnet folder.

# Install/Uninstall the templates
The commands below demonstrate how to install or uninstall the templates, primarily designed for .NET with Visual Studio 2022 or later in mind. Remember, template definitions might include specific limitations like conditional settings (true/false).
```
dotnet new install Coree.Template.Project
dotnet new uninstall Coree.Template.Project
```

### Local package integration test

On Windows, run the parameterless repository-root PowerShell script to remove an existing global installation, create a fresh Debug package, select the newest local package output, and install it ready for testing:

```powershell
.\Test-LocalTemplatePackage.ps1
```

The script performs cleanup before every build using the package ID without a version:

```powershell
dotnet new uninstall Coree.Template.Project
```

The package contains the following templates:
  1. [.NET MSBuild Task library](#Net-MSBuild-Task-library)
  2. [.NET Class library](#Net-class-library)
  3. [.NET multi-library repository](#net-multi-library-repository)
  4. [.NET multi-analyzer repository](#net-multi-analyzer-repository)
  5. [.NET multi-source-generator repository](#net-multi-source-generator-repository)
  6. [.NET multi-msbuild repository](#net-multi-msbuild-repository)
  7. [.NET multi-console repository](#net-multi-console-repository)
  8. [.NET multi-winforms repository](#net-multi-winforms-repository)
  9. [.NET multi-wpf repository](#net-multi-wpf-repository)
  10. [.NET multi-PowerShell module repository](#net-multi-powershell-module-repository)
  11. [.NET multi-project-template repository](#net-multi-project-template-repository)
  12. [.NET Tool](#Net-Tool)
  13. [.NET Wpf](#Net-Wpf-Windows-only)
  14. [.NET Project Template](#Net-Project-Template)

#### Hint:
For testing packages created using these templates, consider setting up a local NuGet test repository. If you're looking to utilize locally built packages, simply establish a NuGet file repository.
In the packages dir `PackageSpecs.props` you can add `<LocalPackagesDir>$(userprofile)\source\packages</LocalPackagesDir>`

Then add a local package source.

Linux/WSL (Sample useage):
```
mkdir -p "$HOME/source/packages" ; dotnet nuget add source "$HOME/source/packages" --name "SourcePackages"
```

Windows cmd (Sample useage):
```
mkdir "%userprofile%\source\packages" & dotnet nuget add source "%userprofile%\source\packages" --name "SourcePackages"
```

To remove the local source
```
dotnet nuget remove source "SourcePackages"
```

## .NET MSBuild Task library
This template provides a foundation for building a .NET Standard compatible MSBuild task library, essential for tasks like build automation. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

General use:
```
dotnet new msbuildtasklib-coree --PackageAuthor Me
```
**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyMSBuildTask" ; cd "source/repos/MyMSBuildTask" ; dotnet new msbuildtasklib-coree --PackageAuthor Me --name "MyMSBuildTask" --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyMSBuildTask" & cd "source\repos\MyMSBuildTask" & dotnet new msbuildtasklib-coree --PackageAuthor Me --name "MyMSBuildTask" --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

**Enhance the TestScript.msbuild in the MSTest project to test your integration.**

## .NET Class library
This template provides a foundation for building a .NET compatible library. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

The test project is the primary source of functionality in this template. To ensure it meets your specific project needs, you should tailor and integrate it accordingly. I pre-configured most variables to minimize the effort required in delving deeply into each documentation, streamlining your setup process.
If you keep the default setting, you can remove the test project without any impact.

**Features:**

- **MSTest Project**: Integrated for robust functionality testing.
- **Coverlet Code Coverage**: Measures code coverage to ensure comprehensive testing.
- **ReportGenerator**: Generates detailed reports on code coverage.
- **BenchmarkDotNet**: Included for performance benchmarking.
- **Docfx**: Facilitates documentation generation from source code and Markdown files.
- **NLog**: Implemented for basic logging, enhancing debugging and monitoring capabilities.
- **Global.json & dotnet-tools.json**: Specify SDK versions and local tool dependencies for a consistent development environment.
- **Solution Items**: Organized for better management of global solution-related files.
- **Nuget-license overview**: Uses the dotnet-project-licenses tool to document licenses used.

General use:
```
dotnet new classlib-coree --PackageAuthor Me
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyClassLib" ; cd "source/repos/MyClassLib" ; dotnet new classlib-coree --PackageAuthor Me --name "MyClassLib" --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyClassLib" & cd "source\repos\MyClassLib" & dotnet new classlib-coree --PackageAuthor Me --name "MyClassLib" --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

## .NET multi-library repository

Create and grow a repository-like structure containing one or more independently packable .NET class libraries using repeatable `dotnet new` calls.

**Ready-to-run baseline**

- **Projects:** packable class library and MSTest project.
- **Quality:** Coverlet coverage by default; `CoverletAndReport` adds ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Debugging:** no separate DebugHost; the library is exercised through tests and benchmarks.
- **Versioning:** optional Nerdbank.GitVersioning, default per-library `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional libraries whenever you need them.

Instead of deciding the complete structure up front, `multilibraryrepo-coree` lets you compose it incrementally:

- create the shared repository layout with the first library;
- add more libraries later using the same template;
- keep every library in a predictable `src/prj` / `src/sln` structure;
- package each library independently;
- use the same workflow interactively, from PowerShell, or from automation.

Each library keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`) so CI can `dotnet pack` / `dotnet publish` against that solution without seeing sibling `.slnx` files in one directory. `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the packable project under `src/prj/{name}/` (not tests or benchmark). One solution may still contain several projects (library, tests, optional benchmark); how much you put in one `.slnx` depends on the pipeline. Splitting by library removes the usual “which solution?” limits.

**Initialize the layout once. Compose as many libraries as you need.**

General use:

```powershell
dotnet new multilibraryrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Core" --name "MyCompany.Core" --InitAllRepoItems
dotnet new multilibraryrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Core" --name "MyCompany.Payments"
dotnet new multilibraryrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Core" --name "MyCompany.Inventory"
```

The first call creates the shared directory layout and initializes the optional repository-level files. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, `TEMPLATE-AI-RELEASE-CHECKPOINT.md`, and `ChoosingPackageBoundaries.md`. Nerdbank defaults to **Project**: `version.json` under each library's `Properties/` folder. Later calls use the same `--output` directory and omit `--InitAllRepoItems`.

One shared repository-root `version.json` instead: `--NerdbankGitVersioning Repo` on each call (the root file is written on the first create only). `--NerdbankGitVersioning Off` keeps `VersionPrefix` in the library project.

Public API tracking is a separate opt-in on each library (`--PublicApiAnalyzers`). It is not part of `--InitAllRepoItems`. The first build writes `Properties/PublicAPI` baseline files if they are missing.

Offline documentation is a separate opt-in (`--DocumentationTemplate`). `Package` seeds `NugetAssets/docs/DocShell.html` on each library. `Repository` seeds repo-root `docs/` on a first create only.

Because the output location and library name are separate arguments, the same composition model works naturally from a script:

```powershell
$repo = "./MyCompany.Core"
$names = @("MyCompany.Core", "MyCompany.Payments", "MyCompany.Inventory", "MyCompany.Reporting")

for ($i = 0; $i -lt $names.Count; $i++) {
    $arguments = @("new", "multilibraryrepo-coree", "--Author", "Carsten Riedel", "--output", $repo, "--name", $names[$i])
    if ($i -eq 0) { $arguments += "--InitAllRepoItems" }
    dotnet @arguments
}
```

After creating `MyCompany.Core`, `MyCompany.Payments`, and `MyCompany.Inventory`, the directory tree looks roughly like this:

```text
MyCompany.Core/
├── README.md
├── LICENSE
├── .gitattributes
├── .gitignore
├── TEMPLATE-AI-RELEASE-CHECKPOINT.md
├── ChoosingPackageBoundaries.md
└── src/
    ├── prj/
    │   ├── MyCompany.Core/
    │   │   ├── Build/
    │   │   ├── NugetAssets/
    │   │   ├── Properties/
    │   │   ├── Class1.cs
    │   │   └── MyCompany.Core.csproj
    │   ├── MyCompany.Core.Tests/
    │   │   └── MyCompany.Core.Tests.csproj
    │   ├── MyCompany.Payments/
    │   │   └── MyCompany.Payments.csproj
    │   ├── MyCompany.Payments.Tests/
    │   │   └── MyCompany.Payments.Tests.csproj
    │   ├── MyCompany.Inventory/
    │   │   └── MyCompany.Inventory.csproj
    │   └── MyCompany.Inventory.Tests/
    │       └── MyCompany.Inventory.Tests.csproj
    └── sln/
        ├── MyCompany.Core/
        │   ├── MyCompany.Core.slnx
        │   └── Readme.md
        ├── MyCompany.Payments/
        │   ├── MyCompany.Payments.slnx
        │   └── Readme.md
        └── MyCompany.Inventory/
            ├── MyCompany.Inventory.slnx
            └── Readme.md
```

The top-level directory is shared. Each additional `dotnet new` call contributes another library-specific project, test project, and solution area. Each library remains its own independently buildable and packable unit while sharing the same repository-like structure.

You do not need a different template for a single-library layout and a multi-library layout. Start with one, add another when you need it, or generate the complete set from a script.

## .NET multi-analyzer repository

Create and grow a repository-like structure containing one or more independently packable Roslyn analyzer NuGet packages using repeatable `dotnet new` calls. This is not `sourcegenerator-coree` (source generators).

**Ready-to-run baseline**

- **Package:** `netstandard2.0` analyzer packed under `analyzers/dotnet/cs`.
- **Quality:** Roslyn analyzer tests, Coverlet coverage by default, and optional ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Debugging:** separate Console DebugHost for compiler execution and Visual Studio's Roslyn Component profile.
- **Versioning:** optional Nerdbank.GitVersioning, default per-analyzer `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional analyzer packages whenever you need them.

Each package targets `netstandard2.0` and packs the assembly under `analyzers/dotnet/cs` (`DevelopmentDependency`). Tests use Microsoft.CodeAnalysis.CSharp.Analyzer.Testing. Each create adds a DebugHost console so Visual Studio can F5 the analyzer via `DebugRoslynComponent`.

The scaffold ships two sample diagnostics you replace with your own rules. **EMD001** reports an em dash (U+2014); **TSQ001** reports typographic quotation marks. Both scan C# syntax trees and, when include globs are set, additional files. Consumers set `EmDashAnalyzerSeverity` / `SmartQuotesAnalyzerSeverity` (`warning`, `error`, `message`, or `off`) and `EmDashAnalyzerIncludes` / `EmDashAnalyzerExcludes` (and the SmartQuotes pair): semicolon-separated globs relative to the consuming project (`*.txt;*.csproj` by default; empty includes skip additional files; `**/*.txt` is recursive). DebugHost is the compile target: ASCII `"1-2"` / `"hello"` stay clean; `"1—2"` and `"“hello”"` plus `SampleTypography.txt` and the host `.csproj` demonstrate the hits.

Each analyzer keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`) so CI can `dotnet test` / `dotnet pack` against that solution. `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the packable analyzer under `src/prj/{name}/` (not tests, DebugHost, or benchmark). DebugHost, tests, and the optional benchmark share one TFM (`.NET 10` by default, `--DebugHostTargetFramework`); the packable analyzer itself is always `netstandard2.0`. Visual Studio F5 needs the **.NET Compiler Platform SDK** component: set the analyzer project as startup, choose the Roslyn Component profile, then F5 (not the DebugHost console).

**Initialize the layout once. Compose as many analyzer packages as you need.**

General use:

```powershell
dotnet new multianalyzerrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Analyzers" --name "MyCompany.Analyzers.Naming" --InitAllRepoItems
dotnet new multianalyzerrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Analyzers" --name "MyCompany.Analyzers.Performance"
```

`--Author` is required. The first call creates the shared directory layout. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Nerdbank defaults to **Project**: `version.json` under each analyzer's `Properties/` folder. Later calls use the same `--output` and omit `--InitAllRepoItems`.

One shared repository-root `version.json` instead: `--NerdbankGitVersioning Repo` on each call (the root file is written on the first create only). `--NerdbankGitVersioning Off` keeps `VersionPrefix` in the analyzer project.

Public API tracking is a separate opt-in on each package (`--PublicApiAnalyzers`). It is not part of `--InitAllRepoItems`. The first build writes `Properties/PublicAPI` baseline files if they are missing.

Offline documentation is a separate opt-in (`--DocumentationTemplate`). `Package` seeds `NugetAssets/docs/DocShell.html` on each package. `Repository` seeds repo-root `docs/` on a first create only.

Because the output location and package name are separate arguments, the same composition model works from a script:

```powershell
$repo = "./MyCompany.Analyzers"
$names = @("MyCompany.Analyzers.Naming", "MyCompany.Analyzers.Performance")

for ($i = 0; $i -lt $names.Count; $i++) {
    $arguments = @("new", "multianalyzerrepo-coree", "--Author", "Carsten Riedel", "--output", $repo, "--name", $names[$i])
    if ($i -eq 0) { $arguments += "--InitAllRepoItems" }
    dotnet @arguments
}
```

After creating `MyCompany.Analyzers.Naming` and `MyCompany.Analyzers.Performance`, the directory tree looks roughly like this:

```text
MyCompany.Analyzers/
├── README.md
├── LICENSE
├── .gitattributes
├── .gitignore
├── TEMPLATE-AI-RELEASE-CHECKPOINT.md
└── src/
    ├── prj/
    │   ├── MyCompany.Analyzers.Naming/
    │   │   ├── Build/
    │   │   ├── NugetAssets/
    │   │   ├── Properties/
    │   │   ├── EmDashAnalyzer.cs
    │   │   ├── SmartQuotesAnalyzer.cs
    │   │   └── MyCompany.Analyzers.Naming.csproj
    │   ├── MyCompany.Analyzers.Naming.Tests/
    │   │   └── MyCompany.Analyzers.Naming.Tests.csproj
    │   ├── MyCompany.Analyzers.Naming.DebugHost/
    │   │   ├── Program.cs
    │   │   ├── SampleTypography.txt
    │   │   └── MyCompany.Analyzers.Naming.DebugHost.csproj
    │   ├── MyCompany.Analyzers.Performance/
    │   │   └── MyCompany.Analyzers.Performance.csproj
    │   ├── MyCompany.Analyzers.Performance.Tests/
    │   │   └── MyCompany.Analyzers.Performance.Tests.csproj
    │   └── MyCompany.Analyzers.Performance.DebugHost/
    │       └── MyCompany.Analyzers.Performance.DebugHost.csproj
    └── sln/
        ├── MyCompany.Analyzers.Naming/
        │   ├── MyCompany.Analyzers.Naming.slnx
        │   └── Readme.md
        └── MyCompany.Analyzers.Performance/
            ├── MyCompany.Analyzers.Performance.slnx
            └── Readme.md
```

The top-level directory is shared. Each additional `dotnet new` call contributes another analyzer project, tests, DebugHost, and solution area. Each package remains independently buildable and packable.

You do not need a different template for a single-analyzer layout and a multi-analyzer layout. Start with one, add another when you need it, or generate the complete set from a script.

## .NET multi-source-generator repository

Create and grow a repository containing one or more independently packable Roslyn source-generator packages.

**Ready-to-run baseline**

- **Package:** `netstandard2.0` generator packed under `analyzers/dotnet/cs`.
- **Quality:** tests compile and execute generated code; Coverlet coverage is enabled by default and ReportGenerator output is optional.
- **Performance:** optional BenchmarkDotNet console project.
- **Debugging:** Console DebugHost and Visual Studio Roslyn Component setup are included.
- **Versioning:** optional Nerdbank.GitVersioning, default per-generator `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

The scaffold uses annotated C# classes as the source of truth. `JsonSupportGenerator` is incremental and generates `TypeNameJson.Serialize` / `Deserialize` helpers. Tests compile and execute the generated API; the DebugHost consumes it and acts as the Visual Studio `DebugRoslynComponent` target. The generator performs no project-file writes or dynamic compilation.

```powershell
dotnet new multisourcegeneratorrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Generators" --name "MyCompany.Generators.Json" --InitAllRepoItems
dotnet new multisourcegeneratorrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Generators" --name "MyCompany.Generators.Mapping"
```

The first call adds the shared repository files; later calls use the same output and omit `--InitAllRepoItems`. DebugHost, tests, and the optional benchmark share the selected runnable TFM (`.NET 10` by default), while the generator package remains `netstandard2.0`.

## .NET multi-msbuild repository

Create and grow a repository-like structure containing one or more independently packable MSBuild task NuGet packages using repeatable `dotnet new` calls. This does **not** replace `msbuildtasklib-coree`.

**Ready-to-run baseline**

- **Package:** `netstandard2.0` MSBuild task packed with its `build/` props and targets.
- **Quality:** unit and integration tests, Coverlet coverage by default, and optional ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Debugging:** separate MSBuild consumer DebugHost for Visual Studio F5.
- **Versioning:** optional Nerdbank.GitVersioning, default per-task `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional task packages whenever you need them.

Each package targets `netstandard2.0` and packs the assembly under `tasks/netstandard2.0` (`DevelopmentDependency`) with auto-imported `build/` props (`UsingTask`) and targets (sample consumer `CoreCompile` extension). Tests are automated unit and integration tests. DebugHost is a separate MSBuild consumer for Visual Studio F5 (not a Roslyn `DebugRoslynComponent` host).

**Initialize the layout once. Compose as many task packages as you need.**

General use:

```powershell
dotnet new multimsbuildrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Tasks" --name "MyCompany.Tasks.Add" --InitAllRepoItems
dotnet new multimsbuildrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Tasks" --name "MyCompany.Tasks.Pack"
```

`--Author` is required. The first call creates the shared directory layout. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Later calls use the same `--output` and omit `--InitAllRepoItems`.

Each task package keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`). `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the packable task under `src/prj/{name}/` (not tests, DebugHost, or benchmark). DebugHost, tests, and the optional benchmark share one TFM (`.NET 10` by default, `--DebugHostTargetFramework`); the packable task itself is always `netstandard2.0`. Visual Studio F5: set DebugHost as startup, choose the Executable profile, then F5.

## .NET multi-console repository

Create and grow a repository-like structure containing one or more independently publishable .NET console apps using repeatable `dotnet new` calls.

**Ready-to-run baseline**

- **Projects:** console app and MSTest project.
- **Quality:** Coverlet coverage by default; `CoverletAndReport` adds ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Publish/debug:** selectable single-file and ReadyToRun profiles; the app itself is the Visual Studio/CLI debug target, so no separate DebugHost is needed.
- **Versioning:** optional Nerdbank.GitVersioning, default per-app `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional apps whenever you need them.

Instead of deciding the complete structure up front, `multiconsolerepo-coree` lets you compose it incrementally:

- create the shared repository layout with the first app;
- add more apps later using the same template;
- keep every app in a predictable `src/prj` / `src/sln` structure;
- publish each app independently; additional pack as a NuGet tool is on by default;
- use the same workflow interactively, from PowerShell, or from automation.

Each app keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`) so CI can `dotnet publish` against that solution without seeing sibling `.slnx` files in one directory. `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the console project under `src/prj/{name}/` (not tests or benchmark). One solution may still contain several projects (console app, tests, optional benchmark); how much you put in one `.slnx` depends on the pipeline. Splitting by app removes the usual “which solution?” limits.

**Initialize the layout once. Compose as many console apps as you need.**

General use:

```powershell
dotnet new multiconsolerepo-coree --Author "Carsten Riedel" --output "./MyCompany.Cli" --name "MyCompany.Cli" --InitAllRepoItems
dotnet new multiconsolerepo-coree --Author "Carsten Riedel" --output "./MyCompany.Cli" --name "MyCompany.Cli.Sync"
dotnet new multiconsolerepo-coree --Author "Carsten Riedel" --output "./MyCompany.Cli" --name "MyCompany.Cli.Migrate"
```

`--Author` is required. The first call creates the shared directory layout and initializes the optional repository-level files. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Nerdbank defaults to **Project**: `version.json` under each app's `Properties/` folder. Later calls use the same `--output` directory and omit `--InitAllRepoItems`.

Default publish is **Windows x64** (`win-x64`) with **Framework-included, single-file, compressed, ReadyToRun**. That RID and profile apply only while publishing; restore and build stay framework-dependent. Other RIDs: `linux-arm64` (Raspberry Pi 64-bit OS), `linux-x64` (Debian/Ubuntu/CentOS/Fedora), `linux-musl-arm64` (Alpine/Docker ARM64). Other profiles: `FrameworkRequired`, `FrameworkRequiredSingle`, `FrameworkIncluded`, `FrameworkIncludedSingle`.

`--PackAsNuGetTool` is on by default: in addition to publish, `IsPackable` / `PackAsTool`, `ToolCommandName` (project name, overridable in the csproj), and NuGet assets under `Properties/NugetAssets/`. `--PackAsNuGetTool false` is publish-only. This is not `--DotNetToolManifest` (empty local `dotnet-tools.json`).

One shared repository-root `version.json` instead: `--NerdbankGitVersioning Repo` on each call (the root file is written on the first create only). `--NerdbankGitVersioning Off` keeps `VersionPrefix` in the console project.

Offline documentation is a separate opt-in (`--DocumentationTemplate`). `Package` seeds `Properties/NugetAssets/docs/DocShell.html` only when `--PackAsNuGetTool` is on. `Repository` seeds repo-root `docs/` on a first create only.

Because the output location and app name are separate arguments, the same composition model works from a script:

```powershell
$repo = "./MyCompany.Cli"
$names = @("MyCompany.Cli", "MyCompany.Cli.Sync", "MyCompany.Cli.Migrate")

for ($i = 0; $i -lt $names.Count; $i++) {
    $arguments = @("new", "multiconsolerepo-coree", "--Author", "Carsten Riedel", "--output", $repo, "--name", $names[$i])
    if ($i -eq 0) { $arguments += "--InitAllRepoItems" }
    dotnet @arguments
}
```

After creating `MyCompany.Cli`, `MyCompany.Cli.Sync`, and `MyCompany.Cli.Migrate`, the directory tree looks roughly like this:

```text
MyCompany.Cli/
├── README.md
├── LICENSE
├── .gitattributes
├── .gitignore
├── TEMPLATE-AI-RELEASE-CHECKPOINT.md
└── src/
    ├── prj/
    │   ├── MyCompany.Cli/
    │   │   ├── Properties/
    │   │   │   └── NugetAssets/
    │   │   ├── Program.cs
    │   │   └── MyCompany.Cli.csproj
    │   ├── MyCompany.Cli.Tests/
    │   │   └── MyCompany.Cli.Tests.csproj
    │   ├── MyCompany.Cli.Sync/
    │   │   └── MyCompany.Cli.Sync.csproj
    │   ├── MyCompany.Cli.Sync.Tests/
    │   │   └── MyCompany.Cli.Sync.Tests.csproj
    │   ├── MyCompany.Cli.Migrate/
    │   │   └── MyCompany.Cli.Migrate.csproj
    │   └── MyCompany.Cli.Migrate.Tests/
    │       └── MyCompany.Cli.Migrate.Tests.csproj
    └── sln/
        ├── MyCompany.Cli/
        │   ├── MyCompany.Cli.slnx
        │   └── Readme.md
        ├── MyCompany.Cli.Sync/
        │   ├── MyCompany.Cli.Sync.slnx
        │   └── Readme.md
        └── MyCompany.Cli.Migrate/
            ├── MyCompany.Cli.Migrate.slnx
            └── Readme.md
```

The top-level directory is shared. Each additional `dotnet new` call contributes another app-specific project, test project, and solution area. Each app remains its own independently buildable and publishable unit while sharing the same repository-like structure.

You do not need a different template for a single-app layout and a multi-app layout. Start with one, add another when you need it, or generate the complete set from a script.

## .NET multi-winforms repository

Same combo and init system as the multi-console repository, for independently publishable C# Windows Forms apps (Windows only). This does **not** replace `winforms-coree` or `winformsdi-coree`.

**Ready-to-run baseline**

- **Projects:** WinForms app and MSTest project.
- **Quality:** Coverlet coverage by default; `CoverletAndReport` adds ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Publish/debug:** selectable single-file and ReadyToRun profiles; the app itself is the Visual Studio debug target, so no separate DebugHost is needed.
- **Versioning:** optional Nerdbank.GitVersioning, default per-app `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional apps whenever you need them.

```powershell
dotnet new multiwinformsrepo-coree --Author "Carsten Riedel" --output "./MyCompany.WinForms" --name "MyCompany.WinForms" --InitAllRepoItems
dotnet new multiwinformsrepo-coree --Author "Carsten Riedel" --output "./MyCompany.WinForms" --name "MyCompany.WinForms.Settings"
```

`--Author` is required. The first call creates the shared directory layout. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Later calls use the same `--output` and omit `--InitAllRepoItems`.

Each app keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`). `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the app under `src/prj/{name}/`.

## .NET multi-wpf repository

Same combo and init system as the multi-console repository, for independently publishable C# WPF apps (Windows only). This does **not** replace `wpfapp-coree`.

**Ready-to-run baseline**

- **Projects:** WPF app and MSTest project.
- **Quality:** Coverlet coverage by default; `CoverletAndReport` adds ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Publish/debug:** selectable single-file and ReadyToRun profiles; the app itself is the Visual Studio debug target, so no separate DebugHost is needed.
- **Versioning:** optional Nerdbank.GitVersioning, default per-app `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional apps whenever you need them.

```powershell
dotnet new multiwpfrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Wpf" --name "MyCompany.Wpf" --InitAllRepoItems
dotnet new multiwpfrepo-coree --Author "Carsten Riedel" --output "./MyCompany.Wpf" --name "MyCompany.Wpf.Editor"
```

`--Author` is required. The first call creates the shared directory layout. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Later calls use the same `--output` and omit `--InitAllRepoItems`.

Each app keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`). `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the app under `src/prj/{name}/`.

## .NET multi-PowerShell module repository

Same combo and init system as the multi-library repository, for independently packable binary PowerShell modules. This does **not** replace `powershelllib-coree`.

**Ready-to-run baseline**

- **Package:** binary module with host-based TFM selection (default Windows PowerShell 5.1 on .NET Framework 4.6.2 plus PowerShell 7.4+ on .NET 8).
- **Quality:** real import tests in `powershell.exe` / `pwsh.exe`, Coverlet coverage by default, and optional ReportGenerator HTML/Markdown output.
- **Performance:** optional BenchmarkDotNet console project.
- **Debugging/publish:** separate DebugHost F5 launcher; `dotnet pack -c Release` stages a PowerShell Gallery package under `bin/Pack/`.
- **Versioning:** optional Nerdbank.GitVersioning, default per-module `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.

Initialize the shared repository layout once, then add additional modules whenever you need them.

```powershell
dotnet new multipowershellrepo-coree --Author "Carsten Riedel" --output "./MyCompany.PowerShell" --name "MyCompany.PowerShell" --InitAllRepoItems
dotnet new multipowershellrepo-coree --Author "Carsten Riedel" --output "./MyCompany.PowerShell" --name "MyCompany.PowerShell.Admin"
```

`--Author` is required. The first call creates the shared directory layout. `--InitAllRepoItems` adds `README.md`, `LICENSE`, `.gitattributes`, `.gitignore`, and `TEMPLATE-AI-RELEASE-CHECKPOINT.md`. Later calls use the same `--output` and omit `--InitAllRepoItems`.

Each module keeps a `src/sln/{name}/` notes folder. By default the `.slnx` lives there too (`--PlaceSolution SlnFolder`). `--PlaceSolution RepoRoot` writes it at the repository root; `--PlaceSolution BesideCsproj` writes it next to the module under `src/prj/{name}/`. Manifest, loader, license, readme, and release notes live in `Properties/NugetAssets/`.

## .NET multi-project-template repository

Create and grow a repository containing one or more independently packable .NET project-template NuGet packages. `multiprojecttemplaterepo-coree` is the authoring/meta-template in this family.

**Baseline**

- **Package:** packable `netstandard2.0` project-template package with `Templates/` content and NuGet assets.
- **Sample:** minimal nested console template (`template.json`, `.csproj`, and `Program.cs`) as a starting point for authoring.
- **Repository:** repeatable multi-package layout with selectable solution placement.
- **Versioning:** optional Nerdbank.GitVersioning, default per-package `Properties/version.json`; `Repo` selects one repository-root file, `Off` keeps manual `VersionPrefix` values.
- **Scope:** intentionally no application DebugHost, benchmark, or test project; the nested sample is itself the template example.

Initialize the shared repository layout once, then add additional project-template packages whenever you need them.

```powershell
dotnet new multiprojecttemplaterepo-coree --Author "Carsten Riedel" --output "./MyCompany.Templates" --name "MyCompany.Templates.Console" --InitAllRepoItems
dotnet new multiprojecttemplaterepo-coree --Author "Carsten Riedel" --output "./MyCompany.Templates" --name "MyCompany.Templates.Web"
```

`--Author` is required. The first call creates the shared repository files; later calls use the same `--output` directory and omit `--InitAllRepoItems`. `--PlaceSolution SlnFolder` is the default; `RepoRoot` and `BesideCsproj` remain available for the solution location. The packable project-template package keeps its nested templates under `Templates/` and can be packed with `dotnet pack`.

## .NET Tool
This template provides a foundation for building a .NET commandline tool. The template is structured to support NuGet packaging and publishing, requiring an author's specification and ToolCommandName for these purposes.
The final command will be the ToolCommandName. The Packagename is the Project and ToolCommandName

General use:
```
dotnet new nettool-coree --PackageAuthor Me --ToolCommandName helloworld
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyNetTool" ; cd "source/repos/MyNetTool" ; dotnet new nettool-coree --PackageAuthor Me --name "MyNetTool" --ToolCommandName helloworld --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyNetTool" & cd "source\repos\MyNetTool" & dotnet new nettool-coree --PackageAuthor Me --name "MyNetTool"  --ToolCommandName helloworld --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```
Assuming you've already copied your package to a NuGet source, whether it's local or remote, you can easily install it using the .NET Core CLI. Specifically, if you're created a prerelease version of a tool called MyNetTool.helloworld, you can install it globally on your machine with the following command.

```
dotnet tool install -g MyNetTool --prerelease
#REM OR use a temporary package location
dotnet tool install -g MyNetTool --prerelease --add-source "%userprofile%\MyNetTool\MyNetTool\bin\Pack"
```

Use:
```
C:\Users\MainUser>helloworld
Hello, World!
```

## .NET Wpf (Windows only)
This template serves as a base for developing WPF applications, optimized primarily for .NET publishing as an executable. It includes five distinct configurations tailored for the dotnet publish process. Enhanced with basic visual improvements, the template utilizes the MaterialDesignThemes.MahApps package. You can tailor the App.xaml to your requirements, particularly by altering color keys like `<Color x:Key="Primary200">#76bddf</Color>`, to suit your design preferences.

General use:
```
dotnet new wpfapp-coree --Author Me
```
Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyWpfApp" & cd "MyWpfApp" & dotnet new wpfapp-coree --Author Me --force & dotnet publish & cd /D %userprofile%
```

Publish settings.
1) Framework-required. (Purpose: Installer)
2) Framework-required, Single-file. (Purpose: Copy smallest) <- DEFAULT
3) Framework-included. (Purpose: Installer)
4) Framework-included, Single-file. (Purpose: Copy large)
5) Framework-included, Single-file, Compressed. (Purpose: Copy medium)

The Single-file selection will have the following settings.
```
<PublishSingleFile>true</PublishSingleFile>
<PublishReadyToRun>true</PublishReadyToRun>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
```

### Screenshots
![image](https://user-images.githubusercontent.com/97656046/282236852-7540e391-0554-4f44-bf5b-eb5d6d0ea984.png)

![image](https://user-images.githubusercontent.com/97656046/282236828-18061fa0-4b72-49f3-baa6-8e49648a7991.png)

## .NET Project Template
Indeed, it may sound a bit perplexing at first – a project template for creating project templates. However, it's quite straightforward. For a project to generate templates, these must be situated within the packages/root/content folder. Additionally, a default class library is incorporated, complete with the provided names, to kickstart your project template creation. For more detailed guidance, refer to the educational section at the beginning of this readme.

General use:
```
dotnet new projecttemplate-coree --PackageAuthor "me" --SampleTemplateName "Testing template" --SampleTemplateShortName "test"
```

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyProjTemplate" ; cd "source/repos/MyProjTemplate" ; dotnet new projecttemplate-coree --PackageAuthor Me --name "MyProjTemplate" --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --output "src" --force ; git init ; cd "src" ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```
Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyProjTemplate" & cd "source\repos\MyProjTemplate" & dotnet new projecttemplate-coree --PackageAuthor Me --name "MyProjTemplate" --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --output "src" --force  & git init & cd "src" & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

## Educational

For more information and resources for templating:
  - [MS Learn: Custom templates for dotnet new](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
  - [MS Learn: Create an item template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-item-template)
  - [MS Learn: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
  - [MS Learn: Create a template package](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-template-package?pivots=dotnet-6-0)
  - [Github: Templating Wiki](https://github.com/dotnet/templating/wiki)
  - [Github: template.json reference](https://github.com/dotnet/templating/wiki/Reference-for-template.json)
  - [Github: Net template samples](https://github.com/dotnet/templating/tree/main/dotnet-template-samples)
  - [Schema: template.json](https://json.schemastore.org/template)
  - [Microsoft Project Repository .net winforms templates](https://github.com/dotnet/winforms/tree/main/pkg/Microsoft.Dotnet.WinForms.ProjectTemplates/content/WinFormsApplication-CSharp)

For more information and resources for msbuild tasks:
  - [MS Learn: Create a custom task for code generation](https://learn.microsoft.com/en-us/visualstudio/msbuild/tutorial-custom-task-code-generation)
  - [MS Learn: Task writing](https://learn.microsoft.com/en-us/visualstudio/msbuild/task-writing)
  - [Github: MSBuild custom task sample](https://github.com/dotnet/samples/tree/main/msbuild/custom-task-code-generation)

For more information and resources for source generators and analyzers:
  - [MS Learn: Source generators overview](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
  - [Github: Source generators cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md)
  - [MS Learn: Analyzer package path format](https://learn.microsoft.com/en-us/nuget/guides/analyzers-conventions#analyzers-path-format)

For more information and resources for .NET tools and NuGet packages:
  - [MS Learn: How to create a .NET tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create)
  - [MS Learn: NuGet package authoring best practices](https://learn.microsoft.com/en-us/nuget/create-packages/package-authoring-best-practices)
