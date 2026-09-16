# Coree.Template.Project

**Composable .NET project templates for humans and AI coding agents.**

**Define the boundaries once. Build freely within them.**

Coree.Template.Project creates an opinionated, deterministic project frame around your .NET code: repository layout, project boundaries, testing, packaging, publishing, tooling, and common infrastructure.

The implementation inside that frame remains yours.

Start with one library, analyzer, source generator, MSBuild task, console application, WPF application, WinForms application, PowerShell module, or project template.

If the repository grows later, add another component with the same `dotnet new` command.

No migration.
No repository restructuring.
No framework dependency.

**Deterministic structure. Flexible implementation.**

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

That's already a complete **single-project repository**.

Build it using the normal .NET CLI:

```bash
dotnet build
dotnet test
dotnet pack
```

If you need another library later:

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

# Why define the frame?

Starting a project involves many decisions that are usually unrelated to the actual problem you want to solve.

Where do projects live?

Where do tests live?

How are packages built?

How is publishing configured?

How are repository-wide files organized?

How should another project be added six months later?

Coree.Template.Project turns those recurring decisions into a reusable template.

The template defines the **boundaries**.

Inside those boundaries, you remain free to implement the application however you want.

```text
Template responsibility
│
├── Repository structure
├── Project boundaries
├── Test structure
├── Packaging
├── Publishing
├── Tooling
├── Common metadata
└── Repeatable project creation
        │
        ▼
Your responsibility
│
└── The actual software
```

This is especially useful when working with AI coding agents.

---

# AI-assisted development

A coding agent should not have to reinvent your repository every time it starts a project.

Without a predefined structure, you might ask an agent:

```text
Create a .NET solution with a library, test project,
NuGet packaging, code coverage, versioning,
publishing configuration, repository files,
and a sensible directory structure.
```

The result depends on the prompt, the agent, and the decisions it makes at that moment.

With Coree.Template.Project, the structural part becomes a command:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyProject" \
    --name "MyProject" \
    --InitAllRepoItems
```

Then the agent can work on the actual task:

```text
Implement the domain model and persistence layer
inside the generated repository.
```

The template creates the frame.

The agent works inside it.

This means less context is spent recreating boilerplate and fewer architectural decisions have to be rediscovered for every project.

It also gives different coding agents the same starting point.

Whether the implementation is written by a human, an AI agent, or both, the surrounding project structure remains consistent.

**Prompts can vary. The project boundaries do not have to.**

---

# Designed to grow

The Multi templates do not require a multi-project repository.

They are designed so that a repository containing one component and a repository containing many components follow the same structure.

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

That is a perfectly valid use case.

If the repository grows:

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

the repository does not need to be redesigned.

Each component remains independently buildable, testable, packable, or publishable.

The structure already had room for growth.

---

# Current template families

The **Multi templates are the current generation of Coree.Template.Project**.

Despite the name, they support both:

* single-project repositories
* repositories containing multiple independent projects

Start with one.

Add more only when you need them.

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

If that is all the repository ever needs, you're done.

If another library becomes necessary later:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Core" \
    --name "MyCompany.Extensions"
```

The new component follows the same established repository boundaries.

A human does not have to recreate them.

An AI agent does not have to infer them.

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

Add another later:

```bash
dotnet new multianalyzerrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Analyzers" \
    --name "MyCompany.Analyzers.Performance"
```

Generated analyzer projects provide the surrounding structure for analyzer development, testing, packaging, and debugging.

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

Add another when required:

```bash
dotnet new multisourcegeneratorrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Generators" \
    --name "MyCompany.Generators.Mapping"
```

The repository structure stays consistent as generators are added.

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

Add another component later:

```bash
dotnet new multimsbuildrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Build" \
    --name "MyCompany.Build.Packaging"
```

The generated structure provides a consistent frame for MSBuild task development, packaging, testing, and consumer-based debugging.

---

# Console application repositories

Create one application:

```bash
dotnet new multiconsolerepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Tools" \
    --name "MyCompany.Tools.Sync" \
    --InitAllRepoItems
```

Add another tool later:

```bash
dotnet new multiconsolerepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.Tools" \
    --name "MyCompany.Tools.Migrate"
```

Console projects can support framework-dependent, framework-included, single-file, and ReadyToRun publishing scenarios.

They can also be configured for distribution as .NET tools.

---

# WPF repositories

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

```bash
dotnet new multipowershellrepo-coree \
    --Author "Your Name" \
    --output "./MyCompany.PowerShell" \
    --name "MyCompany.PowerShell" \
    --InitAllRepoItems
```

