<#
.SYNOPSIS
Completes the repository layout after Visual Studio creates the solution.
.DESCRIPTION
Close Visual Studio first. Preserves the redundant root solution as a backup,
keeps the complete solution in src, and opens it explicitly with VS 2026.
#>
[CmdletBinding()]
param([switch]$NoOpen)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../..'))
$sourceRoot = Join-Path $repoRoot 'src'
$generatedSolution = Join-Path $sourceRoot 'ClassLibrary.generated.slnx'
$finalSolution = Join-Path $sourceRoot 'ClassLibrary.slnx'

if (Get-Process -Name devenv -ErrorAction SilentlyContinue) {
    throw 'Close Visual Studio before completing the repository layout.'
}

$vsExecutable = $null
if (-not $NoOpen) {
    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
    $vsInstall = & $vswhere -latest -products '*' -version '[18.0,19.0)' -property installationPath
    if ($LASTEXITCODE -ne 0 -or -not $vsInstall) { throw 'Visual Studio 2026 was not found.' }
    $vsExecutable = Join-Path $vsInstall 'Common7/IDE/devenv.exe'
    if (-not (Test-Path -LiteralPath $vsExecutable -PathType Leaf)) { throw 'Visual Studio 2026 executable was not found.' }
}

if ((Test-Path -LiteralPath $generatedSolution) -and (Test-Path -LiteralPath $finalSolution)) {
    throw 'Both generated and final solutions exist in src. Resolve this ambiguity first.'
}
$sourceSolution = if (Test-Path -LiteralPath $generatedSolution) { $generatedSolution } else { $finalSolution }
if (-not (Test-Path -LiteralPath $sourceSolution -PathType Leaf)) { throw 'The expected src solution is missing.' }

function Read-Solution([string]$Path) {
    $document = New-Object System.Xml.XmlDocument
    $document.XmlResolver = $null
    $document.Load($Path)
    if ($document.DocumentElement.Name -ne 'Solution') { throw "Invalid solution: $Path" }
    return ,$document
}

function Get-ProjectPaths($Document, [string]$Directory) {
    foreach ($project in $Document.SelectNodes('//Project')) {
        $path = [System.IO.Path]::GetFullPath((Join-Path $Directory $project.GetAttribute('Path')))
        if (-not $path.StartsWith($sourceRoot + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
            throw "Project is outside src: $path"
        }
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Project does not exist: $path" }
        $path
    }
}

$sourceDocument = Read-Solution $sourceSolution
$sourceProjects = @(Get-ProjectPaths $sourceDocument $sourceRoot | Sort-Object -Unique)
if ($sourceProjects.Count -eq 0) { throw 'The src solution contains no projects.' }
$rootSolutions = @(Get-ChildItem -LiteralPath $repoRoot -File | Where-Object { $_.Extension -in @('.sln', '.slnx') })
if ($rootSolutions.Count -gt 1) { throw 'Multiple root solutions exist. Nothing was changed.' }
if ($rootSolutions.Count -eq 1) {
    $rootSolution = $rootSolutions[0]
    if ($rootSolution.Extension -ne '.slnx') { throw 'The root solution must use SLNX.' }
    $rootDocument = Read-Solution $rootSolution.FullName
    # Only the plain project-only solution created by VS is disposable.
    if ($rootDocument.DocumentElement.Attributes.Count -ne 0) { throw 'The root solution has additional settings; preserve it manually.' }
    foreach ($node in $rootDocument.DocumentElement.ChildNodes) {
        if ($node.NodeType -ne [System.Xml.XmlNodeType]::Element -or $node.Name -ne 'Project' -or $node.Attributes.Count -ne 1 -or -not $node.HasAttribute('Path') -or $node.HasChildNodes) {
            throw 'The root solution has additional content; preserve it manually.'
        }
    }
    $rootProjects = @(Get-ProjectPaths $rootDocument $repoRoot | Sort-Object -Unique)
    if (@(Compare-Object $sourceProjects $rootProjects).Count -ne 0) { throw 'The solutions reference different projects. Nothing was changed.' }
}

if ($sourceSolution -ne $finalSolution) {
    Move-Item -LiteralPath $sourceSolution -Destination $finalSolution
}
if ($rootSolutions.Count -eq 1) {
    $backupDirectory = Join-Path $repoRoot '.vs/ClassLibraryRepo1'
    New-Item -ItemType Directory -Path $backupDirectory -Force | Out-Null
    $backup = Join-Path $backupDirectory ($rootSolutions[0].Name + '.' + [guid]::NewGuid().ToString('N') + '.backup')
    Move-Item -LiteralPath $rootSolutions[0].FullName -Destination $backup
    Write-Output "Preserved the redundant root solution: $backup"
}
Write-Output "Repository solution: $finalSolution"
if (-not $NoOpen) {
    # The user is explicitly running this helper to open an interactive VS window.
    Start-Process -FilePath $vsExecutable -ArgumentList ('"' + $finalSolution + '"')
}
