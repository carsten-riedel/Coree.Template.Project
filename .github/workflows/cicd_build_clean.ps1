
######################################################################################
Log-Block -Stage "Setup" -Section "Clean" -Task "Clean local binaries"

# Example of how to call the function and capture the results
$results = Find-SpecialSubfolders -Path "$topLevelPath/$sourceCodeFolder" -FolderNames @('bin', 'obj')
foreach($item in $results)
{
    Remove-FilesAndDirectories -FolderPath $item -DumpDeleted $false
}