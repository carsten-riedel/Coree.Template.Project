<#
.SYNOPSIS
Prepares a clean local Coree.Template.Project installation for testing.

.DESCRIPTION
Removes an existing installation, creates a Debug package, selects the newest
local package output, and installs that package into the active dotnet new
template environment.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$packageId = 'Coree.Template.Project'
$sourceRoot = Join-Path -Path $PSScriptRoot -ChildPath 'src'
$packageOutputDirectory = Join-Path -Path $sourceRoot -ChildPath 'Projects\Coree.Template.Project\bin\Package'
$localPackagePattern = "$packageId.*-local.nupkg"

function Invoke-DotNet {
    param (
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments
    )

    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet $($Arguments -join ' ') failed with exit code $LASTEXITCODE."
    }
}

function Test-TemplatePackageInstalled {
    param (
        [Parameter(Mandatory = $true)]
        [string]$PackageId
    )

    $installedItems = @(& dotnet new uninstall 2>&1)
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to list installed dotnet template packages. Exit code: $LASTEXITCODE."
    }

    return [bool]($installedItems | Where-Object { $_.ToString().Trim() -eq $PackageId } | Select-Object -First 1)
}

function Invoke-TemplatePackageRemoval {
    param (
        [Parameter(Mandatory = $true)]
        [string]$PackageId
    )

    if (Test-TemplatePackageInstalled -PackageId $PackageId) {
        Invoke-DotNet -Arguments @('new', 'uninstall', $PackageId)
    }
    else {
        Write-Output "Template package '$PackageId' is not installed."
    }
}

Invoke-TemplatePackageRemoval -PackageId $packageId

if (-not (Test-Path -LiteralPath $sourceRoot -PathType Container)) {
    throw "Source directory not found: $sourceRoot"
}

Push-Location -LiteralPath $sourceRoot
try {
    Invoke-DotNet -Arguments @('pack', '-c', 'Debug')
}
finally {
    Pop-Location
}

$localPackage = Get-ChildItem -LiteralPath $packageOutputDirectory -Filter $localPackagePattern -File |
    Sort-Object -Property LastWriteTimeUtc -Descending |
    Select-Object -First 1

if ($null -eq $localPackage) {
    throw "No local package matching '$localPackagePattern' was found in '$packageOutputDirectory'."
}

Invoke-DotNet -Arguments @('new', 'install', $localPackage.FullName)

Write-Output "Installed local template package: $($localPackage.FullName)"
Write-Output "The local template package is ready for testing."
