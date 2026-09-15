using Microsoft.Build.Framework;

namespace __SourceName__
{
    /// <summary>
    /// Sample diagnostic task: logs every process environment variable at high importance.
    /// PackageReference only registers <c>UsingTask</c>; the dump runs when a target invokes <c>DumpEnvVarsTask</c>.
    /// Use it to inspect which variables a host actually has, including ones that appeared unexpectedly.
    /// </summary>
    public class DumpEnvVarsTask : Microsoft.Build.Utilities.Task
    {
        /// <inheritdoc />
        public override bool Execute()
        {
            foreach (System.Collections.DictionaryEntry variable in System.Environment.GetEnvironmentVariables())
            {
                Log.LogMessage(MessageImportance.High, "Env: " + variable.Key + " = " + variable.Value);
            }

            return true;
        }
    }
}
