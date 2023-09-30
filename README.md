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

## Install/Uninstall
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

Windows:
```
md "MyMSBuildTask.Name" ; cd "MyMSBuildTask.Name" ; dotnet new msbuildlib --PackageAuthor Me
```




