## Install/Uninstall the templates
How to install or uninstall the templates
```
dotnet new install Coree.Template.Project
dotnet new uninstall Coree.Template.Project
```

## .Net MSBuild Task library
A project template to create a msbuild .netstandard compatible msbuild task library. Author is required for nuget pack/publish reasons.

cmd:
```
dotnet new msbuildlib --PackageAuthor Me
```

Sample unmodified creation of the template nuget:
```
cd $HOME
mkdir "MyMSBuildTask.Name" ; cd "MyMSBuildTask.Name" ; dotnet new msbuildlib --PackageAuthor Me ; dotnet pack
```