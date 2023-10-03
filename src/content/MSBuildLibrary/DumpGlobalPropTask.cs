using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MSBuildLibrary
{

    public class DumpGlobalPropTask : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {

            IReadOnlyDictionary<string, string> environmentVariables = this.BuildEngine.GetDicProjectProperties();
            foreach (KeyValuePair<string, string> variable in environmentVariables)
            {
                Log.LogMessage(MessageImportance.High, $"GlobalProperties: {variable.Key} = {variable.Value}");
            }

            return true;
        }
    }

}