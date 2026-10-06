# Publish, Setup und Versionierung: Arbeitsstand

Stand: 2026-10-06. Die Console-Erweiterungen für Setup-Assets, Icons und Installer-Optionen liegen derzeit im **uncommitteten Arbeitsbaum**. Die Versionierungsänderungen sind bereits im Repository. Dieses Dokument trennt implementierten Code, technische Nachweise, ausstehende Praxisprüfung und spätere Ideen.

## Ziel und festgelegtes Modell

`MultiConsoleAppRepository` dient als **Referenzimplementierung**: Es kombiniert App-Publish, optionales NuGet-Tool-Paket und optionalen WiX-Installer. Erst nach seiner Praxisprüfung wird das passende Muster auf WinForms und WPF übertragen. Das Template liefert Strukturen und Platzhalter; anwendungsspezifische Texte und Bilder ersetzt der Nutzer. Daten sollen möglichst an einer Stelle gepflegt werden. Deklarative MSBuild-Items und native WiX-Bindings haben Vorrang; ein Export-Target kommt nur bei einem belegten Bedarf infrage.

| Zweck | Quelle | Mechanismus |
| --- | --- | --- |
| Zusätzliche Dateien neben der veröffentlichten App | App-Projekt: `Properties/PublishAssets/` | `None`-Glob mit Publish-Metadaten und erhaltenen Unterordnern |
| NuGet-Paket-README und Paketbild | App-Projekt: `Properties/NugetMetadata/` | eigene Paketmetadaten; keine Wiederverwendung der Publish-README |
| Lizenzanzeige und eigenes Installer-Icon | Optionales WiX-Projekt: `SetupAssets/` | nur bei `WixUserInstaller` erzeugt; nicht neben der App installiert |
| Icon der Windows-EXE | App-Projekt: `Properties/AppIcon.ico` | native `ApplicationIcon`-Property |
| MSI-Version | Veröffentlichte Haupt-EXE | WiX-Binder `!(bind.fileVersion.MainExecutable)` |

TXT und RTF bleiben getrennte Lizenzformate. Die Standard-RTFs werden bei der Template-Erzeugung aus der gewählten `ProjectLicense`-Vorlage kopiert; ein Konverter und eine spätere automatische Synchronisierung sind nicht vorgesehen. Die Repository-`LICENSE` ist keine verlässliche Installer-Quelle, weil sie nicht in jeder Auswahl erzeugt wird. `Custom` enthält nur einen Copyright-Platzhalter und braucht vor der Verteilung echte Lizenzbedingungen.

## Status nach Template

| Template | Aktueller Stand | Offene Grenze |
| --- | --- | --- |
| **Console** | Referenz im Code: PublishAssets, getrennte NuGet-Metadaten, WiX-Lizenz und Icon, EXE-Icon, Herstellerwahl, EXE-gebundene MSI-Version sowie drei konfigurierbare Installer-Optionen. CLI-Build und `dotnet publish` mit ICE-Validierung gelangen nach der ICE69-Korrektur. | Frisch aus dem neuesten Template generiertes Projekt, Visual Studio, Dialoge sowie Installation/Upgrade/Deinstallation noch praktisch abnehmen. |
| **WinForms** | Bisheriger WiX-Basisinstaller mit Publish-`ProjectReference` und Dateiharvesting. Sichtbarer Name und Installationsordner wurden vereinheitlicht; gemeinsame NerdBank-Versionsvorlagen sind aktualisiert. | Die Console-Referenz für Publish-/Setup-Assets, Metadaten, Icons, UI und optionale Features ist hier noch nicht übertragen. `Package.wxs` verwendet weiterhin die feste MSI-Version `1.0.0.0`. |
| **WPF** | Gleicher Basisstand wie WinForms; eigene WiX-Dateien und aktualisierte NerdBank-Versionsvorlagen. | Dieselben ausstehenden Übertragungen und dieselbe feste MSI-Version `1.0.0.0`. |

Die gemeinsamen Namensänderungen in allen drei `Package.wxs` sind: sichtbarer Name `__SourceName__ (User)`, Installationsordner `__SourceName__` und eine Downgrade-Meldung mit dem konkreten Projektnamen. Die übrige WiX-Logik von WinForms und WPF wurde bislang nicht angepasst.

## Fertig im Code: Console-Referenz

### App-Publish und NuGet

