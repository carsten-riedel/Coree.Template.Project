using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace __SourceName__.Tests
{
    [TestClass]
    public class FunctionalTests
    {
        [TestMethod]
        public void TestAddTask()
        {
            var addTask = new __SourceName__.AddTask { Param1 = 1, Param2 = 2 };

            var success = addTask.Execute();

            Assert.IsTrue(success);
            Assert.AreEqual(3, addTask.AddResult);
        }

        [TestMethod]
        public void TestTaskNodeTask()
        {
            var nodeFile = System.IO.Path.Combine("repo", "build", "Consumer.props");
            var task = new __SourceName__.TaskNodeTask
            {
                BuildEngine = new StubBuildEngine { ProjectFileOfTaskNode = nodeFile }
            };

            var success = task.Execute();

            Assert.IsTrue(success);
            Assert.AreEqual(nodeFile, task.TaskNodeFile);
            Assert.AreEqual(System.IO.Path.GetDirectoryName(nodeFile), task.TaskNodeDir);
        }

        [TestMethod]
        public void TestHomeTask()
        {
            var task = new __SourceName__.HomeTask
            {
                BuildEngine = new StubBuildEngine()
            };

            var success = task.Execute();

            var expected = System.Environment.GetEnvironmentVariable("USERPROFILE")
                ?? System.Environment.GetEnvironmentVariable("HOME");

            Assert.IsTrue(success);
            Assert.AreEqual(expected, task.Home);
        }

        [TestMethod]
        public void TestDumpEnvVarsTask()
        {
            var task = new __SourceName__.DumpEnvVarsTask
            {
                BuildEngine = new StubBuildEngine()
            };

            var success = task.Execute();

            Assert.IsTrue(success);
        }

        private sealed class StubBuildEngine : IBuildEngine
        {
            public bool ContinueOnError => false;

            public int LineNumberOfTaskNode => 1;

            public int ColumnNumberOfTaskNode => 1;

            public string ProjectFileOfTaskNode { get; set; } = string.Empty;

            public bool BuildProjectFile(
                string projectFileName,
                string[] targetNames,
                System.Collections.IDictionary globalProperties,
                System.Collections.IDictionary targetOutputs)
            {
                return false;
            }

            public void LogCustomEvent(CustomBuildEventArgs e)
            {
            }

            public void LogErrorEvent(BuildErrorEventArgs e)
            {
            }

            public void LogMessageEvent(BuildMessageEventArgs e)
            {
            }

            public void LogWarningEvent(BuildWarningEventArgs e)
            {
            }
        }
    }
}
