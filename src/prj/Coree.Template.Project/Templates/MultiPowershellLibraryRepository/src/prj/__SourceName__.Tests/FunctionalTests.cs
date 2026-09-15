using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace __SourceName__.Tests
{
    [TestClass]
    public sealed class FunctionalTests
    {
        [TestMethod]
        public void FormatterReturnsExpectedValue()
        {
            Assert.AreEqual("Value: test", SampleValueFormatter.Format("test"));
        }

        [TestMethod]
        public void ModuleImportsInPowerShellCore()
        {
            string moduleRoot = GetModuleRoot();
            if (!HasAnyTarget(moduleRoot, "net10.0", "net8.0", "netstandard2.0"))
            {
                Assert.Inconclusive("No PowerShell Core-compatible target was selected.");
            }

            AssertHostReturnsExpectedValue("pwsh.exe", moduleRoot, includeExecutionPolicy: false);
        }

        [TestMethod]
        public void ModuleImportsInWindowsPowerShell()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Inconclusive("Windows PowerShell is available only on Windows.");
            }

            string moduleRoot = GetModuleRoot();
            if (!HasAnyTarget(moduleRoot, "net48", "net462", "netstandard2.0"))
            {
                Assert.Inconclusive("No Windows PowerShell-compatible target was selected.");
            }

            AssertHostReturnsExpectedValue("powershell.exe", moduleRoot, includeExecutionPolicy: true);
        }

        private static string GetModuleRoot()
        {
            var targetDirectory = new DirectoryInfo(AppContext.BaseDirectory);
            string configuration = targetDirectory.Parent?.Name ?? "Debug";
            string projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "__SourceName__"));
            string moduleRoot = Path.Combine(projectDirectory, "bin", "Module", configuration, "__SourceName__");
            Assert.IsTrue(Directory.Exists(moduleRoot), $"The staged module was not found: {moduleRoot}");
            return moduleRoot;
        }

        private static bool HasAnyTarget(string moduleRoot, params string[] targetFrameworks)
        {
            foreach (string targetFramework in targetFrameworks)
            {
                if (File.Exists(Path.Combine(moduleRoot, targetFramework, "__SourceName__.dll")))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AssertHostReturnsExpectedValue(string executable, string moduleRoot, bool includeExecutionPolicy)
        {
            string manifestPath = Path.Combine(moduleRoot, "__SourceName__.psd1").Replace("'", "''");
            string script = $"$ErrorActionPreference='Stop'; Import-Module -Force '{manifestPath}'; Get-SampleValue -InputObject 'test'";
            string executionPolicy = includeExecutionPolicy ? " -ExecutionPolicy Bypass" : string.Empty;

            var startInfo = new ProcessStartInfo
            {
                FileName = executable,
                Arguments = $"-NoLogo -NoProfile -NonInteractive{executionPolicy} -Command \"& {{ {script} }}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            Process? process;
            try
            {
                process = Process.Start(startInfo);
            }
            catch (Win32Exception)
            {
                Assert.Inconclusive($"{executable} is not installed or is not on PATH.");
                return;
            }

            Assert.IsNotNull(process);
            using (process)
            {
                string standardOutput = process.StandardOutput.ReadToEnd();
                string standardError = process.StandardError.ReadToEnd();
                Assert.IsTrue(process.WaitForExit(30_000), $"{executable} did not exit within 30 seconds.");
                Assert.AreEqual(0, process.ExitCode, standardError);
                Assert.AreEqual("Value: test", standardOutput.Trim());
            }
        }
    }
}
