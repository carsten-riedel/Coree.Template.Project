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
[.Net Class library](#classlib)<br><br>
[.Net Tool](#nettool)<br><br>
[.Net Powershell cmdlet](#powershell)<br><br>
[.Net Winforms](#winforms)<br><br>
[.Net Wpf](#wpf)<br><br>
[.Net Project Template](#template)<br><br>

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
dotnet new msbuildtasklib-coree --PackageAuthor Me
```
**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory**
<br><br>

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyMSBuildTask" ; cd "MyMSBuildTask" ; dotnet new msbuildtasklib-coree --PackageAuthor Me --force ; dotnet test ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyMSBuildTask" & cd "MyMSBuildTask" & dotnet new msbuildtasklib-coree --PackageAuthor Me --force & dotnet test & dotnet pack & cd /D %userprofile%
```

**Enhance the TestScript.msbuild in the MSTest project to test your integration.**

## <a name="classlib"> .Net class library
This template provides a foundation for building a .NET compatible library. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

```
dotnet new classlibrary-coree --PackageAuthor Me
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory**

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyClassLib" ; cd "MyClassLib" ; dotnet new classlibrary-coree --PackageAuthor Me --force ; dotnet test ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyClassLib" & cd "MyClassLib" & dotnet new classlibrary-coree --PackageAuthor Me --force & dotnet test & dotnet pack & cd /D %userprofile%
```

## <a name="nettool"> .Net Tool
This template provides a foundation for building a .NET commandline tool. The template is structured to support NuGet packaging and publishing, requiring an author's specification and ToolCommandName for these purposes.
The final command will be the ToolCommandName. The Packagename is the Project and ToolCommandName

```
dotnet new nettool --PackageAuthor Me --ToolCommandName helloworld
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory**

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
#REM OR use a temporary package location
dotnet tool install -g MyNetTool.helloworld --prerelease --add-source "%userprofile%\MyNetTool\Package\PackageOut"
```

Use:
```
C:\Users\MainUser>helloworld
Hello, World!
```

## <a name="powershell"> .Net Powershell cmdlet
### Description TBD

## <a name="winforms"> .Net Winforms (Windows only)
### Description TBD

## <a name="wpf"> .Net Wpf (Windows only)
This template serves as a base for developing WPF applications, optimized primarily for .NET publishing as an executable. It includes five distinct configurations tailored for the dotnet publish process. Enhanced with basic visual improvements, the template utilizes the MaterialDesignThemes.MahApps package. You can tailor the App.xaml to your requirements, particularly by altering color keys like `<Color x:Key="Primary200">#76bddf</Color>`, to suit your design preferences.

Publish settings.
1) Framework-required. (Purpose: Installer)
2) Framework-required, Single-file. (Purpose: Copy smallest) <- DEFAULT
3) Framework-included. (Purpose: Installer)
4) Framework-included, Single-file. (Purpose: Copy large)
5) Framework-included, Single-file, Compressed. (Purpose: Copy medium)

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyWpfApp" & cd "MyWpfApp" & dotnet new wpfapp-dotnet-publish --Author Me --force & dotnet publish & cd /D %userprofile%
```

The Single-file selection will have the following settings.
```
<PublishSingleFile>true</PublishSingleFile>
<PublishReadyToRun>true</PublishReadyToRun>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
```

### Screenshots
![image](https://github.com/carsten-riedel/Coree.Template.Project/assets/97656046/7540e391-0554-4f44-bf5b-eb5d6d0ea984)

![image](https://github.com/carsten-riedel/Coree.Template.Project/assets/97656046/18061fa0-4b72-49f3-baa6-8e49648a7991)

## <a name="template"> .Net Project Template
Indeed, it may sound a bit perplexing at first – a project template for creating project templates. However, it's quite straightforward. For a project to generate templates, these must be situated within the packages/root/content folder. Additionally, a default class library is incorporated, complete with the provided names, to kickstart your project template creation. For more detailed guidance, refer to the educational section at the beginning of this readme.

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyProjTemplate" ; cd "MyProjTemplate" ; dotnet new projecttemplate-dotnet-pack --PackageAuthor Me --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --force ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyProjTemplate" & cd "MyProjTemplate" & dotnet new projecttemplate-dotnet-pack --PackageAuthor Me --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --force & dotnet pack & cd /D %userprofile%
```
