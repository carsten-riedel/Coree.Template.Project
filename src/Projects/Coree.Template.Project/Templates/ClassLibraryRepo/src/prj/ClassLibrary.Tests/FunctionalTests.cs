using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace ClassLibrary.Tests
{
    [TestClass]
    public class FunctionalTests
    {

        [TestInitialize()]
        public void Startup()
        {

        }

        [TestMethod]
        public void TestFooMethod()
        {
            var result = ClassLibrary.Class1.Foo();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result);
        }
    }
}