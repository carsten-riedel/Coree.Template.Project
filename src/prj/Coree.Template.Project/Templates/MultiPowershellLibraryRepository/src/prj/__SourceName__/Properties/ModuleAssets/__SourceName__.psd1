@{
    RootModule = '__SourceName__.psm1'
    ModuleVersion = '0.1.0'
    GUID = '00000000-0000-0000-0000-000000000000'
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
    PowerShellVersion = '__MinimumPowerShellVersion__'
    CompatiblePSEditions = @(__PowerShellEditions__)
    FunctionsToExport = @()
    CmdletsToExport = @('Get-SampleValue')
    VariablesToExport = @()
    AliasesToExport = @()

    PrivateData = @{
        PSData = @{
            Tags = @('PowerShell', 'BinaryModule')
            ReleaseNotes = 'See ReleaseNotes.txt in the module package.'
        }
    }
}
