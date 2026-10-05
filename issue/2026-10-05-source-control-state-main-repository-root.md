# Resolve scalar SourceControlState from the main repository root

## Problem

`Properties/Build/SourceControlState.targets` currently materializes the SDK's multi-valued `@(SourceRoot)` list into scalar properties.

That can produce values such as:

```text
SourceControlState.SourceControlType=;;git
SourceControlState.Raw.Type=;;git
SourceControlState.Raw.RepositoryUrl=;;<repository URL>
SourceControlState.Raw.RevisionId=;;<revision>
SourceControlState.Raw.BranchName=;;refs/heads/<branch>
SourceControlState.SourceControlRoot=<NuGet root>;<Visual Studio NuGet root>;<main repository root>
```

The empty entries come from `SourceRoot` items that do not carry source-control metadata.

## Important semantic requirement

The purpose of this target is not to expose every source root, subrepository, submodule, package root, or Source Link mapping.

Its scalar `SourceControlState.*` properties should describe the **main repository root relevant to the project**.

The SDK-provided `@(SourceRoot)` list is intentionally broader and may contain:

- NuGet/source-package roots;
- the main repository root;
- nested repositories or Git submodules;
- other source-mapping roots.

Those additional roots must not be concatenated into the scalar state.

The implementation therefore needs to identify the one `SourceRoot` item that represents the main repository and ignore nested/subrepository and unrelated source roots for these scalar properties.

## Root cause

The issue is the direct conversion of a multi-item list into scalar properties, for example:

```xml
<_SourceControlTypeRaw>@(_PrimarySourceControlRoot->'%(SourceControl)')</_SourceControlTypeRaw>
```

MSBuild joins transformed items with semicolons, including empty transformed values. That explains the observed leading `;;`.

Filtering all non-SCM roots fixes the visible example, but a correct final solution also has to ensure that exactly the intended main repository is selected rather than a nested/subrepository root.

## SDK context

The .NET SDK already separates the broader `@(SourceRoot)` item list from scalar repository state such as `SourceRevisionId`, `SourceBranchName`, and `ScmRepositoryUrl`.

The implementation should use that scalar SDK state where appropriate and correlate it with the correct main-repository `SourceRoot`, rather than treating the complete list as one repository.

## Possible interim normalization

A defensive interim measure could strip only leading/trailing empty-list separators from scalar metadata values, for example an outer trim of `;`.

That would turn:

```text
;;git
;;<repository URL>
```

into:

```text
git
<repository URL>
```

However, this is only a containment measure:

- it does not identify the correct main repository;
- it does not fix a `SourceControlRoot` value containing several actual paths;
- it could hide ambiguity if multiple SCM roots are selected.

Therefore outer semicolon trimming may be useful as defensive normalization, but it should not replace correct main-root selection.

## Expected behavior

For a Git-backed project in the main repository:

```text
SourceControlState.HasSourceControl=true
SourceControlState.HasGitSourceControl=true
SourceControlState.SourceControlType=git
SourceControlState.SourceControlRoot=<main repository root>
SourceControlState.SourceControlRevisionId=<main repository revision>
SourceControlState.SourceControlRepositoryUrl=<main repository URL>
SourceControlState.SourceControlBranchName=refs/heads/<main repository branch>
```

No scalar property should contain unrelated NuGet roots, subrepository roots, or artificial leading/trailing semicolon entries.

## Attached proposal for review

The issue folder also contains [`SourceControlState.proposal.targets`](./SourceControlState.proposal.targets), provided as a concrete implementation candidate to evaluate.

It should be treated as a **proposal for testing/review**, not as the accepted fix.

Notable ideas in the proposal that are worth validating:

- filters `@(SourceRoot)` to entries with non-empty `SourceControl` metadata, which removes package-cache roots from the observed `;;git` case;
- prefers the SDK scalar `SourceRevisionId` and `SourceBranchName` values before falling back to root metadata;
- recognizes detached Git HEAD when a revision exists but the branch is empty or reported as `HEAD`;
- clears `SourceControlBranchName` for detached HEAD and exposes `HasDetachedHead`;
- converts `refs/heads/feature/example` to the short branch name `feature/example`;
- derives convenience values for a primary branch segment, remainder, and file/path-safe branch-name variants.
- adds `ResolveDeploymentChannel`, mapping `feature`/`bugfix` to `preview`, `develop` to `development`, `release` to `staging`, and `main`/`master`/`hotfix` to `production`;
- adds `ResolveDeploymentChannelActivation`, keeping `preview` inactive while `development`, `staging`, and `production` are active.
- runs `ResolveSourceControlState` before `GetAssemblyAttributes` as well as `GenerateAssemblyInfo` and `GenerateNuspec`; this should be validated for target ordering, but the behavior is project-neutral.

The proposal still needs explicit verification against the main requirement of this issue: **the selected scalar root must represent the main repository, not merely every `SourceRoot` that has SCM metadata**. Filtering on `SourceControl != ''` alone may still leave more than one repository/root when subrepositories or submodules are present.

Suggested tests for this proposal:

1. normal checked-out local branch, e.g. SDK value `refs/heads/feature/example`;
2. detached HEAD, where the revision remains available while the branch is empty or `HEAD`;
3. package/NuGet source roots plus the main repository root;
4. main repository with nested Git submodule/subrepository roots;
5. more than one SCM-bearing `SourceRoot`, confirming that scalar values never become `git;git` or multi-path values;
6. consistency between `SourceControlRoot`, repository URL, revision, and branch so all describe the same main repository;
7. deployment-channel mapping: `feature/*` and `bugfix/*` → `preview`, `develop` → `development`, `release/*` → `staging`, `main`/`master`/`hotfix/*` → `production`;
8. activation behavior: `preview=false`, `development=true`, `staging=true`, `production=true`;
9. detached HEAD / unnamed-branch behavior does not accidentally activate a deployment channel;
10. `GetAssemblyAttributes` ordering behaves as intended and does not cause duplicate or late source-control-derived assembly metadata.

## Acceptance criteria

- One main repository root is identified explicitly from the SDK source-control information.
- Nested repositories/submodules and unrelated source roots are excluded from scalar `SourceControlState.*` values.
- `SourceControlRoot` contains exactly one physical main-repository root.
- `SourceControlType` is a single provider value such as `git`, not a semicolon-separated list.
- Revision, repository URL, and branch describe the same selected main repository.
- `HasGitSourceControl=true` when that selected main repository uses Git.
- Non-SCM `SourceRoot` items cannot produce leading `;;` artifacts.
- Any defensive `Trim(';')`-style normalization is documented as supplemental, not as the primary repository-selection algorithm.
- Tests cover at least a normal Git repository with package roots and a repository containing nested/subrepository roots.
- The top-level target and the template copies remain consistent.