- `Properties/PublishAssets/Readme.md` beginnt mit `# __SourceName__`. Das App-Projekt kopiert `Properties/PublishAssets/**` beim Publish rekursiv neben die App, erhält Unterordner, lässt die Dateien aus normalem Build-Output und Single-File-Bundle heraus und setzt `Pack=false`. Dafür gibt es kein `AfterPublish`-Target.
- `Properties/NugetMetadata/Readme.md` beginnt ebenfalls mit `# __SourceName__`, ist aber ausschließlich Paket-README. Publish-README, Paket-README und Lösungs-/Template-README haben getrennte Zwecke.
- Das Windows-App-Icon liegt unter `Properties/AppIcon.ico` und wird in die EXE eingebettet. Es ist kein loses Publish-Asset. Das NuGet-PNG bleibt ein separates Paketbild.

### Optionaler WiX-Installer

- `ProjectReference` und `PublishForInstaller` liefern den App-Publish für WiX. `MainExecutable` hat eine stabile File-ID, liefert über den Binder die MSI-Version und wird vom übrigen `<Files>`-Harvesting ausgeschlossen; die EXE erscheint dadurch nur einmal im MSI.
- `ProjectLicense` wählt bei `WixUserInstaller` eine von vier RTF-Vorlagen (`MIT`, `BSD3Clause`, `Apache2`, `Custom`) nach `SetupAssets/License.rtf`. Die WiX-UI zeigt sie über `WixUILicenseRtf`. Das WiX-Projekt führt `SetupAssets/**` als `None`-Items, damit die Dateien im Visual-Studio-Projektexplorer sichtbar sein können.
- `SetupAssets/Icon.ico` ist unabhängig vom App-Icon austauschbar. `Icon` und `ARPPRODUCTICON` verwenden es für den Eintrag unter Windows „Installierte Apps“; es wird nicht neben die App kopiert. Beide ICOs starten mit demselben transparenten blauen Platzhalter.
- `Manufacturer` wird **einmal bei der Template-Erzeugung** gewählt: Organisations-Copyright-Inhaber, falls gesetzt, sonst das erforderliche `Author`-Feld. Es ist keine laufende Ableitung aus App-Properties.
- Das erzeugte `.wixproj` enthält je zwei Bool-Werte für **Start nach Installation**, **Desktop-Verknüpfung** und **Benutzer-`PATH`**: `Offer...` steuert, ob die Option angeboten wird; `...SelectedByDefault` steuert die anfängliche Auswahl. Der festgelegte Console-Standard bietet nur Benutzer-`PATH` an und wählt ihn zunächst aus. Start nach Installation und Desktop-Verknüpfung bleiben konfigurierbar, sind aber standardmäßig nicht angeboten und nicht gewählt.
- Desktop-Verknüpfung und Benutzer-`PATH` sind getrennte optionale MSI-Features auf `WixUI_FeatureTree`. Ohne beide verwendet das Paket `WixUI_Minimal`. „Run __SourceName__“ ist eine Checkbox auf dem Abschlussdialog. Die Publish-Dateien gehören zu einer expliziten Pflicht-Feature. `MajorUpgrade` migriert Feature-Zustände standardmäßig, sodass neue Defaults bisherige Benutzerauswahlen normalerweise nicht überschreiben.
- Der Shortcut verweist auf `[INSTALLFOLDER]__SourceName__.exe`. Der frühere Verweis `[#MainExecutable]` löste ICE69 aus, weil Shortcut und EXE in verschiedenen Komponenten/Features liegen. Für den Benutzer-`PATH` ist nur das Installationsverzeichnis und dessen Entfernung bei Deinstallation autorisiert; die tatsächliche Wirkung ist noch praktisch zu prüfen.
- `.wixproj` und `Package.wxs` sind nach Ausgabe, Optionen, Assets, UI und Features gegliedert und mit kurzen Zweckkommentaren versehen.

## Nachgewiesen und noch nicht abgenommen

**Technisch nachgewiesen:**

- Eine frühere CLI-Probe mit `net8.0`, `win-x64`, `FrameworkRequiredSingle`, `WixUserInstaller` und `PackAsNuGetTool=true` veröffentlichte EXE plus Publish-README. Das MSI enthielt die EXE genau einmal und die README. Das NuGet-Paket enthielt die eigene Paket-README im Paket-Root sowie die Publish-README unter `tools/net8.0/any/`. Die EXE-FileVersion war `0.1.0`, das MSI-ProductVersion `0.1.0.0`.
- Ein temporär generiertes Console-WiX-Projekt wurde mit allen Optionen zunächst abgewählt, mit allen Defaults gewählt, mit allen Angeboten aus sowie nur mit `PATH` angeboten und gewählt gebaut. Die MSI-Tabellen zeigten die erwarteten Features, Shortcut und Benutzer-`PATH`-Einträge.
- Ein realer Publish-Versuch des Autors fand ICE69 am alten Shortcut-Ziel. Nach der Korrektur gelangen Build und `dotnet publish` der temporären Console-Kopie **mit aktivierter ICE-Validierung**; die schon zuvor im `.wixproj` ausgeschlossenen ICE38, ICE64 und ICE91 blieben ausgeschlossen. Die MSI-Shortcut-Tabelle enthält den neuen Zielwert. Bei `dotnet publish` erschien lediglich NU1900, weil die NuGet-Audit-Quelle in der Umgebung nicht erreichbar war.
- Der Autor meldete, dass die erste Lizenz-UI im generierten Console-Projekt funktioniert. Das ist kein Nachweis für alle Lizenz- und Installer-Auswahlen.

