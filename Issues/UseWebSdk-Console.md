# Console: Switch auf Microsoft.NET.Sdk.Web fehlt

Stand: 2026-10-07.

`MultiLibraryRepository` kann das Library-Projekt von `Microsoft.NET.Sdk` auf `Microsoft.NET.Sdk.Web` umstellen. `MultiConsoleAppRepository` hat denselben expliziten SDK-Import, aber keinen Switch.

## Ist

Library (`UseWebSdk`, Standard `false`, CLI `--UseWebSdk`, Visual Studio "Use the ASP.NET Core Web SDK"):

- `src/prj/__SourceName__/__SourceName__.csproj` wählt `Sdk.props` über `<!--#if (UseWebSdk) -->`.
- `Properties/Build/ImportSdkTargets.targets` wählt dasselbe SDK für `Sdk.targets`.
- Tests und Benchmark bleiben bei `Microsoft.NET.Sdk`.

Console schreibt beide Imports fest:

- `<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />` am Anfang der App-csproj.
- `<Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />` in `Properties/Build/ImportSdkTargets.targets`.

Die Console-Maintainer-Notiz sagt ausdrücklich, dass dieses Template `UseWebSdk` nicht verwendet.

## Soll

Denselben Generate-Time-Switch für das Console-App-Projekt: `Sdk.props` und `Sdk.targets` zusammen auf `Microsoft.NET.Sdk.Web`, Standard weiter `Microsoft.NET.Sdk`. Test- und Benchmark-Projekte nicht umstellen. `ImportSdkTargets` bleibt der letzte Import der App-csproj, damit `PublishDefaultFramework` nicht vom SDK überschrieben wird.
