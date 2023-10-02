using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;


namespace MSBuildLibrary
{
    public class ConcatTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string Parameter1 { get; set; }

        [Required]
        public string Parameter2 { get; set; }

        [Output]
        public string Result { get; set; }

        public override bool Execute()
        {
            try
            {            
                Log.LogMessage(subcategory: null,code: null,helpKeyword: null, file: null,lineNumber: 0, columnNumber: 0,endLineNumber: 0, endColumnNumber: 0, importance: MessageImportance.High, message: "logmessage {0}", messageArgs: Parameter1);
                Result = $@"{Parameter1}{Parameter2}";
                Log.LogMessage(MessageImportance.High, $@"ConcatTask Result: {Result}");
            }
            catch (Exception ex)
            {
                //This logging helper method is designed to capture and display information from arbitrary exceptions in a standard way.
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
                Log.LogErrorFromException(ex, showStackTrace: true);
            }
            return !Log.HasLoggedErrors;
        }
    }

    public class DumpEnvironmentVariablesTask : Microsoft.Build.Utilities.Task
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


}
