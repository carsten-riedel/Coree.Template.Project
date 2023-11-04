using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MSBuildLibrary.MSTest
{
    [TestClass]
    public class IntegrationTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Process buildProcess;
        private List<string> output;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize()]
        public void Startup()
        {
            output = new List<string>();
            buildProcess = new Process();
            buildProcess.StartInfo.FileName = "dotnet";
            buildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            buildProcess.StartInfo.CreateNoWindow = true;
            buildProcess.StartInfo.RedirectStandardOutput = true;
        }

        [TestCleanup()]
        public void Cleanup()
        {
            buildProcess.Close();
        }

        [TestMethod]
        public void InvokeAddTaskMsbuild()
        {
            //Arrage
            buildProcess.StartInfo.Arguments = "build .\\Resources\\TestScript.msbuild --nologo -nodeReuse:false /t:RunAddTask";

            //Act
            ExecuteCommandAndCollectResults();
            
            var x = output.First().TrimStart();

            //Assert
            Assert.AreEqual(x, "The sum is: 8");
            Assert.AreEqual(0, buildProcess.ExitCode);
        }

        private void ExecuteCommandAndCollectResults()
        {
            buildProcess.Start();
            while (!buildProcess.StandardOutput.EndOfStream)
            {
                output.Add(buildProcess.StandardOutput.ReadLine() ?? string.Empty);
            }
            buildProcess.WaitForExit();
        }
    }
}