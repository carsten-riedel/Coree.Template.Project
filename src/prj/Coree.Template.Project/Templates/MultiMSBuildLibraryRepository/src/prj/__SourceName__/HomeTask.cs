using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;

namespace __SourceName__
{
    /// <summary>
    /// Sample MSBuild task: resolves the user home directory.
    /// OS-specific helpers use <see cref="ExcludeFromCodeCoverageAttribute"/> so Coverlet 100% does not require every platform branch in one testhost.
    /// Replace this type with your own task, or keep it as a warning-code example.
    /// </summary>
    public class HomeTask : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// User home directory (<c>USERPROFILE</c> on Windows, <c>HOME</c> on Unix).
        /// </summary>
        [Output]
        public string? Home { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            Home = ResolveHome();
            WarnIfHomeMissing();
            return !Log.HasLoggedErrors;
        }

        // One testhost cannot execute every OS branch. Execute() stays in Coverlet.
        [ExcludeFromCodeCoverage]
        private static string? ResolveHome()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return System.Environment.GetEnvironmentVariable("USERPROFILE");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return System.Environment.GetEnvironmentVariable("HOME");
            }

            return null;
        }

        // Testhost normally has a profile directory, so this warning path is not hit.
        [ExcludeFromCodeCoverage]
        private void WarnIfHomeMissing()
        {
            if (Home != null)
            {
                return;
            }

            Log.LogWarning(
                subcategory: null,
                warningCode: "HomeTask0001",
                helpKeyword: null,
                file: null,
                lineNumber: 0,
                columnNumber: 0,
                endLineNumber: 0,
                endColumnNumber: 0,
                message: "Home directory environment variable was not set (USERPROFILE on Windows, HOME on Unix).");
        }
    }
}
