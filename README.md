# Coree.Template.Project

**Composable .NET project templates that work just as well for a single project as for a growing multi-project repository.**

Start with one library, analyzer, source generator, MSBuild task, console app, WPF app, WinForms app, PowerShell module, or project template.

When you need another one later, add it to the same repository with another `dotnet new` command.

No migration. No repository restructuring. No framework dependency.

---

## Quick start

Install the templates:

```bash
dotnet new install Coree.Template.Project
```

Create a repository containing a single library:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Core" \
    --InitAllRepoItems
```

That's already a perfectly valid **single-project repository**.

If the project grows later, add another library:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Payments"
```

And another:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Inventory"
```

The same template handles both cases:

**one project today → multiple projects tomorrow**

---

# Why the Multi templates?

Most project templates make an early assumption:

> Is this going to be a single-project repository or a multi-project repository?

Coree's Multi templates avoid that decision.

You can start with:

```text
MyCompany.Core/
└── src/
    ├── prj/
    │   ├── MyCompany.Core/
    │   └── MyCompany.Core.Tests/
    └── sln/
        └── MyCompany.Core/
            └── MyCompany.Core.slnx
```

and stop there forever.

Or, when the repository grows:

```text
MyCompany.Core/
├── README.md
├── LICENSE
├── .gitattributes
├── .gitignore
└── src/
    ├── prj/
    │   ├── MyCompany.Core/
    │   ├── MyCompany.Core.Tests/
    │   ├── MyCompany.Payments/
    │   ├── MyCompany.Payments.Tests/
    │   ├── MyCompany.Inventory/
    │   └── MyCompany.Inventory.Tests/
    └── sln/
        ├── MyCompany.Core/
        │   └── MyCompany.Core.slnx
        ├── MyCompany.Payments/
        │   └── MyCompany.Payments.slnx
        └── MyCompany.Inventory/
            └── MyCompany.Inventory.slnx
```

Each component remains independently buildable, testable, packable, or publishable.

The repository structure does not need to change when the project grows.

---

# Current template families

The **Multi templates are the current generation of Coree.Template.Project**.

Despite the name, they are intended for both:

* single-project repositories
* repositories containing multiple independent projects

Start with one. Add more only when you need them.

| Template                    | Short name                       | Use case                       |
| --------------------------- | -------------------------------- | ------------------------------ |
| Library repository          | `multilibraryrepo-coree`         | NuGet libraries                |
| Analyzer repository         | `multianalyzerrepo-coree`        | Roslyn analyzers               |
| Source-generator repository | `multisourcegeneratorrepo-coree` | Roslyn source generators       |
| MSBuild repository          | `multimsbuildrepo-coree`         | MSBuild task packages          |
| Console repository          | `multiconsolerepo-coree`         | Console apps and .NET tools    |
| WinForms repository         | `multiwinformsrepo-coree`        | Windows Forms applications     |
| WPF repository              | `multiwpfrepo-coree`             | WPF applications               |
| PowerShell repository       | `multipowershellrepo-coree`      | Binary PowerShell modules      |
| Project-template repository | `multiprojecttemplaterepo-coree` | `dotnet new` template packages |

---

# Library repositories

Create one library:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Core" \
    --InitAllRepoItems
```

Build it normally:

```bash
dotnet build
dotnet test
dotnet pack
```

If you never need another library, you're done.

If you do:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Extensions"
```

The additional project is integrated into the existing repository structure without turning the repository into a different kind of project.

---

# Roslyn analyzer repositories

Create a single analyzer:

```bash
dotnet new multianalyzerrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Analyzers" \
    --name "MyCompany.Analyzers.Naming" \
    --InitAllRepoItems
```

That repository can stay exactly like that.

Or add another analyzer later:

```bash
dotnet new multianalyzerrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Analyzers" \
    --name "MyCompany.Analyzers.Performance"
```

Generated analyzer projects include the infrastructure required for analyzer development, testing, packaging, and debugging.

---

# Source-generator repositories

Create one source generator:

```bash
dotnet new multisourcegeneratorrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Generators" \
    --name "MyCompany.Generators.Json" \
    --InitAllRepoItems
