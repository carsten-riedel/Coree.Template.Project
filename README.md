

# Coree.Template.Project

![image](https://github.com/carsten-riedel/Coree.Template.Project/assets/97656046/1d691cb1-b24b-4827-be11-c96cd83d5a12)

Welcome to the Coree.Template.Project repository! This project offers a comprehensive suite of templates for Visual Studio and dotnet CLI, designed to streamline the creation of various .NET projects. From MSBuild tasks to class libraries and WPF applications, this repository serves as a one-stop resource for developers looking to enhance their .NET development workflow.

## WSL Setup

To set up WSL on Windows:
  1. Open the Command Prompt or PowerShell as an administrator.
  2. Execute: `wsl --install --no-distribution`.
  3. Restart your computer when prompted.

To install a wsl image e.g Ubuntu on Windows:
  1. Open the Command Prompt or PowerShell.
  2. Execute: `wsl --update` to update your wsl to the latest version.
  3. Execute: `wsl --set-default-version 2` to set the WSL version to WSL2.
  4. Execute: `wsl --list --online` to list all online availible wsl image versions.
  5. Execute: `wsl --install Ubuntu-22.04 --web-download` to install a online version as webdownload in the case the store is blocked.
  6. Enter your username. If you get an error use lowercase and numbers only.
  7. Enter your password.
  8. Confirm your password.

To uninstall a wsl image e.g Ubuntu on Windows:
  1. Open the Command Prompt or PowerShell.
  2. Execute: `wsl --list` to see a list of your local installed wsl images.
  3. Execute: `wsl --unregister Ubuntu-22.04` to remove a installed wsl image.

## Get ready for dotnet powershell and vscode on ubuntu

To install dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute: `sudo wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && sudo dpkg -i packages-microsoft-prod.deb && sudo rm packages-microsoft-prod.deb` to install the microsoft apt package sources.
  3. Execute: `sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0` to install the .NET 8.0 SDK.
  4. Execute: `dotnet tool install --global PowerShell` if you want to use Powershell Core.
  5. Execute: `sudo wget --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868 && sudo apt install -y ./code.deb && rm -f ./code.deb` to install Visual Studio code.
  6. Execute: `echo >>"$HOME/.bashrc" "export DONT_PROMPT_WSL_INSTALL=1" && mkdir -p "$HOME/source/repos" && mkdir -p "$HOME/localpackage"` to get rid of the Visual Studio code promt, and to create some default directories.

To uninstall dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute: `sudo apt-get remove -y code` to uninstall Visual Studio Code.
  3. Execute: `dotnet tool uninstall --global PowerShell` to uninstall Powershell.
  4. Execute: `sudo apt remove -y --purge --autoremove "dotnet*" "aspnet*" "netstandard*" && sudo rm /etc/apt/sources.list.d/microsoft-prod.list` to uninstall all .NET 8.0 packages.
  
## Windows Setup

Normal install and download procedure.
  1. [Download/Install dotnet SDK](https://dotnet.microsoft.com/en-us/download)
  2. [Download/Install Visual Studio Code](https://code.visualstudio.com/)
  3. [Download/Install Powershell](https://learn.microsoft.com/en-US/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3)



# Install/Uninstall the templates
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
dotnet nuget add source "$HOME/localpackage" --name "localpackage"
```

Windows cmd (Sample useage):
```
dotnet nuget add source "%userprofile%\localpackage" --name "localpackage"
```

To remove the local source
```
dotnet nuget remove source "localpackage"
```

## <a name="msbuild"> .Net MSBuild Task library
This template provides a foundation for building a .NET Standard compatible MSBuild task library, essential for tasks like build automation. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

```
dotnet new msbuildtasklib-coree --PackageAuthor Me
```
**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**
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

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyClassLib" ; cd "MyClassLib" ; dotnet new classlib-coree --PackageAuthor Me --force ; dotnet test ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyClassLib" & cd "MyClassLib" & dotnet new classlib-coree --PackageAuthor Me --force & dotnet test & dotnet pack & cd /D %userprofile%
```

## <a name="nettool"> .Net Tool
This template provides a foundation for building a .NET commandline tool. The template is structured to support NuGet packaging and publishing, requiring an author's specification and ToolCommandName for these purposes.
The final command will be the ToolCommandName. The Packagename is the Project and ToolCommandName

```
dotnet new nettool-coree --PackageAuthor Me --ToolCommandName helloworld
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
cd $HOME ;mkdir "MyNetTool" ; cd "MyNetTool" ; dotnet new nettool-coree --PackageAuthor Me --ToolCommandName helloworld --force ; dotnet pack ; cd $HOME
```

Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyNetTool" & cd "MyNetTool" & dotnet new nettool-coree --PackageAuthor Me --ToolCommandName helloworld --force & dotnet pack & cd /D %userprofile%
```

Assuming you've already copied your package to a NuGet source, whether it's local or remote, you can easily install it using the .NET Core CLI. Specifically, if you're created a prerelease version of a tool called MyNetTool.helloworld, you can install it globally on your machine with the following command.
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
cd /D %userprofile% & mkdir "MyWpfApp" & cd "MyWpfApp" & dotnet new wpfapp-coree --Author Me --force & dotnet publish & cd /D %userprofile%
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
cd $HOME ;mkdir "MyProjTemplate" ; cd "MyProjTemplate" ; dotnet new projecttemplate-coree --PackageAuthor Me --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --force ; dotnet pack ; cd $HOME
```
Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyProjTemplate" & cd "MyProjTemplate" & dotnet new projecttemplate-coree --PackageAuthor Me --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --force & dotnet pack & cd /D %userprofile%
```

# Educational

For more information and resources for templating:
  - [MS Learn: Custom templates for dotnet new](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
  - [MS Learn: Create an item template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-item-template)
  - [MS Learn: Create a project template](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-project-template)
  - [MS Learn: Create a template package](https://learn.microsoft.com/en-us/dotnet/core/tutorials/cli-templates-create-template-package?pivots=dotnet-6-0)
  - [Github: Templating Wiki](https://github.com/dotnet/templating/wiki)
  - [Github: Net template samples](https://github.com/dotnet/templating/tree/main/dotnet-template-samples)
  - [Microsoft Project Repository .net winforms templates](https://github.com/dotnet/winforms/tree/main/pkg/Microsoft.Dotnet.WinForms.ProjectTemplates/content/WinFormsApplication-CSharp)

For more information and resources for msbuild tasks:
  - [MSbuild Custom Task](https://github.com/dotnet/samples/tree/main/msbuild/custom-task-code-generation)

