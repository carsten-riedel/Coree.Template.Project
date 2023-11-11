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
sudo apt-get update && sudo apt-get install -y dotnet-sdk-6.0 dotnet-sdk-7.0
```

## Install/Uninstall the templates
The commands below demonstrate how to install or uninstall the templates, primarily designed for .NET with Visual Studio 2022 compatibility in mind. Remember, template definitions might include specific limitations like conditional settings (true/false).
```
dotnet new install Coree.Template.Project
dotnet new uninstall Coree.Template.Project
```
The package contains the following templates:<br><br>
[.Net MSBuild Task library](#msbuild)<br><br>
[.Net Class library](#classlib)
[.Net Tool](#nettool)

#### Hint:
For testing packages created using these templates, consider setting up a local NuGet test repository. If you're looking to utilize locally built packages, simply establish a NuGet file repository.
In the packages dir `PackageSpecs.props` you can add `<LocalPackagesDir>$(userprofile)\localpackage</LocalPackagesDir>`<br>
Then add a local package source.

Linux/WSL (Sample useage):
```
dotnet nuget add source "$HOME\localpackage" --name "localpackage"
```

Windows cmd (Sample useage):
```
dotnet nuget add source "%userprofile%\localpackage" --name "localpackage"
```

## <a name="msbuild"> .Net MSBuild Task library
This template provides a foundation for building a .NET Standard compatible MSBuild task library, essential for tasks like build automation. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

```
dotnet new msbuildlib --PackageAuthor Me
```
**Easily edit your NuGet package metadata inside the Package directory.**
<br><br>

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyMSBuildTask" ; cd "MyMSBuildTask" ; dotnet new msbuildlib --PackageAuthor Me --force ; dotnet test ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyMSBuildTask" & cd "MyMSBuildTask" & dotnet new msbuildlib --PackageAuthor Me --force & dotnet test & dotnet pack & cd /D %userprofile%
```

**Enhance the TestScript.msbuild in the MSTest project to test your integration.**

## <a name="classlib"> .Net class library
This template provides a foundation for building a .NET compatible library. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

```
dotnet new classlibrary-dotnet-pack --PackageAuthor Me
```

**Easily edit your NuGet package metadata inside the Package directory.**

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyClassLib" ; cd "MyClassLib" ; dotnet new classlibrary-dotnet-pack --PackageAuthor Me --force ; dotnet test ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyClassLib" & cd "MyClassLib" & dotnet new classlibrary-dotnet-pack --PackageAuthor Me --force & dotnet test & dotnet pack & cd /D %userprofile%
```

## <a name="nettool"> .Net Tool

```
dotnet new nettool --PackageAuthor Me --ToolCommandName helloworld
```

**Easily edit your NuGet package metadata inside the Package directory.**

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyNetTool" ; cd "MyNetTool" ; dotnet new nettool --PackageAuthor Me --ToolCommandName helloworld --force ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyNetTool" & cd "MyNetTool" & dotnet new nettool --PackageAuthor Me --ToolCommandName helloworld --force & dotnet pack & cd /D %userprofile%
```

Assuming you've already copied your package to a NuGet source, whether it's local or remote, you can easily install it using the .NET Core CLI. Specifically, if you're created a prerelease version of a tool called MyNetTool.helloworld, you can install it globally on your machine with the following command
```
dotnet tool install -g MyNetTool.helloworld --prerelease
```
