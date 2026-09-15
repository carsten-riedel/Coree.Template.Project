#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;

#endif
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

        [TestMethod]
        public void Form1_Constructs()
        {
            using var form = new __SourceName__.Form1();
            Assert.AreEqual("Form1", form.Text);
        }
    }
}
