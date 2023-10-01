using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;


namespace MSBuildLibrary35
{

    /*
     	<UsingTask TaskName="FooTask" AssemblyFile="$(MSBuildThisFileDirectory)$(OutputPath)MSBuildLibrary35.dll"/>

	    <Target Name="FooTaskTarget" AfterTargets="Build">
		    <FooTask Parameter1="abc">
			    <Output TaskParameter="Result1" PropertyName="SettingClassFileName" />
		    </FooTask>
		    <Message Text="Hello foo $(SettingClassFileName)" Importance="high" />
	    </Target>
    */

    //https://learn.microsoft.com/en-us/visualstudio/msbuild/task-writing?view=vs-2022

    public class FooTask : Microsoft.Build.Utilities.Task
    {
 

        //The name of the class which is going to be generated
        [Required]
        public string Parameter1 { get; set; }

        // The filename where the class was generated
        [Output]
        public string Result1 { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(subcategory: null,code: null,helpKeyword: null, file: null,lineNumber: 0, columnNumber: 0,endLineNumber: 0, endColumnNumber: 0, importance: MessageImportance.High, message: "logmessage {0}", messageArgs: Parameter1);
            Log.LogMessage(MessageImportance.High, $@"pa {Parameter1}");

            Result1 = "fooresult1";
    
            try
            {
                /*
                Log.LogError(subcategory: null,
                                     errorCode: "APPS0001",
                                     helpKeyword: null,
                                     file: null,
                                     lineNumber: 0,
                                     columnNumber: 0,
                                     endLineNumber: 0,
                                     endColumnNumber: 0,
                                     message: $@"Incorrect line format. {Parameter1}");
                */
            }
            catch (Exception ex)
            {
                //This logging helper method is designed to capture and display information from arbitrary exceptions in a standard way.
                Log.LogErrorFromException(ex, showStackTrace: true);
            }
            return !Log.HasLoggedErrors;
        }
    }

    public class SimpleTask : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {

            // Logging all environment variables
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry variable in environmentVariables)
            {
                Log.LogMessage(MessageImportance.High, $"{variable.Key} = {variable.Value}");
            }

            return true;
        }
    }

    public class MyTask : Microsoft.Build.Utilities.Task
    {
        // The filename where the class was generated
        [Output]
        public string Result1 { get; set; }

        public override bool Execute()
        {
            string currentProjectPath = this.BuildEngine.ProjectFileOfTaskNode;
            var projectCollection = new ProjectCollection();
            var project = new Project(currentProjectPath, null, null, projectCollection);

            // Log some project properties
            Log.LogMessage(MessageImportance.High, $"Project File: {currentProjectPath}");
            Log.LogMessage(MessageImportance.High, $"Target Framework: {project.GetPropertyValue("TargetFramework")}");
            Log.LogMessage(MessageImportance.High, $"Output Path: {project.GetPropertyValue("OutputPath")}");
            project.SetProperty("foox", "fooxvalue");
            project.SetGlobalProperty("far", "far");
            this.Result1 = project.GetPropertyValue("TargetFramework");
            return true;
        }
    }

    public class CollectPropertiesTask : Microsoft.Build.Utilities.Task
    {
        [Output]
        public ITaskItem[] AllProperties { get; set; }

        public override bool Execute()
        {
            var properties = new List<ITaskItem>();

            Project project = new Project(BuildEngine.ProjectFileOfTaskNode);

            foreach (var item in project.GlobalProperties)
            {
                //Log.LogMessage(MessageImportance.High, $@"sssssssss {item.Key}");
            }

 
            if (project != null)
            {
                foreach (var prop in project.AllEvaluatedProperties)
                {
                    var taskItem = new TaskItem(prop.Name);
                    taskItem.SetMetadata("Value", prop.EvaluatedValue);
                    properties.Add(taskItem);
                }
            }

            project.Save(@"C:\temp\cc.csproj");

            AllProperties = properties.ToArray();
            return true;
        }
    }

    /*
    <!-- 
    <ItemGroup>
        <PackageReference Include="Microsoft.Build" Version="15.9.20" />
        <PackageReference Include="Microsoft.Build.Utilities.Core" Version="15.9.20" />
    </ItemGroup>

	<Target Name = "out" AfterTargets="Build">
		<Message Text = "out: $(MSBuildThisFileDirectory)MSBuildLibrary35.dll" Importance="high" />
	</Target>

	<UsingTask TaskName = "FooTask" AssemblyFile="$(MSBuildThisFileDirectory)$(OutputPath)MSBuildLibrary35.dll"/>
	<UsingTask TaskName = "SimpleTask" AssemblyFile="$(MSBuildThisFileDirectory)$(OutputPath)MSBuildLibrary35.dll"/>
	<UsingTask TaskName = "MyTask" AssemblyFile="$(MSBuildThisFileDirectory)$(OutputPath)MSBuildLibrary35.dll"/>
	<UsingTask TaskName = "CollectPropertiesTask" AssemblyFile="$(MSBuildThisFileDirectory)$(OutputPath)MSBuildLibrary35.dll"/>

	<Target Name = "FooTaskTarget" AfterTargets="Build">
		<FooTask Parameter1 = "abc">

            <Output TaskParameter="Result1" PropertyName="SettingClassFileName" />
		</FooTask>
		
		<Message Text = "Hello foo $(SettingClassFileName)" Importance="high" />
	</Target>

	<Target Name = "UseSimpleTask" AfterTargets="Build">
		<SimpleTask/>
	</Target>

	<Target Name = "UseMyTask" AfterTargets="Build">
		<MyTask><Output TaskParameter = "Result1" PropertyName="SetMe" /></MyTask>
		<Message Text = "SetMe: $(SetMe)" Importance="high" />
	</Target>

	<Target Name = "UseCollectPropertiesTask" AfterTargets="Build">
		<CollectPropertiesTask>
			<Output TaskParameter = "AllProperties" ItemName="CapturedProperties" />
		</CollectPropertiesTask>

		<Message Text = "%(CapturedProperties.Identity) = %(CapturedProperties.Value)" Importance="high"/>
	</Target>

	<Target Name = "CustomAfterBuildTarget" AfterTargets="Build">
		<Message Text = "Hello from CustomAfterBuildTarget" Importance="high" />
		<Message Text = "MSBuildThisFile:                $(MSBuildThisFile)" Importance="high" />
		<Message Text = "MSBuildThisFileDirectory:       $(MSBuildThisFileDirectory)" Importance="high" />
		<Message Text = "MSBuildThisFileDirectoryNoRoot: $(MSBuildThisFileDirectoryNoRoot)" Importance="high" />
		<Message Text = "MSBuildThisFileExtension:       $(MSBuildThisFileExtension)" Importance="high" />
		<Message Text = "MSBuildThisFileFullPath:        $(MSBuildThisFileFullPath)" Importance="high" />
		<Message Text = "MSBuildThisFileName:            $(MSBuildThisFileName)" Importance="high" />

		<Message Text = "BinFolder:						$(BinFolder)" Importance="high" />
		<Message Text = "DestinationFolder:				$(DestinationFolder)" Importance="high" />
		<Message Text = "OutDir:							$(OutDir)" Importance="high" />
		<Message Text = "OutputPath:						$(OutputPath)" Importance="high" />
		<Message Text = "BaseOutputPath:					$(BaseOutputPath)" Importance="high" />
		<Message Text = "PublishDir:						$(PublishDir)" Importance="high" />

	</Target>
-->	
    */
}
