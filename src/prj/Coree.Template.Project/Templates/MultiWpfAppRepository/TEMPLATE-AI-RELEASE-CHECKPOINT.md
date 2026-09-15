# AI-supported release checkpoint

Once, before the first publish. Work through this list yourself or with an assistant, using the repository as the source of truth. This is not `AGENTS.md` and not standing agent rules.

Walk the whole repository, including apps added after the first `dotnet new`. When every item below is true, delete this file.

1. The root `README.md` describes this repository as it actually is (capabilities, not leftover template layout). If the root README was never added, skip this item.
2. Each app `.csproj` `<Description>` is an empty CDATA block at generate (not a template parameter). Fill it when the product has a public surface; CDATA keeps multiline text. An assistant can draft the blurb from the code.
3. Copyright and company on each app match the product (not leftover template holder text). If a repository-root `LICENSE` is present, it matches that same license.
4. Public surface has no leftover template samples (`Program`, tests that do not assert the product, copyright lines that still lie).
5. Version is a conscious first publish (including whether `0.1` and a prerelease suffix are still correct).
<!--#if (PlaceSolution == "BesideCsproj") -->
6. Solution notes in `src/prj/*/Readme.md` match the beside-csproj layout (`.slnx` next to the app csproj, no `src/sln/` tree) and any host-specific leftover instructions.
<!--#else -->
6. Solution notes under `src/sln/*/Readme.md` match where each `.slnx` actually is (`SlnFolder` or `RepoRoot`) and any host-specific leftover instructions. That folder is for solution-level or cross-project files; delete it only when the `.slnx` is not there and you do not need the notes.
<!--#endif -->
7. `dotnet publish` of each app succeeds.
8. If `docs/DocShell.html` is present at the repository root, bootstrap that documentation root and then write a short real **repository** site (how this multi-app repo is composed, how to add another app, layout). Do not copy one app's UI into the repository site. If that file does not exist, skip this item.
9. This file is deleted. Later chats should read the apps and the real docs, not this checkpoint.
