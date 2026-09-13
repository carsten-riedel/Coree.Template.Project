using ClassLibrary;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass]
    public class SourceCharacterScannerTests
    {
        [TestMethod]
        public void OrEmptyReplacesNullWithEmptyText()
        {
            Assert.AreEqual(0, SourceCharacterScanner.OrEmpty(null).Length);
            var text = SourceText.From("x");
            Assert.AreSame(text, SourceCharacterScanner.OrEmpty(text));
        }
    }
}
