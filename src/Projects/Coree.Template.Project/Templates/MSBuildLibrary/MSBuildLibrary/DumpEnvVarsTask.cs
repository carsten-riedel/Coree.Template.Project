using Microsoft.Build.Framework;
using System;
using System.Collections;

namespace MSBuildLibrary
{
    public class DumpEnvVarsTask : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry variable in environmentVariables)
            {
                Log.LogMessage(MessageImportance.High, $"{variable.Key} = {variable.Value}");
            }

            return true;
        }
    }
}