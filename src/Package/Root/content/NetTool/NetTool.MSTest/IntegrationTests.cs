using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace NetTool.MSTest
{
     [TestClass]
    public class IntegrationTests
    {
        StringBuilder? sb;

        [TestInitialize()]
        public void Startup()
        {
            sb = new StringBuilder();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            sb!.Clear();
        }

        [TestMethod]
        public void ConsoleProcess()
        {
            // Assuming 'SomeType' is a type from the referenced project
            Type typeFromReferencedAssembly = typeof(Program);

            // Get the Assembly object
            Assembly referencedAssembly = typeFromReferencedAssembly.Assembly;

            // Get the assembly name
            string assemblyName = referencedAssembly.GetName().Name!;

            var startInfo = new ProcessStartInfo($"{assemblyName}.exe");
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var process = new Process();
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += Proc_OutputDataReceived;
            process.ErrorDataReceived += Proc_ErrorDataReceived;
            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.StandardInput.WriteLine("Somebody");
            process.StandardInput.WriteLine("99");
            process.WaitForExit();
            var output = sb.ToString();

            string expectedOutput = String.Join(Environment.NewLine, new string[] {
                "What's your name?",
                "How old are you?",
                "Hello Somebody, you are 99 years old!!"
            }) + Environment.NewLine;

            Assert.AreEqual(output, expectedOutput);
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                sb.AppendSafeWithNewLine(e.Data);
            }
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                sb.AppendSafeWithNewLine(e.Data);
            }
        }
    }
}