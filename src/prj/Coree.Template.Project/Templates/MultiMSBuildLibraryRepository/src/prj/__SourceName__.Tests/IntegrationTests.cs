using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace __SourceName__.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private Process? _buildProcess;
        private List<string> _output = new List<string>();

        [TestInitialize]
        public void Startup()
        {
            _output = new List<string>();
            _buildProcess = new Process();
            _buildProcess.StartInfo.FileName = "dotnet";
            _buildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _buildProcess.StartInfo.CreateNoWindow = true;
            _buildProcess.StartInfo.RedirectStandardOutput = true;
            _buildProcess.StartInfo.UseShellExecute = false;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _buildProcess?.Dispose();
        }

        [TestMethod]
        public void InvokeAddTaskMsbuild()
        {
            var script = Path.Combine(AppContext.BaseDirectory, "Resources", "TestScript.msbuild");
            _buildProcess!.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
            _buildProcess.StartInfo.Arguments = "build \"" + script + "\" --nologo -nodeReuse:false -t:RunAddTask";

            ExecuteCommandAndCollectResults();

            string? sumLine = null;
            foreach (var line in _output)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("The sum is:", StringComparison.Ordinal))
                {
                    sumLine = trimmed;
                    break;
                }
            }

            Assert.IsNotNull(sumLine);
            Assert.AreEqual("The sum is: 8", sumLine);

            string? nodeLine = null;
            foreach (var line in _output)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("Task node:", StringComparison.Ordinal))
                {
                    nodeLine = trimmed;
                    break;
                }
            }

            Assert.IsNotNull(nodeLine);
            Assert.IsTrue(nodeLine.IndexOf("TestScript.msbuild", StringComparison.OrdinalIgnoreCase) >= 0);

            string? homeLine = null;
            foreach (var line in _output)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("Home:", StringComparison.Ordinal))
                {
                    homeLine = trimmed;
                    break;
                }
            }

            Assert.IsNotNull(homeLine);
            Assert.IsTrue(homeLine.Length > "Home: ".Length);

            string? envLine = null;
            foreach (var line in _output)
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("Env: ", StringComparison.Ordinal))
                {
                    envLine = trimmed;
                    break;
                }
            }

            Assert.IsNotNull(envLine);
            Assert.AreEqual(0, _buildProcess.ExitCode);
        }

        private void ExecuteCommandAndCollectResults()
        {
            _buildProcess!.Start();
            var stdout = _buildProcess.StandardOutput.ReadToEnd();
            _buildProcess.WaitForExit();
            foreach (var line in stdout.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                _output.Add(line);
            }
        }
    }
}
