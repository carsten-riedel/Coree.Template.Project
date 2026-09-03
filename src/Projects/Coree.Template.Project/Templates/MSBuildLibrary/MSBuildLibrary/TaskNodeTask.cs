using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MSBuildLibrary
{
    public class TaskNodeTask : Microsoft.Build.Utilities.Task
    {
        [Output]
        public string? TaskNodeDir { get; set; }

        [Output]
        public string? TaskNodeFile { get; set; }

        public override bool Execute()
        {
            try
            {
                this.TaskNodeDir = System.IO.Path.GetDirectoryName(this.BuildEngine.ProjectFileOfTaskNode);
                this.TaskNodeFile = this.BuildEngine.ProjectFileOfTaskNode;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
            }

            return !Log.HasLoggedErrors;
        }
    }

}
