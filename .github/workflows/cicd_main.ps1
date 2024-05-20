$ErrorActionPreference = 'Stop'

$version    = $PSVersionTable.PSVersion.ToString()
$datetime   = Get-Date -f 'yyyyMMdd_HHmmss'
$filename   = "cicd-${version}-${datetime}.log"
$Transcript = Join-Path -Path "$PSScriptRoot" -ChildPath $filename
Start-Transcript -Path "$Transcript"

$PAT = $args[0]
$NUGET_PAT = $args[1]
$NUGET_TEST_PAT = $args[2]

. "$PSScriptRoot/cicd_util.ps1"
. "$PSScriptRoot/cicd_prebuild_enviroment_prepare.ps1"
. "$PSScriptRoot/cicd_prebuild_enviroment_check.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_prepare.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_check.ps1"
. "$PSScriptRoot/cicd_build_clean.ps1"
. "$PSScriptRoot/cicd_build_config.ps1"
. "$PSScriptRoot/cicd_build.ps1"
. "$PSScriptRoot/cicd_deploy.ps1"
. "$PSScriptRoot/cicd_postdeploy_clean.ps1"
. "$PSScriptRoot/cicd_postdeploy_run.ps1"

#git status --porcelain $sourceCodeFolder

Stop-Transcript