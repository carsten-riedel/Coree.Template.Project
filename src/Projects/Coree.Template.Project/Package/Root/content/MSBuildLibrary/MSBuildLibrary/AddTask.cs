using System;
using Microsoft.Build.Framework;

namespace MSBuildLibrary
{
    public class AddTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public int Param1 { get; set; }

        [Required]
        public int Param2 { get; set; }

        [Output]
        public int AddResult { get; set; }

        public override bool Execute()
        {
            try
            {
                AddResult = Param1 + Param2;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
            }
            return !Log.HasLoggedErrors;
        }
    }
}