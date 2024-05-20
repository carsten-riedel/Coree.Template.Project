######################################################################################
Log-Block -Stage "Build" -Section "Restore" -Task "Restoreing nuget packages."

if ($null -ne $dotnet_restore_param)
{
    Execute-Command "dotnet restore $topLevelPath/$sourceCodeFolder $dotnet_restore_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Build" -Task "Building the solution."

if ($null -ne $dotnet_build_param)
{
    Execute-Command "dotnet build $topLevelPath/$sourceCodeFolder $dotnet_build_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Test" -Task "Building the solution."

if ($null -ne $dotnet_test_param)
{
    Execute-Command "dotnet test $topLevelPath/$sourceCodeFolder $dotnet_test_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Pack" -Task "Creating a nuget package."

if ($null -ne $dotnet_pack_param)
{
    Execute-Command "dotnet pack $topLevelPath/$sourceCodeFolder $dotnet_pack_param"
}

######################################################################################
Log-Block -Stage "Build" -Section "Docfx" -Task "Creating the docs."

if ($null -ne $docfx_param)
{
    Execute-Command "docfx $docfx_param"
}