######################################################################################
Log-Block -Stage "Prebuild envars" -Section "Prepare" -Task "Secrets"

$secretsPath = "$PSScriptRoot/cicd_secrets.ps1"
# Check if the secrets file exists before importing
if (Test-Path $secretsPath) {
    . "$secretsPath"
    Write-Host "Secrets loaded from file."
} else {
    Write-Host "Secrets will be taken from args."
}

######################################################################################
Log-Block -Stage "Prebuild envars" -Section "Prepare" -Task "Set fixed values"

$sourceCodeFolder = "src"
$versionMajor = "0"
$versionMinor = "1"
$versionBuild = Get-BaseVersionBuild
$versionRevision = Get-BaseVersionRevision
$fullVersion = "$versionMajor.$versionMinor.$versionBuild.$versionRevision"

######################################################################################
Log-Block -Stage "Prebuild envars" -Section "Prepare" -Task "Resolving branchnames and paths."

$branchName = Get-GitBranchName
$branchNameSegment = @(Get-NormalizedPathSegments -InputPath $branchName)[0].ToLower()
 
$topLevelPath = Get-GitTopLevelPath
$topLevelDirectory = @(Get-NormalizedPathSegments -InputPath $topLevelPath)[-1]

$gitRemoteOriginUrl = Get-GitRemoteOriginUrl
$gitOwner = @(Get-NormalizedPathSegments -InputPath $gitRemoteOriginUrl)[1]
$gitRepo = @(Get-NormalizedPathSegments -InputPath $gitRemoteOriginUrl)[2]

Write-Host "branchName is         : $branchName"
Write-Host "branchNameSegment is  : $branchNameSegment"
Write-Host "topLevelPath is       : $topLevelPath"
Write-Host "topLevelDirectory is  : $topLevelDirectory"
Write-Host "gitRemoteOriginUrl is : $gitRemoteOriginUrl"
Write-Host "gitOwner is           : $gitOwner"
Write-Host "gitRepo is            : $gitRepo"
Write-Host "fullVersion is        : $fullVersion"



