using Moq;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MSBuildLibrary.MSTest
{
    [TestClass]
    public class FunctionalTests
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Mock<IBuildEngine> buildEngine;
        private List<BuildErrorEventArgs> errors;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize()]
        public void Startup()
        {
            buildEngine = new Mock<IBuildEngine>();
            errors = new List<BuildErrorEventArgs>();
            buildEngine.Setup(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>())).Callback<BuildErrorEventArgs>(e => errors.Add(e));
        }

        [TestMethod]
        public void TestAddTask()
        {
            //Arrange
            var addTask = new AddTask { Param1 = 1, Param2 = 2 };
            addTask.BuildEngine = buildEngine.Object;

            //Act
            var success = addTask.Execute();

            //Trace
            foreach (var error in errors)
            {
                Trace.WriteLine(error.Message);
            }

            //Assert
            Assert.IsTrue(success);
            Assert.AreEqual(0, errors.Count);
            Assert.AreEqual(3, addTask.AddResult);
        }
    }
}