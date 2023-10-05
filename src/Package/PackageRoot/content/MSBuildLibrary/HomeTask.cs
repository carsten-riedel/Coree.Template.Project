using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MSBuildLibrary
{

    public class HomeTask : Microsoft.Build.Utilities.Task
    {
        [Output]
        public string Home { get; set; }

        public override bool Execute()
        {
            try
            {   
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Home = Environment.GetEnvironmentVariable("USERPROFILE");
                    if (Home == null)
                    {
                        Log.LogWarning(subcategory: null, warningCode: "HomeTask0001", helpKeyword: null, file: null, lineNumber: 0, columnNumber: 0, endLineNumber: 0, endColumnNumber: 0, message: "HomeTask: USERPROFILE could not be resolved. Home Output is not been set.");
                    }
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Home = Environment.GetEnvironmentVariable("HOME");
                    if (Home == null)
                    {
                        Log.LogWarning(subcategory: null, warningCode: "HomeTask0001", helpKeyword: null, file: null, lineNumber: 0, columnNumber: 0, endLineNumber: 0, endColumnNumber: 0, message: "HomeTask: USERPROFILE could not be resolved. Home Output is not been set.");
                    }
                }
                else
                {
                    throw new NotSupportedException("Unknown operating system.");
                }
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
            }

            return !Log.HasLoggedErrors;
        }
    }

}
