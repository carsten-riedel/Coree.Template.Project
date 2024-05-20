function Log-Block {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSUseApprovedVerbs", "")]
    param (
        [string]$Stage,
        [string]$Section,
        [string]$Task
    )
    Write-Output "_"
    Write-Output "==============================================================================================================="
    if (-not [string]::IsNullOrEmpty($Stage)) {
        $output =  "Stage: {0} Section: {1} Task: {2} " -f $Stage.PadRight(15), $Section.PadRight(20), $Task.PadRight(35)
        Write-Output $output
    }
    Write-Output "==============================================================================================================="
}

function Get-NormalizedPathSegments {
    param (
        [string]$InputPath
    )
    
    try {
        # Check if the input is a well-formed absolute URI
        if ([Uri]::IsWellFormedUriString($InputPath, [UriKind]::Absolute)) {
            $uri = New-Object System.Uri $InputPath
            # Normalize by combining the host and the absolute path
            $normalizedPath = $uri.Host + $uri.AbsolutePath
        } else {
            # Treat the input as a local path if it's not a valid absolute URI
            $normalizedPath = $InputPath
        }

        # Normalize and split the path into segments
        $pathSegments = $normalizedPath -split '[\\/]+'  # Split on both forward and back slashes
        
        return $pathSegments  # Return the array of segments
    } catch {
        Write-Host "Error processing the path: $_"
        return $null
    }
}

$global:BaseVersionTicks
$global:BaseVersionTicksPerDay