**Noch praktisch zu prüfen:** Ein **frisch aus dem aktuellen Arbeitsbaum** erzeugtes Console-Projekt wurde nach der ICE69-Korrektur noch nicht vollständig in Visual Studio abgenommen. Insbesondere sind die Sichtbarkeit von `SetupAssets` im Projektexplorer, beide Icons, Lizenz-RTF, die tatsächlichen Dialogzustände, Desktop-Verknüpfung und Benutzer-`PATH` nach Installation und Deinstallation sowie das Upgrade-Verhalten offen. Ein neuer Terminalprozess ist nötig, um einen geänderten Benutzer-`PATH` zu sehen.

## Nächste Schritte: Console als Referenz abschließen

1. Console mit und ohne `WixUserInstaller` frisch erzeugen. Für die Lizenzwahl mindestens eine Standardlizenz und `Custom` kontrollieren: Bei WiX muss die passende `License.rtf` neben `Icon.ico` erscheinen; ohne WiX darf kein `SetupAssets`-Ordner entstehen.
2. Das frisch erzeugte WiX-Projekt in Visual Studio öffnen und `dotnet publish` ohne unterdrückte ICE-Validierung wiederholen. Projektexplorer, App-Icon, Installer-Icon und Lizenzdialog prüfen. Im Installer muss nur Benutzer-`PATH` angeboten und vorausgewählt sein; die beiden anderen Optionen können für eine getrennte Prüfung vorübergehend im `.wixproj` eingeschaltet werden.
3. MSI installieren und deinstallieren: Der ausgewählte Benutzer-`PATH` muss erscheinen und wieder verschwinden. Bei vorübergehend aktiviertem Desktop-Angebot auch die Verknüpfung prüfen. Danach Reparatur und Upgrade einschließlich beibehaltener Feature-Auswahl prüfen.
4. Die Kombination Linux-`RuntimeIdentifier` plus `WixUserInstaller` klar begrenzen oder verständlich fehlschlagen lassen: das EXE-Binding verlangt einen Windows-Publish. Ein leerer Publish-Rest nach Ausschluss der EXE kann zudem ein Empty-Harvest-Warning auslösen; der Standardfall mit externer Publish-README war warnungsfrei.
5. Die MSI-`ProductVersion` mit einer tatsächlich steigenden NerdBank-Git-Höhe prüfen und einen Major-Upgrade-Fall ausführen.

## Danach: WinForms und WPF übertragen

Console ist die technische Referenz, aber nicht jede Console-Voreinstellung passt zu GUI-Apps. Für **beide** Templates einzeln prüfen und übernehmen: PublishAssets, gegebenenfalls App-Icon, Setup-RTF und Setup-Icon, Herstellerwahl, EXE-gebundene MSI-Version mit stabilem File-ID-/Harvesting-Muster und nur passende Installer-Optionen. Für „Run“ und Desktop-Verknüpfung sind GUI-Defaults erst nach der Console-Abnahme festzulegen; Benutzer-`PATH` ist dort kein naheliegender Standard. Danach beide generierten Varianten selbst bauen und in Visual Studio prüfen.

## Versionierung: umgesetzt und gesondert zu prüfen

