# This is the authored module manifest. Build stages a copy under bin/Module.
# PackAsPowerShellModule.targets changes only that staged copy: it synchronizes
# ModuleVersion with the NuGet package version, adds a prerelease label when
# needed, and replaces the ReleaseNotes placeholder from PackageReleaseNotes.txt.
@{
    # The script loader selects and imports the binary built for the active host.
    RootModule = '__SourceName__.psm1'

    # A normal build uses this development value. Pack overwrites it in the
    # staged manifest so the PowerShell Gallery and NuGet versions agree.
    ModuleVersion = '0.1.0'

    # Keep this identity stable for the lifetime of the published module.
    GUID = '00000000-0000-0000-0000-000000000000'

    # These values describe the module in Get-Module and PowerShell Gallery.
    Author = 'MyAuthor'
#if ((PackageCopyrightHolderIsOrganization == true) && PackageCopyrightHolderIsSet)
    CompanyName = 'MyCopyrightHolder'
#else
    CompanyName = ''
#endif
#if (PackageCopyrightHolderIsSet)
    Copyright = 'Copyright © 1975 MyCopyrightHolder'
#else
    Copyright = 'Copyright © 1975 MyAuthor'
#endif
    Description = 'Binary PowerShell module __SourceName__.'

    # PowerShellVersion is the minimum selected host. CompatiblePSEditions
    # advertises the selected native host families; the loader still verifies
    # that a compatible target DLL is present before importing it.
    PowerShellVersion = '__MinimumPowerShellVersion__'
    CompatiblePSEditions = @(__PowerShellEditions__)

    # Keep exports explicit. Update CmdletsToExport whenever the public binary
    # command surface changes; avoid wildcard exports in a published module.
    FunctionsToExport = @()
    CmdletsToExport = @('Get-SampleValue')
    VariablesToExport = @()
    AliasesToExport = @()

    # PSData is consumed by PowerShellGet/PowerShell Gallery for discovery and
    # display. Package metadata in the csproj remains the corresponding NuGet view.
    PrivateData = @{
        PSData = @{
            Tags = @('PowerShell', 'BinaryModule')
#if (ProjectLicense == "MIT")
            LicenseUri = 'https://licenses.nuget.org/MIT'
#endif
#if (ProjectLicense == "BSD3Clause")
            LicenseUri = 'https://opensource.org/licenses/BSD-3-Clause'
#endif
#if (ProjectLicense == "Apache2")
            LicenseUri = 'https://www.apache.org/licenses/LICENSE-2.0'
#endif

            # Pack replaces this placeholder in the staged manifest only.
            # The source text lives in Properties/NugetMetadata/PackageReleaseNotes.txt.
            ReleaseNotes = 'Package release notes are injected during pack.'
        }
    }
}
