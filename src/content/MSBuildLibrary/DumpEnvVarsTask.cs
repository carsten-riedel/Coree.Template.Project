using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

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
