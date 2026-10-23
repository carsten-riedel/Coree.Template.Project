$escapedSolutionPath = "$topLevelPath/$solutionFolder".Replace("'", "''")

######################################################################################
Log-Block -Stage "Build" -Section "Restore" -Task "Restoreing nuget packages."

if ($null -ne $dotnet_restore_param)
{
    Execute-Command "dotnet restore '$escapedSolutionPath' $dotnet_restore_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Build" -Task "Building the solution."

if ($null -ne $dotnet_build_param)
{
    Execute-Command "dotnet build '$escapedSolutionPath' $dotnet_build_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Pack" -Task "Creating a nuget package."

if ($null -ne $dotnet_pack_param)
{
    Execute-Command "dotnet pack '$escapedSolutionPath' $dotnet_pack_param"
}
