param (
    [string]$PAT = $args[0],
    [string]$NUGET_PAT = $args[1],
    [string]$NUGET_TEST_PAT= $args[2]
)

$ErrorActionPreference = 'Stop'

Start-Transcript -Path "$(Join-Path -Path "$PSScriptRoot" -ChildPath "$($(Get-ChildItem "$PSCommandPath").BaseName)-$(Get-Date -f 'yyyyMMdd_HHmmss').log")"

. "$PSScriptRoot/cicd_util.ps1"
. "$PSScriptRoot/cicd_prebuild_enviroment_requirments.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_prepare.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_check.ps1"
. "$PSScriptRoot/cicd_prebuild_enviroment_configure.ps1"

. "$PSScriptRoot/cicd_build_clean.ps1"
. "$PSScriptRoot/cicd_build_config.ps1"
. "$PSScriptRoot/cicd_build.ps1"


#git status --porcelain $sourceCodeFolder

Stop-Transcript

$x=1