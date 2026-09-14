# AI-supported release checkpoint

Once, before the first publish. Work through this list yourself or with an assistant, using the repository as the source of truth. This is not `AGENTS.md` and not standing agent rules.

Walk the whole repository, including console apps added after the first `dotnet new`. When every item below is true, delete this file.

1. The root `README.md` describes this repository as it actually is (capabilities, not leftover template layout). If the root README was never added, skip this item.
2. Every `src/prj/*/Properties/NugetAssets/Readme.md` describes that app's public surface. Empty or placeholder NuGet readmes are not done.
3. Every `src/prj/*/Properties/NugetAssets/ReleaseNotes.txt` matches the first release, not scaffold text.
4. Each console app `.csproj` `<Description>` is an empty CDATA block at generate (not a template parameter). Fill it before first pack from that app's public surface; CDATA keeps multiline gallery text. An assistant can draft the blurb from the code. Tags, project URL, and repository URL: match the product, or leave empty only when that emptiness is intentional.
5. NuGet license metadata matches what you ship: SPDX `PackageLicenseExpression` for MIT / BSD 3-Clause / Apache 2.0, or the packed `Properties/NugetAssets/License.txt` when the license is custom (copyright-only until you add grant terms). If a repository-root `LICENSE` is present, it matches that same license. Replace the icon only if the default asset must not ship.
6. Public surface has no leftover template samples (`Class1`, tests that do not assert the product, copyright lines that still lie).
7. Package version is a conscious first publish (including whether `0.1` and a prerelease suffix are still correct).
<!--#if (PlaceSolution == "BesideLibrary") -->
8. Solution notes in `src/prj/*/Readme.md` match the beside-library layout (`.slnx` next to the console app csproj, no `src/sln/` tree) and any host-specific leftover instructions.
<!--#else -->
8. Solution notes under `src/sln/*/Readme.md` match where each `.slnx` actually is (`SlnFolder` or `RepoRoot`) and any host-specific leftover instructions. That folder is for solution-level or cross-project files; delete it only when the `.slnx` is not there and you do not need the notes.
<!--#endif -->
9. `dotnet publish` of each console app succeeds. `dotnet pack` is optional (`IsPackable` is false until you turn it on); if you pack, the NuGet readme is inside the `.nupkg`.
10. If `DocShell.html` is present, bootstrap that documentation root and then write a short real site. Infer the kind of documentation from the location; do not mix them. If none of these files exist, skip this item.
    - `src/prj/<name>/Properties/NugetAssets/docs/DocShell.html` is **package** documentation for that app (install, public surface, pack/consume). Follow the template's initial bootstrap, then replace the minimal `index.html` with a short package guide from the actual code. Keep later apps' package docs in their own `Properties/NugetAssets/docs` tree.
    - `docs/DocShell.html` at the repository root is **repository** documentation (how this multi-console-app repo is composed, how to add another app, layout). Bootstrap that tree independently. Do not copy one app's API into the repository site.
11. This file is deleted. Later chats should read the apps and the real docs, not this checkpoint.