```

Add another only if the repository needs one:

```bash
dotnet new multisourcegeneratorrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Generators" \
    --name "MyCompany.Generators.Mapping"
```

The repository is usable from the first generator onward.

---

# MSBuild task repositories

Create a single MSBuild task package:

```bash
dotnet new multimsbuildrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Build" \
    --name "MyCompany.Build.Tasks" \
    --InitAllRepoItems
```

Later, another independently packaged build component can be added to the same repository:

```bash
dotnet new multimsbuildrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Build" \
    --name "MyCompany.Build.Packaging"
```

The generated structure includes the infrastructure needed for MSBuild task development, packaging, testing, and consumer-based debugging.

---

# Console application repositories

Create one console application:

```bash
dotnet new multiconsolerepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Tools" \
    --name "MyCompany.Tools.Sync" \
    --InitAllRepoItems
```

If you later need another tool:

```bash
dotnet new multiconsolerepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Tools" \
    --name "MyCompany.Tools.Migrate"
```

Console projects can support different publishing models, including framework-dependent, framework-included, single-file, and ReadyToRun deployments.

They can also be configured for distribution as .NET tools.

---

# WPF repositories

Create a single WPF application:

```bash
dotnet new multiwpfrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Desktop" \
    --name "MyCompany.Desktop" \
    --InitAllRepoItems
```

Additional applications can be added later without changing the repository structure.

**Windows only.**

---

# WinForms repositories

Create a single WinForms application:

```bash
dotnet new multiwinformsrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.WinForms" \
    --name "MyCompany.WinForms" \
    --InitAllRepoItems
```

The same repository can later contain additional independently publishable WinForms applications.

**Windows only.**

---

# PowerShell module repositories

Create a single binary PowerShell module:

```bash
dotnet new multipowershellrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.PowerShell" \
    --name "MyCompany.PowerShell" \
    --InitAllRepoItems
```

More modules can be added to the same repository later if required.

---

# Project-template repositories

Coree.Template.Project can also generate repositories that contain other `dotnet new` template packages.

Create the first template package:

```bash
dotnet new multiprojecttemplaterepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Templates" \
    --name "MyCompany.Templates.Console" \
    --InitAllRepoItems
```

Add another:

```bash
dotnet new multiprojecttemplaterepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Templates" \
    --name "MyCompany.Templates.Library"
```

Each can be packed and distributed as a normal NuGet .NET Template Package.

```bash
dotnet pack
```

---

# What gets generated?

Coree templates are deliberately more complete than the minimal templates included with the .NET SDK.

Depending on the selected template, generated repositories can provide infrastructure for:

| Capability             | Support               |
| ---------------------- | --------------------- |
| Standard .NET projects | ✓                     |
| `.slnx` solutions      | ✓                     |
| NuGet packaging        | ✓                     |
| Package metadata       | ✓                     |
| Tests                  | ✓                     |
| Code coverage          | ✓                     |
| Local .NET tools       | ✓                     |
| Repository-level files | ✓                     |
| Git versioning         | Optional              |
| Documentation          | Template dependent    |
| Benchmarks             | Template dependent    |
| DebugHost projects     | Template dependent    |
| Public API tracking    | Template dependent    |
| Single-file publishing | Application templates |
| ReadyToRun             | Application templates |

The generated result remains normal .NET/MSBuild infrastructure.

There is no Coree runtime your application must depend on.

---

# Designed to grow

The main idea behind the current templates is simple:

```text
Day 1

Repository
└── Project A
```

does not need a different architecture than:

```text
Day 500

Repository
├── Project A
├── Project B
├── Project C
└── Project D
```

A repository should be able to grow without first having to predict how large it will become.

That is why the Multi templates are also the recommended templates for **single-project repositories**.

---

# Repository initialization

The first invocation can initialize the repository-level files:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyRepository" \
    --name "MyLibrary" \
    --InitAllRepoItems
```