function Set-BaseVersion {
    
    [void]([Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUserDeclaredVarsMoreThanAssignments','')] $global:BaseVersionTicks = [DateTime]::UtcNow.Ticks - [DateTime]::new(2000, 1, 1, 0, 0, 0, [DateTimeKind]::Utc).Ticks)
    [void]([Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUserDeclaredVarsMoreThanAssignments','')] $global:BaseVersionTicksPerDay = [TimeSpan]::TicksPerDay)
}

Set-BaseVersion

function Get-BaseVersionBuild {
    $assemblyVersionBuild = [Math]::Truncate($global:BaseVersionTicks / $global:BaseVersionTicksPerDay)
    return $assemblyVersionBuild
}

function Get-BaseVersionRevision {

    $assemblyVersionTotalSeconds = [Math]::Truncate($global:BaseVersionTicks / [TimeSpan]::TicksPerSecond)
    $assemblyVersionRemainingSeconds = [Math]::Truncate($assemblyVersionTotalSeconds % 86400)
    $assemblyVersionRevision = [Math]::Truncate($assemblyVersionRemainingSeconds / 2)
    return $assemblyVersionRevision
}

function Test-CommandAvailability {
    param (
        [Parameter(Mandatory = $true)]
        [string]$CommandName
    )

    try {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUserDeclaredVarsMoreThanAssignments','')]
        $commandInfo = Get-Command $CommandName -ErrorAction Stop
        Write-Host "Command is available    : $CommandName"
        return $true
    }
    catch {
        Write-Host "Command is not available: $CommandName"
        return $false
    }        
}

function Test-CommandsAvailabilities {
    param (
        [Parameter(Mandatory = $true)]
        [string[]]$CommandList
    )

    foreach ($item in $CommandList) {
        $testResult = Test-CommandAvailability -CommandName $item
        if (-not $testResult) {
            Write-Host "Stopping checks: '$item' is not available."
            return $false
        }
    }

    return $true
}

function Test-IsGitDirectory {
    param (
        [string]$Path = (Get-Location).Path  # Defaults to the current working directory's path
    )
    
    try {
        # Attempt to get the top-level directory of the git repository for the given path
        $gitTopLevel = git -C $Path rev-parse --show-toplevel

        if ($null -eq $gitTopLevel) {
            Write-Host "The path '$Path' is not within a git repository."
            return $false
        } else {
            Write-Host "The path '$Path' is within a git repository."
            return $true
        }
    } catch {
        Write-Host "Error determining git repository status for the path: $Path"
        return $false
    }
}

function Test-GitRemoteExistence {
    param (
        [string]$Path = (Get-Location).Path
    )
    
    try {
        # Attempt to retrieve the URL of the 'origin' remote of the Git repository
        $gitRemoteOriginUrl = git -C $Path config --get remote.origin.url

        if ($null -eq $gitRemoteOriginUrl) {
            Write-Host "The Git repository does not have a remote named 'origin'."
            return $false
        } else {
            Write-Host "The Git repository has a remote named 'origin'."
            return $true
        }
    } catch {
        Write-Host "Error determining the existence of a remote for the Git repository."
        return $false
    }
}

function Test-UrlRootDomainMatch {
    param (
        [string]$Url,
        [string]$ExpectedRootDomain
    )

    try {
        # Convert the string URL to a URI object
        $uri = [System.Uri]$Url

        # Extract the domain part of the URI
        $actualDomain = $uri.Host

        # Perform a case-insensitive 'contains' check
        if ($actualDomain.IndexOf($ExpectedRootDomain, [StringComparison]::OrdinalIgnoreCase) -ne -1) {
            return $true
        } else {
            return $false
        }
    } catch {
        Write-Host "Processing error for URL '$Url': $_"
        return $false
    }
}

function Test-GitHubRemoteExistence {
    param (
        [string]$Path = (Get-Location).Path  # Defaults to the current working directory's path
    )
    
    try {
        # Attempt to retrieve the URL of the 'origin' remote of the Git repository
        $gitRemoteOriginUrl = git -C $Path config --get remote.origin.url

        # Check if the remote URL is from GitHub
        $result = Test-UrlRootDomainMatch -Url $gitRemoteOriginUrl -ExpectedRootDomain "github.com"

        if ($result) {
            Write-Host "The Git remote URL '$gitRemoteOriginUrl' contains the expected value 'github.com'."
            return $true
        } else {
            Write-Host "The Git remote URL '$gitRemoteOriginUrl' does not contain the expected value 'github.com'."
            return $false
        }
    } catch {
        Write-Host "Error determining the remote configuration of the Git repository."
        return $false
    }
}

function Get-GitBranchName {
    param (
        [string]$Path = (Get-Location).Path  # Defaults to the current working directory's path
    )
    try {
        # Attempt to get the current Git branch name
        $branch = git -C $Path rev-parse --abbrev-ref HEAD
        if ($branch) {
            return $branch
        } else {
            return $null
        }
    } catch {
        return $null
    }
}

function Get-GitTopLevelPath {
    param (
        [string]$Path = (Get-Location).Path  # Defaults to the current working directory's path
    )
    try {
        # Attempt to get the current Git branch name
        $topLevelPath = git -C $Path rev-parse --show-toplevel
        if ($topLevelPath) {
            return $topLevelPath
        } else {
            return $null
        }
    } catch {
        return $null
    }
}

function Get-GitRemoteOriginUrl {
    param (
        [string]$Path = (Get-Location).Path  # Defaults to the current working directory's path
    )
    try {
        # Attempt to get the current Git branch name
        $gitRemoteOriginUrl = git -C $Path config --get remote.origin.url
        if ($gitRemoteOriginUrl) {
            return $gitRemoteOriginUrl
        } else {
            return $null
        }
    } catch {
        return $null
    }
}



<# 
.SYNOPSIS
Executes a specified command and checks if the exit code is one of the expected codes.

.DESCRIPTION
The function executes a command using Invoke-Expression. It throws an error if the exit code of the command is not within the specified expected codes.

.PARAMETER Command
The command string to be executed.

.PARAMETER ExpectedExitCodes
An array of integers specifying acceptable exit codes. Defaults to 0.

.EXAMPLE
Execute-Command "ping 8.8.8.8"
This example executes the ping command and checks if the exit code is 0.

.EXAMPLE
Execute-Command "ping 8.8.8.8" -ExpectedExitCodes 0,1
This example executes the ping command and checks if the exit code is either 0 or 1.
#>
function Execute-Command {
    [CmdletBinding()]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSUseApprovedVerbs", "")]
    param (
        [string]$Command,
        [int[]]$ExpectedExitCodes = @(0)
    )


    # Output the command being executed for transparency
    Write-Output "Executing command: $Command"

    # Execute the command using Invoke-Expression
    Invoke-Expression -Command $Command

    # Check if the exit code is not in the expected array
    if (-not $ExpectedExitCodes.Contains($LASTEXITCODE)) {
        throw "Unexpected exit code: $($LASTEXITCODE). Expected: $($ExpectedExitCodes -join ', ')"
    }
    else {
        Write-Output "Expected  exit code: $($LASTEXITCODE)"
    }
}



function Ensure-VariableSet {
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSUseApprovedVerbs", "")]
    param (
        [Parameter(Mandatory = $true)]
        [string]$VariableName,
        
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$VariableValue
    )
    process {
        if ([string]::IsNullOrEmpty($VariableValue)) {
            $output = "VariableName: {0} is not set." -f $VariableName.PadRight(30)
            Write-Output $output
            throw "$output";
        }
        else {
            Write-Output ("VariableName: {0} is set." -f $VariableName.PadRight(30))
        }
    }
}

function Find-SpecialSubfolders {
    [CmdletBinding()]
    param (
        [string]$Path,  # The root path where the search will begin
        [string[]]$FolderNames = @('bin', 'obj')  # Default folders to search for and skip recursion
    )

    # ArrayList to hold the results for better performance than a regular array
    $foundDirectories = New-Object System.Collections.ArrayList

    # Internal function to handle recursion manually
    function Search-Directories {
        param (
            [string]$SearchPath
        )
        
        # Try to get directories in the current path
        try {
            $directories = Get-ChildItem -Path $SearchPath -Directory -Force -ErrorAction Stop
        } catch {
            Write-Host "Skipping inaccessible directory: $SearchPath"
            return  # Skip this directory and return to continue with others
        }

        foreach ($dir in $directories) {
            if ($FolderNames -contains $dir.Name) {
                # Add the directory to the result list
                $null = $foundDirectories.Add($dir.FullName)
                
                # Skip further recursion into directories listed in FolderNames
                continue
            }

            # Recursively search the next level of directories
            Search-Directories -SearchPath $dir.FullName
        }
    }

    try {
        # Ensure the path exists
        if (-Not (Test-Path $Path)) {
            Write-Host "Specified path does not exist."
            return
        }

        # Start the recursive search from the root path
        Search-Directories -SearchPath $Path

        # Return the list of found directories
        return $foundDirectories
    } catch {
        Write-Host "An error occurred: $_"
    }
}

function Remove-FilesAndDirectories {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string]$FolderPath,  # The root path where the cleanup will begin
        [bool]$DumpDeleted = $true
    )

    # Internal function to recursively delete files
    function Delete-Files {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSUseApprovedVerbs", "")]
        param ([string]$Path)
        $files = $null
        try {
            $files = Get-ChildItem -Path $Path -File -Force -ErrorAction Stop
        } catch {
            Write-Host "Unable to access files in: $Path"
        }
        foreach ($file in $files) {
            try {
                $file | Remove-Item -Force -ErrorAction Stop
                if ($DumpDeleted)
                {
                    Write-Host "Deleted file: $($file.FullName)"
                }
            } catch {
                Write-Host "Failed to delete file: $($file.FullName)"
            }
        }
    }

    # Internal function to recursively delete directories
    function Delete-Directories {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSUseApprovedVerbs", "")]
        param ([string]$Path)
        $directories = $null
        try {
            $directories = Get-ChildItem -Path $Path -Directory -Force -ErrorAction Stop | Sort-Object -Property FullName -Descending
        } catch {
            Write-Host  "Unable to access subdirectories in: $Path"
        }
        foreach ($dir in $directories) {
            # Recursive call to delete files in subdirectories first
            Delete-Files -Path $dir.FullName
            Delete-Directories -Path $dir.FullName
            try {
                $dir | Remove-Item -Force -Recurse -ErrorAction Stop
                if ($DumpDeleted)
                {
                    Write-Host "Deleted directory: $($dir.FullName)"
                }
            } catch {
                Write-Host "Failed to delete directory: $($dir.FullName)"
            }
        }
    }

    # Check if the path is a valid directory
    if (-Not (Test-Path $FolderPath -PathType Container)) {
        Write-Host "The provided path is not a valid directory."
        return
    }

    # Start cleanup with file deletion followed by directory cleanup
    try {
        Delete-Files -Path $FolderPath
        Delete-Directories -Path $FolderPath
    } catch {
        Write-Host "An error occurred during the cleanup process: $_"
    }
}

function Copy-Directory {
    param (
        [string]$sourceDir,
        [string]$destinationDir,
        [string[]]$exclusions
    )

    $sourceDirParam = $sourceDir
    $destinationDirParam = $destinationDir

    # Ensure that $sourceDir and $destinationDir are absolute paths
    if (-not [System.IO.Path]::IsPathRooted($sourceDir)) {
        $sourceDir = Join-Path (Get-Location) $sourceDir
    }

    if (-not [System.IO.Path]::IsPathRooted($destinationDir)) {
        $destinationDir = Join-Path (Get-Location) $destinationDir
    }

    # Ensure paths end with a directory separator for consistent behavior
    $sourceDir = [System.IO.Path]::GetFullPath($sourceDir)
    $destinationDir = [System.IO.Path]::GetFullPath($destinationDir)

    # Get all items in the source directory
    $items = Get-ChildItem -Path $sourceDir -Recurse

    foreach ($item in $items) {
        # Check if the item is in an excluded directory
        $excluded = $false
        foreach ($exclusion in $exclusions) {
            if ($item.FullName -like "*\$exclusion*") {
                $excluded = $true
                break
            }
        }

        if (-not $excluded) {
            $relativePath = [System.IO.Path]::GetRelativePath($sourceDir, $item.FullName)
            $targetPath = Join-Path -Path $destinationDir -ChildPath $relativePath


            $relativeSource = Join-Path -Path $sourceDirParam -ChildPath $relativePath
            $relativeDestination = Join-Path -Path $destinationDirParam -ChildPath $relativePath

            if ($item.PSIsContainer) {
                # Create directory if it doesn't exist
                if (-not (Test-Path -Path $targetPath)) {
                    New-Item -ItemType Directory -Path $targetPath
                }
            } else {
                # Copy file
                Copy-Item -Path $item.FullName -Destination $targetPath -Force
                Write-Output "Copyied: $($relativeSource) --> $($relativeDestination)"
            }
        }
    }
}