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