Subsequent invocations add projects to the existing repository:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyRepository" \
    --name "MySecondLibrary"
```

This distinction allows the same template to handle both repository creation and repository expansion.

---

# Template options

All supported options are exposed through the standard .NET template CLI.

For example:

```bash
dotnet new multilibraryrepo-coree --help
```

```bash
dotnet new multianalyzerrepo-coree --help
```

```bash
dotnet new multisourcegeneratorrepo-coree --help
```

```bash
dotnet new multiconsolerepo-coree --help
```

The output of `--help` always reflects the version currently installed on your machine.

---

# Legacy templates

Coree.Template.Project also contains older first-generation templates such as:

```text
classlib-coree
msbuildtasklib-coree
nettool-coree
wpfapp-coree
projecttemplate-coree
```

These templates predate the current composable repository architecture.

They remain available for compatibility during the transition, but the **Multi templates are their intended successors**.

For new projects, prefer the corresponding Multi template.

The goal is for the Multi templates to replace the older templates while still supporting the simple use case that those templates originally covered.

In other words:

```text
old single template
        ↓
new Multi template
        ↓
works for one project
        +
can grow to many projects
```

Existing users therefore do not need to adopt a multi-project workflow just because the replacement template is called `multi...`.

---

# Philosophy

Coree.Template.Project is opinionated about project **infrastructure**, not application architecture.

The templates try to remove repetitive setup around:

* repository structure
* testing
* packaging
* publishing
* debugging
* documentation
* versioning
* common tooling

while leaving the generated .NET projects under your control.

There is no framework lock-in.

There is no custom project format.

There is no requirement to keep using Coree.Template.Project after generation.

The output is yours.

Change it.

Delete parts you do not need.

Add whatever your project requires.

---

# Requirements

Install a supported .NET SDK.

Then install Coree.Template.Project:

```bash
dotnet new install Coree.Template.Project
```

Individual template families may have additional platform requirements.

| Template             | Platform                                                |
| -------------------- | ------------------------------------------------------- |
| Libraries            | Cross-platform                                          |
| Analyzers            | Cross-platform                                          |
| Source generators    | Cross-platform                                          |
| MSBuild tasks        | Cross-platform where supported by the generated project |
| Console applications | Cross-platform                                          |
| Project templates    | Cross-platform                                          |
| WPF                  | Windows                                                 |
| WinForms             | Windows                                                 |
| PowerShell modules   | Depends on generated target/host                        |

For exact requirements and available options:

```bash
dotnet new <template> --help
```

---

# Updating

Install the latest version:

```bash
dotnet new install Coree.Template.Project
```

Remove the package:

```bash
dotnet new uninstall Coree.Template.Project
```

---

# Local development

Clone the repository:

```bash
git clone https://github.com/carsten-riedel/Coree.Template.Project.git
cd Coree.Template.Project
```

The repository contains tooling for building and testing the template package locally.

On Windows:

```powershell
.\Test-LocalTemplatePackage.ps1
```

Development-environment setup and contributor-specific instructions should live separately from the normal user installation flow.

---

# Resources

* [Coree.Template.Project on NuGet](https://www.nuget.org/packages/Coree.Template.Project)
* [Custom templates for `dotnet new`](https://learn.microsoft.com/dotnet/core/tools/custom-templates)
* [Create a project template](https://learn.microsoft.com/dotnet/core/tutorials/cli-templates-create-project-template)
* [Create a template package](https://learn.microsoft.com/dotnet/core/tutorials/cli-templates-create-template-package)
* [dotnet/templating](https://github.com/dotnet/templating)
* [Template samples](https://github.com/dotnet/dotnet-template-samples)

---

# License

Coree.Template.Project is licensed under the [MIT License](LICENSE).

---

## Start with one

```bash
dotnet new install Coree.Template.Project

dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyLibrary" \
    --name "MyLibrary" \
    --InitAllRepoItems
```

If it stays one project, that's fine.

If it grows to twenty, that's fine too.
