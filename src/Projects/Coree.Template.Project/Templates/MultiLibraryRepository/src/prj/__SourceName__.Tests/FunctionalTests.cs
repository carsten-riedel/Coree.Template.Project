using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace __SourceName__.Tests
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
            var result = __SourceName__.Class1.Foo();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result);
        }
    }
}