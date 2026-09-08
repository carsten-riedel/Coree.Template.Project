param (
    [string]$PAT,
    [Alias("NUGET_PAT")]
    [string]$SECRET_NUGET_APIKEY,
    [Alias("NUGET_TEST_PAT")]
    [string]$SECRET_INTTESTNUGET_APIKEY
)

$NUGET_PAT = $SECRET_NUGET_APIKEY
$NUGET_TEST_PAT = $SECRET_INTTESTNUGET_APIKEY

$ErrorActionPreference = 'Stop'

Start-Transcript -Path "$(Join-Path -Path "$PSScriptRoot" -ChildPath "$($(Get-ChildItem "$PSCommandPath").BaseName)-$(Get-Date -f 'yyyyMMdd_HHmmss').log")"

. "$PSScriptRoot/cicd_util.ps1"
. "$PSScriptRoot/cicd_prebuild_environment_requirements.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_prepare.ps1"
. "$PSScriptRoot/cicd_prebuild_envars_check.ps1"
. "$PSScriptRoot/cicd_prebuild_environment_configure.ps1"

. "$PSScriptRoot/cicd_build_clean.ps1"
. "$PSScriptRoot/cicd_build_config.ps1"
. "$PSScriptRoot/cicd_build.ps1"

. "$PSScriptRoot/cicd_deploy.ps1"


#git status --porcelain $sourceCodeFolder

Stop-Transcript

$x=1
