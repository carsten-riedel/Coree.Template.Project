# This root script module is a small host dispatcher. It keeps one public module
# identity while loading the best compatible binary produced by the multi-target
# C# project. The candidate order is intentional: prefer the newest native binary
# supported by the current host, then fall back to the portable build if present.
Set-StrictMode -Version 3.0

# Windows PowerShell runs on .NET Framework. PowerShell 7+ runs on modern .NET.
# Only folders actually selected and built by the project exist in the staged
# module, so a missing preferred target naturally falls through to the next one.
$binaryTargetCandidates = if ($PSEdition -eq 'Desktop') {
    # A net48 host can load net462; prefer net48 when both were intentionally built.
    @('net48', 'net462', 'netstandard2.0')
}
else {
    $targets = @()

    # PowerShell 7.6 is based on .NET 10; earlier Core hosts must not probe net10.0.
    if ($PSVersionTable.PSVersion -ge [version]'7.6') {
        $targets += 'net10.0'
    }

    # PowerShell 7.4 introduced the .NET 8 host line. Newer releases can also
    # load the broader-compatibility net8.0 binary.
    if ($PSVersionTable.PSVersion -ge [version]'7.4') {
        $targets += 'net8.0'
    }

    # PowerShellStandard is the shared Desktop/Core fallback, not a third host family.
    $targets += 'netstandard2.0'
    $targets
}

# Resolve the first candidate whose DLL is present beside this loader. Using
# $PSScriptRoot makes import independent of the caller's current directory.
$binaryModulePath = $binaryTargetCandidates |
    ForEach-Object { Join-Path $PSScriptRoot "$_\__SourceName__.dll" } |
    Where-Object { Test-Path -LiteralPath $_ -PathType Leaf } |
    Select-Object -First 1

# Fail with host and version context instead of letting Import-Module emit an
# indirect file-not-found error when the package lacks a compatible target.
if (-not $binaryModulePath) {
    throw "__SourceName__ has no binary compatible with $($PSVersionTable.PSEdition) PowerShell $($PSVersionTable.PSVersion). Recreate or rebuild the module with a matching PowerShell target."
}

# Import the selected assembly as a nested module and retain its module object.
# The handle lets removal of this wrapper also remove the nested binary module,
# which avoids stale commands and makes rebuild/reimport cycles predictable.
$script:BinaryModule = Import-Module -Name $binaryModulePath -Force -PassThru -ErrorAction Stop
$ExecutionContext.SessionState.Module.OnRemove = {
    if ($script:BinaryModule) {
        Remove-Module -ModuleInfo $script:BinaryModule -Force -ErrorAction SilentlyContinue
    }
}

# The wrapper publishes only the intended public cmdlets from the nested module.
Export-ModuleMember -Cmdlet 'Get-SampleValue'
