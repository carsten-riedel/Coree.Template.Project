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
        public async System.Threading.Tasks.Task TestMainMethod()
        {
            var result = await __SourceName__.Program.Main(System.Array.Empty<string>());

            Assert.AreEqual(0, result);
        }
    }
}
