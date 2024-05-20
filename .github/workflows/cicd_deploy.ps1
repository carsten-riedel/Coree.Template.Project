######################################################################################
Log-Block -Stage "Deploy" -Section "Nuget" -Task "Nuget"

if ($branchNameSegment -ieq "feature") {

    $basePath = "$topLevelPath/src/Projects/Coree.Template.Project"
    $pattern = "*.nupkg"
    $firstFileMatch = Get-ChildItem -Path $basePath -Filter $pattern -File -Recurse | Select-Object -First 1
    Execute-Command "dotnet nuget push ""$($firstFileMatch.FullName)"" --api-key $PAT --source ""github"""

} elseif ($branchNameSegment -ieq "develop") {

    $basePath = "$topLevelPath/src/Projects/Coree.Template.Project"
    $pattern = "*.nupkg"
    $firstFileMatch = Get-ChildItem -Path $basePath -Filter $pattern -File -Recurse | Select-Object -First 1
    Execute-Command "dotnet nuget push ""$($firstFileMatch.FullName)"" --api-key $PAT --source ""github"""

} elseif ($branchNameSegment -ieq "release") {

    $basePath = "$topLevelPath/src/Projects/Coree.Template.Project"
    $pattern = "*.nupkg"
    $firstFileMatch = Get-ChildItem -Path $basePath -Filter $pattern -File -Recurse | Select-Object -First 1
    Execute-Command "dotnet nuget push ""$($firstFileMatch.FullName)"" --api-key $PAT --source ""github"""

    dotnet nuget push "$($firstFileMatch.FullName)" --api-key $NUGET_TEST_PAT --source https://apiint.nugettest.org/v3/index.json

} elseif ($branchNameSegment -ieq "master") {

    $basePath = "$topLevelPath/src/Projects/Coree.Template.Project"
    $pattern = "*.nupkg"
    $firstFileMatch = Get-ChildItem -Path $basePath -Filter $pattern -File -Recurse | Select-Object -First 1
    Execute-Command "dotnet nuget push ""$($firstFileMatch.FullName)"" --api-key $PAT --source ""github"""

    dotnet nuget push "$($firstFileMatch.FullName)" --api-key $NUGET_PAT --source https://api.nuget.org/v3/index.json

} elseif ($branchNameSegment -ieq "hotfix") {

    $basePath = "$topLevelPath/src/Projects/Coree.Template.Project"
    $pattern = "*.nupkg"
    $firstFileMatch = Get-ChildItem -Path $basePath -Filter $pattern -File -Recurse | Select-Object -First 1
    Execute-Command "dotnet nuget push ""$($firstFileMatch.FullName)"" --api-key $PAT --source ""github"""

    dotnet nuget push "$($firstFileMatch.FullName)" --api-key $NUGET_PAT --source https://api.nuget.org/v3/index.json
}

######################################################################################
Log-Block -Stage "Post Deploy" -Section "Tag and Push" -Task ""

if ($branchNameSegment -eq "master" -OR $branchNameSegment -eq "release" -OR $branchNameSegment -eq "hostfix")
{
    $tag = "v$fullVersion"
}
else {
    $tag = "v$fullVersion-$branchNameSegment"
}

$gitUserLocal = git config user.name
$gitMailLocal = git config user.email

# Check if the variables are null or empty (including whitespace)
if ([string]::IsNullOrWhiteSpace($gitUserLocal) -or [string]::IsNullOrWhiteSpace($gitMailLocal)) {
    $gitTempUser= "Workflow"
    $gitTempMail = "carstenriedel@outlook.com"  # Assuming a placeholder email
} else {
    $gitTempUser= $gitUserLocal
    $gitTempMail = $gitMailLocal
}

git config user.name $gitTempUser
git config user.email $gitTempMail

Execute-Command -Command "git add --all"
Execute-Command -Command "git commit -m ""Updated form Workflow [no ci]""" -ExpectedExitCodes @(0,1)
Execute-Command -Command "git push origin $branchName"
Execute-Command -Command "git tag -a ""$tag"" -m ""[no ci]"""
Execute-Command -Command "git push origin ""$tag"""
Execute-Command -Command "gh release create ""$tag"" --notes ""auto release"""
Execute-Command -Command "gh release upload ""$tag"" ""$($firstFileMatch.FullName)"" --clobber"

#restore
git config user.name $gitUserLocal
git config user.email $gitMailLocal