More modules can be added later while keeping the same repository conventions.

---

# Project-template repositories

Coree.Template.Project can also generate repositories whose output is itself a `dotnet new` template package.

Create the first template:

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

Each generated template package can be packed through the normal .NET toolchain:

```bash
dotnet pack
```

---

# What does the template own?

Coree.Template.Project is intentionally opinionated about the **frame**, not about your application logic.

Depending on the template, that frame can include:

| Area                   | Support               |
| ---------------------- | --------------------- |
| Repository layout      | ✓                     |
| Project boundaries     | ✓                     |
| `.slnx` solutions      | ✓                     |
| Testing structure      | ✓                     |
| NuGet packaging        | ✓                     |
| Package metadata       | ✓                     |
| Code coverage          | ✓                     |
| Repository-level files | ✓                     |
| Local .NET tools       | ✓                     |
| Git versioning         | Optional              |
| Documentation          | Template dependent    |
| Benchmarks             | Template dependent    |
| DebugHost projects     | Template dependent    |
| Public API tracking    | Template dependent    |
| Single-file publishing | Application templates |
| ReadyToRun             | Application templates |

The generated output remains normal .NET and MSBuild infrastructure.

There is no Coree runtime required by the generated application.

---

# What does the template not own?

The template does not decide your:

* domain model
* business logic
* application architecture
* algorithms
* persistence strategy
* UI design
* public API
* coding style beyond the generated frame

Those decisions remain with the developer or coding agent.

This distinction is intentional.

**The template constrains the repetitive structure so that implementation can remain flexible.**

---

# Repository initialization

The first invocation can initialize repository-wide files:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyRepository" \
    --name "MyLibrary" \
    --InitAllRepoItems
```

Later invocations add components to the existing repository:

```bash
dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyRepository" \
    --name "MySecondLibrary"
```

The same operation works whether it is performed manually or by an automated coding agent.

---

# Template options

Every template exposes its options through the standard .NET template CLI.

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

Using `--help` ensures that the available options match the exact version installed on the machine.

This also makes the templates straightforward for tools and coding agents to inspect without requiring custom documentation formats or APIs.

---

# Legacy templates

Coree.Template.Project still contains first-generation templates such as:

```text
classlib-coree
msbuildtasklib-coree
nettool-coree
wpfapp-coree
projecttemplate-coree
```

These predate the current composable repository architecture.

They remain available during the transition, but the **Multi templates are their intended successors**.

For new projects, prefer the corresponding Multi template.

The Multi templates preserve the simple single-project use case while allowing the same repository to grow later.

```text
Single project
     │
     │ same template
     ▼
Single project + another component
     │
     │ same structure
     ▼
Larger repository
```

There is no separate migration from a "single" template to a "multi" template.

---

# Philosophy

A good project template should do more than copy a few files.

It should define the repetitive boundaries once.

Humans should not need to rebuild those boundaries for every repository.

AI coding agents should not need to spend context and reasoning recreating them either.

Coree.Template.Project therefore separates two concerns:

```text
Deterministic frame
        +
Flexible implementation
```

The template establishes the frame.

Humans and AI build within it.

If the project grows, the frame grows with it.

After generation, the repository is still yours.

Change it.

Remove parts you do not need.

Extend it.

Or never invoke Coree.Template.Project again.

There is no framework lock-in.

---

# Requirements

Install a supported .NET SDK.

Then install Coree.Template.Project:

```bash
dotnet new install Coree.Template.Project
```

Individual template families can have additional platform requirements.

| Template             | Platform                       |
| -------------------- | ------------------------------ |
| Libraries            | Cross-platform                 |
| Analyzers            | Cross-platform                 |
| Source generators    | Cross-platform                 |
| MSBuild tasks        | Cross-platform where supported |
| Console applications | Cross-platform                 |
| Project templates    | Cross-platform                 |
| WPF                  | Windows                        |
| WinForms             | Windows                        |
| PowerShell modules   | Depends on target and host     |

For exact template options:

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

On Windows, the repository contains tooling for building and installing the current package locally:

```powershell
.\Test-LocalTemplatePackage.ps1
```

Detailed development-machine setup belongs outside the normal user installation flow.

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

## Start with the frame

```bash
dotnet new install Coree.Template.Project

dotnet new multilibraryrepo-coree \
    --Author "Your Name" \
    --output "./MyLibrary" \
    --name "MyLibrary" \
    --InitAllRepoItems
```

Then build the software inside it.
