######################################################################################
Log-Block -Stage "Resolving" -Section "Branch" -Task "Config values for branches"

# Some variables can be $null or unset indicating a skipping step.

if ($branchNameSegment -ieq "feature") {

    $version = "--property:AssemblyVersion=$fullVersion --property:VersionPrefix=$fullVersion --property:VersionSuffix=$branchNameSegment"

    $dotnet_restore_param = "";
    $dotnet_build_param = "--no-restore --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $dotnet_test_param = "--configuration Release";
    $dotnet_pack_param =  "--force --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $docfx_param = $null

} elseif ($branchNameSegment -ieq "develop") {

    $version = "--property:AssemblyVersion=$fullVersion --property:VersionPrefix=$fullVersion --property:VersionSuffix=$branchNameSegment"

    $dotnet_restore_param = "";
    $dotnet_build_param = "--no-restore --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $dotnet_test_param = "--configuration Release";
    $dotnet_pack_param =  "--force --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $docfx_param = $null

} elseif ($branchNameSegment -ieq "release") {

    $version = "--property:AssemblyVersion=$fullVersion --property:VersionPrefix=$fullVersion"

    $dotnet_restore_param = "";
    $dotnet_build_param = "--no-restore --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $dotnet_test_param = "";
    $dotnet_pack_param =  "--force --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $docfx_param = $null

} elseif ($branchNameSegment -ieq "master") {

    $version = "--property:AssemblyVersion=$fullVersion --property:VersionPrefix=$fullVersion"

    $dotnet_restore_param = "";
    $dotnet_build_param = "--no-restore --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $dotnet_test_param = "--configuration Release";
    $dotnet_pack_param =  "--force --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $docfx_param = "$topLevelPath/docfx/build/docfx_local.json"

} elseif ($branchNameSegment -ieq "hotfix") {

    $version = "--property:AssemblyVersion=$fullVersion --property:VersionPrefix=$fullVersion"

    $dotnet_restore_param = "";
    $dotnet_build_param = "--no-restore --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $dotnet_test_param = "--configuration Release";
    $dotnet_pack_param =  "--force --configuration Release --property:ContinuousIntegrationBuild=true --property:WarningLevel=3 $version";
    $docfx_param = $null

}
