
######################################################################################
Log-Block -Stage "Checking" -Section "Preconditions" -Task "Commands setup."

# Check availability of required commands
$result = Test-CommandsAvailabilities -CommandList @("git", "dotnet", "pwsh" , "curl" , "gh")
if (-not $result) {
    Write-Host "One or more required commands are unavailable. Stopping execution."
    exit 1
}

######################################################################################
Log-Block -Stage "Checking" -Section "Preconditions" -Task "Git setup."

# Verify that the current directory is a Git repository
$result = Test-IsGitDirectory
if (-not $result) {
    Write-Host "The directory is not a Git repository."
    exit 1
}

# Check for the existence of a remote named 'origin' in the current Git repository
$result = Test-GitRemoteExistence
if (-not $result) {
    Write-Host "This Git repository does not have a remote named 'origin'."
    exit 1
}

######################################################################################
Log-Block -Stage "Checking" -Section "Preconditions" -Task "Github setup."

$result = Test-GitHubRemoteExistence
if (-not $result) {
    Write-Host "This Git is not a github repository."
    exit 1
}