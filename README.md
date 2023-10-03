# Coree.Template.Project
Project Templates for Visual Studio and dotnet cli

# Educational

## About Templates creation (Visual Studio; Visual Studio Code; dotnet cli)

- [MS Learn: Custom templates for dotnet new](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)

- [MS Learn: Create an item template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-item-template)

- [MS Learn: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)

- [MS Learn: Create a template package](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-template-package?pivots=dotnet-6-0)

- [Github: Templating Wiki](https://github.com/dotnet/templating/wiki)

- [Github: Net template samples](https://github.com/dotnet/templating/tree/main/dotnet-template-samples)

- [Microsoft Project Repository .net winforms templates](https://github.com/dotnet/winforms/tree/main/pkg/Microsoft.Dotnet.WinForms.ProjectTemplates/content/WinFormsApplication-CSharp)

## About MSBuild Tasks creation
- [MSbuild Custom Task](https://github.com/dotnet/samples/tree/main/msbuild/custom-task-code-generation)

# Coree Templates Project

## Prerequirements

[Download/Install dotnet SDK](https://dotnet.microsoft.com/en-us/download)

Notes Ubuntu/WSL:
```
#In the case dotnet sdk does not install.
sudo apt remove -y --purge --autoremove "dotnet*" "aspnetcore*" && sudo rm /etc/apt/sources.list.d/microsoft-prod.list

#Install the dotnet sdk
sudo apt-get update && sudo apt-get install -y dotnet-sdk-7.0
```

## Install/Uninstall the templates
How to install or uninstall the templates
```
dotnet new install Coree.Template.Project
dotnet new uninstall Coree.Template.Project
```

## .Net MSBuild Task library
A project template to create a msbuild .netstandard compatible msbuild task library. Author is required for nuget pack/publish reasons.

cmdline:
```
dotnet new msbuildlib --PackageAuthor Me
```

Linux/WSL:
```
#create directory, create the project, create a nugetpackage
cd $HOME ;mkdir "MyMSBuildTask" ; cd "MyMSBuildTask" ; dotnet new msbuildlib --PackageAuthor Me ; dotnet pack ; cd $HOME
# Create a local nuget source directory
mkdir $HOME/nugetlocal & dotnet nuget add source "$HOME/nugetlocal" --name "UserNugetLocal"
# Copy nuget to local source
cp MyMSBuildTask/bin/Debug/MyMSBuildTask.0.0.1-prerelease.nupkg "$HOME/nugetlocal"


cd $HOME ;mkdir "ConsoleApp" ; cd "ConsoleApp" ; dotnet new console ; dotnet add package MyMSBuildTask --prerelease ; cd $HOME
```

Windows cmd:
```
REM create directory, create the project, create a nugetpackage
cd /D %userprofile% & mkdir "MyMSBuildTask" & cd "MyMSBuildTask" & dotnet new msbuildlib --PackageAuthor Me & dotnet pack & cd /D %userprofile%
REM Create a local nuget source directory
mkdir %userprofile%\nugetlocal & dotnet nuget add source "%userprofile%\nugetlocal" --name "UserNugetLocal"
REM Copy nuget to local source
copy /y MyMSBuildTask\bin\Debug\MyMSBuildTask.0.0.1-prerelease.nupkg "%userprofile%\nugetlocal"


cd /D %userprofile% & mkdir "ConsoleApp" & cd "ConsoleApp" & dotnet new console & dotnet add package MyMSBuildTask --prerelease & cd /D %userprofile%
```

## <span style="color:red">Inside your ConsoleApp.csproj add a target</span>
```
	<Target Name="DemoTarget" BeforeTargets="CoreCompile">
		<DumpEnvVarsTask/>
		<DumpGlobalPropTask/>
	</Target>
```