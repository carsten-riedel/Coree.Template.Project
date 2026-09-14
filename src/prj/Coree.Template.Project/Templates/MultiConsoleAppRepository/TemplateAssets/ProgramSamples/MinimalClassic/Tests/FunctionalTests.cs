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
        public void TestMainMethod()
        {
            var result = __SourceName__.Program.Main(System.Array.Empty<string>());

            Assert.AreEqual(0, result);
        }
    }
}
