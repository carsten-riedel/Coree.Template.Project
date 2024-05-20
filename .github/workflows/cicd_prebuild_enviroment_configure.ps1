
######################################################################################
#Log-Block -Stage "cicd_prebuild_enviroment_prepare" -Section "Tools" -Task "Install dotnet tools"

#if (-not (Test-CommandAvailability -CommandName "docfx"))
#{
#    Execute-Command "dotnet tool install --global docfx --version 2.74.1"
#}

######################################################################################
#Log-Block -Stage "cicd_prebuild_enviroment_prepare" -Section "Tools" -Task "Install powershell modules"

#if (-not (Test-CommandAvailability -CommandName "New-PGPKey"))
#{
#    Install-Module -Name PSPGP -AcceptLicense -AllowClobber -AllowPrerelease -Force
#}


######################################################################################
Log-Block -Stage "Prebuild enviroment" -Section "Prepare" -Task "Add github nuget source"

Execute-Command "dotnet nuget remove source github" -ExpectedExitCodes @(0,1)
Execute-Command "dotnet nuget add source --username carsten-riedel --password $PAT --store-password-in-clear-text --name github ""https://nuget.pkg.github.com/carsten-riedel/index.json"""

