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

function Remove-LocalPackageArtifacts {
    param (
        [Parameter(Mandatory = $true)]
        [string]$PackageDirectory,
        [Parameter(Mandatory = $true)]
        [string]$PackagePattern
    )

    if (-not (Test-Path -LiteralPath $PackageDirectory -PathType Container)) {
        New-Item -ItemType Directory -Path $PackageDirectory -Force | Out-Null
    }

    Get-ChildItem -LiteralPath $PackageDirectory -Filter $PackagePattern -File |
        Remove-Item -Force
}

if (Get-Process -Name 'devenv' -ErrorAction SilentlyContinue) {
    throw 'Close all Visual Studio instances before running this script so its template cache cannot retain an older package.'
}

Invoke-TemplatePackageRemoval -PackageId $packageId
Remove-LocalPackageArtifacts -PackageDirectory $packageOutputDirectory -PackagePattern $localPackagePattern

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

$localPackages = @(Get-ChildItem -LiteralPath $packageOutputDirectory -Filter $localPackagePattern -File)

if ($localPackages.Count -ne 1) {
    throw "Expected exactly one local package in '$packageOutputDirectory', found $($localPackages.Count)."
}

$localPackage = $localPackages[0]

Invoke-DotNet -Arguments @('new', 'install', $localPackage.FullName, '--force')

if (-not (Test-TemplatePackageInstalled -PackageId $packageId)) {
    throw "Template package '$packageId' was not registered after installation."
}

Write-Output "Installed local template package: $($localPackage.FullName)"
Write-Output "The local template package is ready for testing."
