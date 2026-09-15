Set-StrictMode -Version 3.0

$binaryTargetCandidates = if ($PSEdition -eq 'Desktop') {
    @('net48', 'net462', 'netstandard2.0')
}
else {
    $targets = @()
    if ($PSVersionTable.PSVersion -ge [version]'7.6') {
        $targets += 'net10.0'
    }

    if ($PSVersionTable.PSVersion -ge [version]'7.4') {
        $targets += 'net8.0'
    }

    $targets += 'netstandard2.0'
    $targets
}

$binaryModulePath = $binaryTargetCandidates |
    ForEach-Object { Join-Path $PSScriptRoot "$_\__SourceName__.dll" } |
    Where-Object { Test-Path -LiteralPath $_ -PathType Leaf } |
    Select-Object -First 1

if (-not $binaryModulePath) {
    throw "__SourceName__ has no binary compatible with $($PSVersionTable.PSEdition) PowerShell $($PSVersionTable.PSVersion). Recreate or rebuild the module with a matching PowerShell target."
}

$script:BinaryModule = Import-Module -Name $binaryModulePath -Force -PassThru -ErrorAction Stop
$ExecutionContext.SessionState.Module.OnRemove = {
    if ($script:BinaryModule) {
        Remove-Module -ModuleInfo $script:BinaryModule -Force -ErrorAction SilentlyContinue
    }
}

Export-ModuleMember -Cmdlet 'Get-SampleValue'
