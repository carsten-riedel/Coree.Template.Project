######################################################################################
Log-Block -Stage "Prebuild envars" -Section "Check" -Task "Valid branch names."

$isValidBranchRootName = @("feature", "develop", "release", "master", "main" , "hotfix" )

if (-not($isValidBranchRootName.ToLower() -contains $branchNameSegment)) {
    Write-Host "No configuration for branches $branchNameSegment. Exiting"
    exit 1
}
else {
    Write-Host "Configuration for branch '$branchNameSegment' will be used."
}

######################################################################################
Log-Block -Stage "Prebuild envars" -Section "Check" -Task "Variables are set."

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