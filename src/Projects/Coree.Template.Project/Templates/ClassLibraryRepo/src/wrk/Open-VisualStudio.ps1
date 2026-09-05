<#
.SYNOPSIS
Tries to launch the generated solution in VS 2026, then removes this helper.
.DESCRIPTION
This helper exists to meet the repository-layout requirements: generate a single
SLNX solution under src and open it automatically in VS 2026, without a VS
extension or manual correction of the generated layout.

No working alternative using only the built-in VS template configuration and
post-actions was found for these requirements in the investigated VS 2026 host:
- Its solution discovery accepts SLN primary outputs, but ignores SLNX outputs.
- It therefore adds generated projects to its own root solution. Removing the
  .generated suffix and declaring src/*.slnx as a primary output did not fix this.
- Even its recognized-solution branch saves to the host's chosen solution path.
- The Open file post-action opens a document editor; testing confirmed that it
  did not activate the nested SLNX as the current solution.
- The documented Run script post-action has a dotnet CLI implementation, but
  no built-in implementation in the inspected VS template host.

Consequently, dotnet new owns generation and invokes this helper through its
Run script post-action. No outer VS solution is created by that workflow.
vswhere selects VS 2026 explicitly, even when older VS versions are installed.
The helper deletes itself after process launch; it cannot confirm that solution
loading has completed. On launch failure, it remains available for a retry.
.NOTES
Investigated on 2026-09-05 with VS 2026 18.9.2, template-host DLL version
18.9.36.21130, and .NET SDK 10.0.400. This is a finding about the tested host and
constraints, not proof that every possible external automation is impossible.
Reassess this workaround when the VS template host changes.
.LINK
https://github.com/dotnet/templating/wiki/Post-Action-Registry
.LINK
https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.rpccontracts.opendocument.iopendocumentservice.opendocumentasync
#>
[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$solutionPath = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../ClassLibrary.slnx'))

try {
    if (-not (Test-Path -LiteralPath $solutionPath -PathType Leaf)) {
        throw "Solution not found: $solutionPath"
    }
    $installerRoot = ${env:ProgramFiles(x86)}
    if (-not $installerRoot) { throw 'The Visual Studio installer location is unavailable.' }
    $vswhere = Join-Path $installerRoot 'Microsoft Visual Studio/Installer/vswhere.exe'
    if (-not (Test-Path -LiteralPath $vswhere -PathType Leaf)) { throw 'vswhere.exe was not found.' }

    $vsInstall = & $vswhere -latest -products Microsoft.VisualStudio.Product.Community Microsoft.VisualStudio.Product.Professional Microsoft.VisualStudio.Product.Enterprise -version '[18.0,19.0)' -property installationPath
    if ($LASTEXITCODE -ne 0 -or -not $vsInstall) { throw 'Visual Studio 2026 was not found.' }
    $vsExecutable = Join-Path $vsInstall 'Common7/IDE/devenv.exe'
    if (-not (Test-Path -LiteralPath $vsExecutable -PathType Leaf)) { throw 'The Visual Studio 2026 executable was not found.' }

    # An interactive VS window is the explicitly requested result of this helper.
    Start-Process -FilePath $vsExecutable -ArgumentList ('"' + $solutionPath + '"')
}
catch {
    Write-Warning "Could not start Visual Studio 2026: $($_.Exception.Message)"
    Write-Output "The repository is available. Open this solution manually: $solutionPath"
    Write-Output "The launcher was kept for another attempt: $PSCommandPath"
    return
}

Write-Output "Started Visual Studio 2026 with: $solutionPath"
try {
    # Successful process launch does not confirm completion of solution loading.
    Remove-Item -LiteralPath $PSCommandPath
}
catch {
    Write-Warning "Visual Studio was started, but the launcher could not remove itself: $($_.Exception.Message)"
}