- Alle neun Repository-Templates besitzen je drei NerdBank-Quellen: `TemplateAssets/Versioning/Repo/version.json`, `TemplateAssets/Versioning/Project/version.json` und `Properties/Build/Nerdbank.version.json` als Repo-Seed. Die `pathFilters` unterscheiden Repo/Seed (`.`) und Project (`..`). Alle 27 Vorlagen verwenden jetzt `version: "0.1"`, `assemblyVersion.precision: "build"` und `nugetPackageVersion.precision: "build"`. Auch das eigene `Coree.Template.Project/Properties/version.json` verwendet `version: "0.9"` und beide `precision`-Werte auf `build`. Die sieben NuGet-Template-Release-Notes beginnen mit `0.1 (1975-01-01)`; historische Release-Notes des Coree-Pakets bleiben erhalten.
- Grund: Windows Installer vergleicht bei `ProductVersion` nur die ersten drei numerischen Felder. Bei einer NerdBank-Basis `0.1.0` wächst die Git-Höhe im vierten Feld; mit `0.1` wächst sie im dritten. `precision` allein verschiebt die Höhe nicht. Die `Off`-Auswahl hat statische Versionen und braucht weiterhin eine bewusste manuelle Erhöhung eines der ersten drei Felder.
- Lokale Console-Probe in einem temporären Git-Repo: Mit `0.1`, Assembly- und NuGet-Präzision `build` ergab der dritte Commit AssemblyVersion `0.1.3.0`, EXE-FileVersion `0.1.3.9725` und NuGet-Paket `0.1.3`. Die vierte Dateiversionsstelle bleibt eine technische Commit-Kennung; die ersten drei Stellen passen zur MSI-Versionsstrategie. Die zuvor erprobten Kombinationen mit `revision` ergaben Paketversionen `0.1.1.60100` bzw. `0.1.2`.
- `Test-LocalTemplatePackage.ps1` baute und installierte das Coree-Template-Paket lokal; dessen `.nupkg` enthielt alle 27 neuen Versionsvorlagen. Der Test lief vor dem Commit der eigenen `version.json`-Änderung, daher zeigte die damalige Coree-Nuspec noch `0.9.0.61274`. Die heutige Paketversion wurde dabei nicht nachgewiesen.
- Noch offen: Repo-Modus und Bibliotheks-/Analyzer-Paketversionen an generierten Projekten prüfen. Bereits generierte Projekte werden nicht automatisch migriert. Für stark benannte .NET-Framework-Bibliotheken kann eine gröbere Assembly-Version wegen Binding nötig sein; das ist eine eigene Kompatibilitätsentscheidung. Eine frühere MSI-Versionsprobe war wegen damals fehlendem WiX-Restore nicht möglich; der erfolgreiche spätere Console-Installer-Publish ersetzt noch keinen Upgrade-Test mit steigender NerdBank-Version.

## Spätere Ideen und bewusste Grenzen

- **Sichtbarer App-Name:** `__SourceName__` dient heute zugleich als Projekt-/Namespace-/NuGet-Name und als sichtbarer App- und Installername. Für Windows „Installierte Apps“ und andere Publish-Oberflächen kann ein eigener Anzeigename sinnvoll sein. Erst Nutzungsstellen, Vorbelegung und stabile technische Identitäten klären, dann einen zusätzlichen Template-Parameter erwägen.
- **Spätere Metadatenänderungen:** `Manufacturer` ist ein einmalig eingesetzter Template-Wert. Nur wenn laufende Synchronisierung aus App-Metadaten wirklich nötig wird, eine gemeinsam importierte `.props` plus `DefineConstants` oder für spät berechnete Werte ein kleines Export-Target prüfen. `ProjectReference` liefert nicht automatisch beliebige ausgewertete App-Properties wie `Authors`, `Company` oder `Description`.
- **Keine automatische Lizenzkonvertierung:** TXT und RTF bleiben absichtlich getrennt. Bei Änderungen nach der Projekterzeugung muss der Nutzer die betroffenen Lizenzdateien selbst pflegen.

## Technische Referenzen

- [WiX `File`](https://docs.firegiant.com/wix/schema/wxs/file/), [`Files`](https://docs.firegiant.com/wix/schema/wxs/files/) und [`Exclude`](https://docs.firegiant.com/wix/schema/wxs/exclude/)
- [WiX `Icon`](https://docs.firegiant.com/wix/schema/wxs/icon/), [Windows Installer `ARPPRODUCTICON`](https://learn.microsoft.com/en-us/windows/win32/msi/arpproducticon)
- [WiX mit MSBuild](https://docs.firegiant.com/wix/tools/msbuild/), [WiX UI](https://docs.firegiant.com/wix/tools/wixext/wixui/), [WiX `Feature`](https://docs.firegiant.com/wix/schema/wxs/feature/)
- [WiX `Shortcut`](https://docs.firegiant.com/wix/schema/wxs/shortcut/), [ICE69](https://learn.microsoft.com/en-us/windows/win32/msi/ice69), [WiX `Environment`](https://docs.firegiant.com/wix/schema/wxs/environment/), [MSI `INSTALLLEVEL`](https://learn.microsoft.com/en-us/windows/win32/msi/installlevel)
- [.NET `CopyToPublishDirectory`](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#copytopublishdirectory), [MSBuild `RecursiveDir`](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-well-known-item-metadata), [Single-File-Ausschluss](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview)
- [Windows Installer `ProductVersion`](https://learn.microsoft.com/en-us/windows/win32/msi/productversion), [NerdBank `version.json`](https://dotnet.github.io/Nerdbank.GitVersioning/docs/versionJson.html), [.NET-Bibliotheksversionierung](https://learn.microsoft.com/en-us/dotnet/standard/library-guidance/versioning)
