# Coree.Template.Project

![brand](https://raw.githubusercontent.com/carsten-riedel/Coree.Template.Project/main/assets/images/brand.png)

Welcome to the Coree.Template.Project repository! This project offers a comprehensive suite of templates for Visual Studio and dotnet CLI, designed to streamline the creation of various .NET projects. From MSBuild tasks to class libraries and WPF applications, this repository serves as a one-stop resource for developers looking to enhance their .NET development workflow.

## Preperation (Prerequirments)
  
### Windows Setup

Normal install and download procedure.
  1. [Download/Install dotnet SDK](https://dotnet.microsoft.com/en-us/download)
  2. [Download/Install Visual Studio Code](https://code.visualstudio.com/)
  3. [Download/Install Powershell](https://learn.microsoft.com/en-US/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3)
  4. [Download/Install git](https://git-scm.com/download/win)

#### User-Space Installation of .NET and PowerShell Core on Windows.

To install dotnet and powershell core from cmd in Windows:
  1. Open the Command Prompt.
  2. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 6.0"` to install the .NET 6.0 SDK.
  3. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 7.0"` to install the .NET 7.0 SDK.
  4. Execute: `powershell -NoProfile -ExecutionPolicy unrestricted -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; &([scriptblock]::Create((Invoke-WebRequest -UseBasicParsing 'https://dot.net/v1/dotnet-install.ps1'))) -channel 8.0"` to install the .NET 8.0 SDK.
  5. Execute: `powershell -NoProfile -ExecutionPolicy Unrestricted -Command "& {[Environment]::SetEnvironmentVariable('DOTNET_ROOT', \"$env:localappdata\Microsoft\dotnet\", 'User')}"` to set the enviroment variables.
  6. Execute: `powershell -NoProfile -ExecutionPolicy Unrestricted -Command "& {[Environment]::SetEnvironmentVariable('PATH', \"$($env:path);$env:localappdata\Microsoft\dotnet\", 'User')}"`  to set the enviroment variables.
  7. Execute: `SET "DOTNET_ROOT=%localappdata%\Microsoft\dotnet" & SET "PATH=%PATH%;%localappdata%\Microsoft\dotnet"`  to set the current session enviroment variables.
  8. Execute: `dotnet tool install --global Powershell --no-cache` 

### WSL Setup

WSL (Windows Subsystem for Linux) enables running Linux environments on Windows. This section covers its setup, essential for cross-platform development.

To set up WSL on Windows:
  1. Open the Command Prompt or PowerShell as an **administrator**.
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

### Setting up .NET, PowerShell, and Visual Studio Code on Ubuntu

To install dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute:  `sudo apt-get update && sudo apt-get -y upgrade ` to upgrade the linux distrobution to the latest state.
  3. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 6.0` to install the .NET 6.0 SDK.
  4. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 7.0` to install the .NET 7.0 SDK.
  5. Execute:  `curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -channel 8.0` to install the .NET 8.0 SDK.
  6. Execute:  `export DOTNET_ROOT=$HOME/.dotnet ; export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools ; echo 'export DOTNET_ROOT=$HOME/.dotnet' >> $HOME/.bashrc && echo 'export PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools' >> $HOME/.bashrc` to add the dotnet root and path to the enviroment and shell startup.
  7. Execute:  `dotnet tool install --global PowerShell` if you want to use Powershell Core.
  8. Execute:  `sudo wget --content-disposition -O code.deb https://go.microsoft.com/fwlink/?LinkID=760868 && sudo apt install -y ./code.deb && rm -f ./code.deb` to install Visual Studio code.
  9. Execute:  `export DONT_PROMPT_WSL_INSTALL=1 ; echo 'export DONT_PROMPT_WSL_INSTALL=1' >> $HOME/.bashrc ; mkdir -p "$HOME/source/repos" ; mkdir -p "$HOME/source/packages"` to get rid of the Visual Studio code prompt, and to create some default directories.
 10. Optional:  Start visual studio code, Execute: `code`
 11. Optional:  In the case visual studio code flickers shutdown wsl inside windows command prompt and start wsl again. Execute: `wsl --shutdown & wsl export DONT_PROMPT_WSL_INSTALL=1 ; code`

To uninstall dotnet powershell and vscode:
  1. Open the wsl app in windows or type `wsl` inside a command prompt.
  2. Execute: `sudo apt-get remove -y code` to uninstall Visual Studio Code.
  3. Execute: `dotnet tool uninstall --global PowerShell` to uninstall Powershell.
  4. Manual:  Remove entries $HOME/.bashrc and delete the .dotnet folder.

# Install/Uninstall the templates
The commands below demonstrate how to install or uninstall the templates, primarily designed for .NET with Visual Studio 2022 compatibility in mind. Remember, template definitions might include specific limitations like conditional settings (true/false).
```
dotnet new install Coree.Template.Project
dotnet new uninstall Coree.Template.Project
```
The package contains the following templates:
  1. [.NET MSBuild Task library](#Net-MSBuild-Task-library)
  2. [.NET Class library](#Net-class-library)
  3. [.NET Tool](#Net-Tool)
  4. [.NET Wpf](#Net-Wpf-Windows-only)
  5. [.NET Project Template](#Net-Project-Template)

#### Hint:
For testing packages created using these templates, consider setting up a local NuGet test repository. If you're looking to utilize locally built packages, simply establish a NuGet file repository.
In the packages dir `PackageSpecs.props` you can add `<LocalPackagesDir>$(userprofile)\source\packages</LocalPackagesDir>`

Then add a local package source.

Linux/WSL (Sample useage):
```
mkdir -p "$HOME/source/packages" ; dotnet nuget add source "$HOME/source/packages" --name "SourcePackages"
```

Windows cmd (Sample useage):
```
mkdir "%userprofile%\source\packages" & dotnet nuget add source "%userprofile%\source\packages" --name "SourcePackages"
```

To remove the local source
```
dotnet nuget remove source "SourcePackages"
```

## .NET MSBuild Task library
This template provides a foundation for building a .NET Standard compatible MSBuild task library, essential for tasks like build automation. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

General use:
```
dotnet new msbuildtasklib-coree --PackageAuthor Me
```
**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyMSBuildTask" ; cd "source/repos/MyMSBuildTask" ; dotnet new msbuildtasklib-coree --PackageAuthor Me --name "MyMSBuildTask" --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyMSBuildTask" & cd "source\repos\MyMSBuildTask" & dotnet new msbuildtasklib-coree --PackageAuthor Me --name "MyMSBuildTask" --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

**Enhance the TestScript.msbuild in the MSTest project to test your integration.**

## .NET Class library
This template provides a foundation for building a .NET compatible library. It includes an MSTest project for testing the functionality you develop. The template is structured to support NuGet packaging and publishing, requiring an author's specification for these purposes.

The test project is the primary source of functionality in this template. To ensure it meets your specific project needs, you should tailor and integrate it accordingly. I pre-configured most variables to minimize the effort required in delving deeply into each documentation, streamlining your setup process.
If you keep the default setting, you can remove the test project without any impact.

**Features:**

- **MSTest Project**: Integrated for robust functionality testing.
- **Coverlet Code Coverage**: Measures code coverage to ensure comprehensive testing.
- **ReportGenerator**: Generates detailed reports on code coverage.
- **BenchmarkDotNet**: Included for performance benchmarking.
- **Docfx**: Facilitates documentation generation from source code and Markdown files.
- **NLog**: Implemented for basic logging, enhancing debugging and monitoring capabilities.
- **Global.json & dotnet-tools.json**: Specify SDK versions and local tool dependencies for a consistent development environment.
- **Solution Items**: Organized for better management of global solution-related files.
- **Nuget-license overview**: Uses the dotnet-project-licenses tool to document licenses used.

General use:
```
dotnet new classlib-coree --PackageAuthor Me
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyClassLib" ; cd "source/repos/MyClassLib" ; dotnet new classlib-coree --PackageAuthor Me --name "MyClassLib" --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyClassLib" & cd "source\repos\MyClassLib" & dotnet new classlib-coree --PackageAuthor Me --name "MyClassLib" --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

## .NET Tool
This template provides a foundation for building a .NET commandline tool. The template is structured to support NuGet packaging and publishing, requiring an author's specification and ToolCommandName for these purposes.
The final command will be the ToolCommandName. The Packagename is the Project and ToolCommandName

General use:
```
dotnet new nettool-coree --PackageAuthor Me --ToolCommandName helloworld
```

**Modify the metadata for your NuGet package by accessing the PackageMetadata.props file located in the Package directory.**

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyNetTool" ; cd "source/repos/MyNetTool" ; dotnet new nettool-coree --PackageAuthor Me --name "MyNetTool" --ToolCommandName helloworld --output "src" --force ; git init ; cd "src" ; dotnet test ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```

Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyNetTool" & cd "source\repos\MyNetTool" & dotnet new nettool-coree --PackageAuthor Me --name "MyNetTool"  --ToolCommandName helloworld --output "src" --force & git init & cd "src" & dotnet test & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```
Assuming you've already copied your package to a NuGet source, whether it's local or remote, you can easily install it using the .NET Core CLI. Specifically, if you're created a prerelease version of a tool called MyNetTool.helloworld, you can install it globally on your machine with the following command.

```
dotnet tool install -g MyNetTool --prerelease
#REM OR use a temporary package location
dotnet tool install -g MyNetTool --prerelease --add-source "%userprofile%\MyNetTool\MyNetTool\bin\Pack"
```

Use:
```
C:\Users\MainUser>helloworld
Hello, World!
```

## .NET Wpf (Windows only)
This template serves as a base for developing WPF applications, optimized primarily for .NET publishing as an executable. It includes five distinct configurations tailored for the dotnet publish process. Enhanced with basic visual improvements, the template utilizes the MaterialDesignThemes.MahApps package. You can tailor the App.xaml to your requirements, particularly by altering color keys like `<Color x:Key="Primary200">#76bddf</Color>`, to suit your design preferences.

General use:
```
dotnet new wpfapp-coree --Author Me
```
Windows cmd (Sample useage):
```
cd /D %userprofile% & mkdir "MyWpfApp" & cd "MyWpfApp" & dotnet new wpfapp-coree --Author Me --force & dotnet publish & cd /D %userprofile%
```

Publish settings.
1) Framework-required. (Purpose: Installer)
2) Framework-required, Single-file. (Purpose: Copy smallest) <- DEFAULT
3) Framework-included. (Purpose: Installer)
4) Framework-included, Single-file. (Purpose: Copy large)
5) Framework-included, Single-file, Compressed. (Purpose: Copy medium)

The Single-file selection will have the following settings.
```
<PublishSingleFile>true</PublishSingleFile>
<PublishReadyToRun>true</PublishReadyToRun>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
```

### Screenshots
![image](https://user-images.githubusercontent.com/97656046/282236852-7540e391-0554-4f44-bf5b-eb5d6d0ea984.png)

![image](https://user-images.githubusercontent.com/97656046/282236828-18061fa0-4b72-49f3-baa6-8e49648a7991.png)

## .NET Project Template
Indeed, it may sound a bit perplexing at first – a project template for creating project templates. However, it's quite straightforward. For a project to generate templates, these must be situated within the packages/root/content folder. Additionally, a default class library is incorporated, complete with the provided names, to kickstart your project template creation. For more detailed guidance, refer to the educational section at the beginning of this readme.

General use:
```
dotnet new projecttemplate-coree --PackageAuthor "me" --SampleTemplateName "Testing template" --SampleTemplateShortName "test"
```

Linux/WSL (Sample useage):
```
dotnet new install Coree.Template.Project ; cd $HOME ; mkdir -p "source/repos/MyProjTemplate" ; cd "source/repos/MyProjTemplate" ; dotnet new projecttemplate-coree --PackageAuthor Me --name "MyProjTemplate" --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --output "src" --force ; git init ; cd "src" ; dotnet pack ; cd .. ; code -n . ; cd $HOME
```
Windows cmd (Sample useage):
```
dotnet new install Coree.Template.Project & cd /D %userprofile% & mkdir "source\repos\MyProjTemplate" & cd "source\repos\MyProjTemplate" & dotnet new projecttemplate-coree --PackageAuthor Me --name "MyProjTemplate" --SampleTemplateName "My Class library template" --SampleTemplateShortName "my template" --output "src" --force  & git init & cd "src" & dotnet pack & cd.. & code -n . & cd /D %userprofile%
```

## Educational

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

