######################################################################################
Log-Block -Stage "Resolving" -Section "Preconditions" -Task "Set fixed values"

$sourceCodeFolder = "src"
$versionMajor = "0"
$versionMinor = "1"
$versionBuild = Get-BaseVersionBuild
$versionRevision = Get-BaseVersionRevision
$fullVersion = "$versionMajor.$versionMinor.$versionBuild.$versionRevision"

######################################################################################
Log-Block -Stage "Resolving" -Section "Preconditions" -Task "Branchnames and Paths."

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

######################################################################################
Log-Block -Stage "Checking" -Section "Preconditions" -Task "Valid branch."

$isValidBranchRootName = @("feature", "develop", "release", "master" , "hotfix" )

if (-not($isValidBranchRootName.ToLower() -contains $branchNameSegment)) {
    Write-Host "No configuration for branches $branchNameSegment. Exiting"
    exit 1
}
else {
    Write-Host "Configuration for branch '$branchNameSegment' will be used."
}

######################################################################################
Log-Block -Stage "Resolving" -Section "Preconditions" -Task "Secrets"

$secretsPath = "$PSScriptRoot/cicd_secrets.ps1"
# Check if the secrets file exists before importing
if (Test-Path $secretsPath) {
    . "$secretsPath"
    Write-Host "Secrets loaded from file."
} else {
    Write-Host "Secrets will be taken from args."
}


######################################################################################
Log-Block -Stage "Checking" -Section "Preconditions" -Task "Variables set."

Ensure-VariableSet -VariableName "`$branchName" -VariableValue "$branchName"
Ensure-VariableSet -VariableName "`$branchNameSegment" -VariableValue "$branchNameSegment"
Ensure-VariableSet -VariableName "`$topLevelPath" -VariableValue "$topLevelPath"
Ensure-VariableSet -VariableName "`$topLevelDirectory" -VariableValue "$topLevelDirectory"
Ensure-VariableSet -VariableName "`$gitRemoteOriginUrl" -VariableValue "$gitRemoteOriginUrl"
Ensure-VariableSet -VariableName "`$gitOwner" -VariableValue "$gitOwner"
Ensure-VariableSet -VariableName "`$gitRepo" -VariableValue "$gitRepo"
Ensure-VariableSet -VariableName "`$sourceCodeFolder" -VariableValue "$sourceCodeFolder"
Ensure-VariableSet -VariableName "`$versionMajor" -VariableValue "$versionMajor"
Ensure-VariableSet -VariableName "`$versionMinor" -VariableValue "$versionMinor"
Ensure-VariableSet -VariableName "`$versionBuild" -VariableValue "$versionBuild"
Ensure-VariableSet -VariableName "`$versionRevision" -VariableValue "$versionRevision"
Ensure-VariableSet -VariableName "`$fullVersion" -VariableValue "$fullVersion"
Ensure-VariableSet -VariableName "`$PAT" -VariableValue "$PAT"
Ensure-VariableSet -VariableName "`$NUGET_PAT" -VariableValue "$NUGET_PAT"
Ensure-VariableSet -VariableName "`$NUGET_TEST_PAT" -VariableValue "$NUGET_TEST_PAT"