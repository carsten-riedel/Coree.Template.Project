using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace __SourceName__.Tests
{
    [TestClass]
    public class FunctionalTests
    {

        [TestInitialize()]
        public void Startup()
        {

        }

        [STATestMethod]
        public void MainWindow_Constructs()
        {
            var window = new __SourceName__.MainWindow();
            Assert.AreEqual("MainWindow", window.Title);
            window.Close();
        }
    }
}
