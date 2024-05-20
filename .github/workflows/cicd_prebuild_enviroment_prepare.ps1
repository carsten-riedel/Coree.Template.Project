dotnet tool install --global PowerShell

######################################################################################
Log-Block -Stage "Setup" -Section "Tools" -Task "Install dotnet tools"

if (-not (Test-CommandAvailability -CommandName "docfx"))
{
    Execute-Command "dotnet tool install --global docfx --version 2.74.1"
}

######################################################################################
Log-Block -Stage "Setup" -Section "Tools" -Task "Install powershell modules"

if (-not (Test-CommandAvailability -CommandName "New-PGPKey"))
{
    #Install-Module -Name PSPGP -AcceptLicense -AllowClobber -AllowPrerelease -Force
}


######################################################################################
Log-Block -Stage "Setup" -Section "Tools" -Task "Add addtional nuget source"

Execute-Command "dotnet nuget add source --username carsten-riedel --password $PAT --store-password-in-clear-text --name github ""https://nuget.pkg.github.com/carsten-riedel/index.json"""

