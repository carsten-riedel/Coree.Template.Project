# Template release checkpoint

Once, before the first package publish. This is not `AGENTS.md` and not standing agent rules.

Walk the whole repository, including libraries added after the first `dotnet new`. When every item below is true, delete this file.

1. The root `README.md` describes this repository as it actually is (capabilities, not leftover template layout). If the root README was never added, skip this item.
2. Every packable library `src/prj/*/NugetAssets/Readme.md` describes that library's public surface. Empty or placeholder NuGet readmes are not done.
3. Every `src/prj/*/NugetAssets/ReleaseNotes.txt` matches the first release, not scaffold text.
4. Packable `.csproj` metadata that will ship (description, tags, project URL, repository URL) matches the product. Leave a field empty only when that emptiness is intentional.
5. License text in each `NugetAssets/License.txt` is the license you mean to ship. Replace the icon only if the default asset must not ship.
6. Public surface has no leftover template samples (`Class1`, tests that do not assert the product, copyright lines that still lie).
7. Package version is a conscious first publish (including whether `0.1` and a prerelease suffix are still correct).
8. Solution notes under `src/sln/*/Readme.md` do not contradict the layout you kept (`.slnx` under `src/sln/{name}/` vs repository root, host-specific leftover instructions).
9. `dotnet pack` of each packable library succeeds, and the NuGet readme is inside the `.nupkg`.
10. This file is deleted. Later chats should read the libraries and the real package docs, not this checkpoint.
