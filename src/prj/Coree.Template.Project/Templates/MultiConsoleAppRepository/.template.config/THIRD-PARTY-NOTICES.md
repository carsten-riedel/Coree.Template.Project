# Third-party notices

Libraries this template uses that are not owned or provided by the template author.

## Default content

- [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning) (MIT)
- [coverlet.msbuild](https://github.com/coverlet-coverage/coverlet) (MIT)
- [Microsoft.NET.Test.Sdk](https://github.com/microsoft/vstest) (MIT)
- [MSTest.TestAdapter](https://github.com/microsoft/testfx) / [MSTest.TestFramework](https://github.com/microsoft/testfx) (MIT)

## Optional content

- [ReportGenerator](https://github.com/danielpalme/ReportGenerator) (Apache-2.0)
- [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) (MIT)

## Optional WiX user installer

- [WiX Toolset](https://wixtoolset.org/) (WiX SDK 6.0.2; review the package license terms)

Visual Studio requires the WiX Toolset HeatWave extension to load or build the generated `.wixproj`; the `dotnet` CLI uses the SDK package directly.

## Documentation template (DocShell)

When the documentation template is selected, DocShell specifies these libraries (vendored at bootstrap, not inside the template package):

- Bootstrap 5.3.8 (MIT)
- Bootstrap Icons 1.13.1 (MIT)
- ClipboardJS 2.0.11 (MIT)
- Highlight.js 11.11.1 (BSD-3-Clause)
- Marked 18.0.6 (MIT)
- Mermaid 11.17.2 (MIT)
