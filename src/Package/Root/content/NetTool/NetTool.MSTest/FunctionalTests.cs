using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace NetTool.MSTest
{
    [TestClass]
    public class FunctionalTests
    {

        [TestInitialize()]
        public void Startup()
        {

        }

        [TestMethod]
        public void ConsoleProgramMainTest()
        {
            StringWriter @out = new StringWriter();
            Console.SetOut(@out);

            string[] inputs = new string[] {
                "Somebody",
                "99"
            };  
            Console.SetIn(new StringReader(String.Join(Environment.NewLine, inputs)));

            Program.Main(Array.Empty<string>());

            string? output = @out.ToString();

            string expectedOutput = String.Join(Environment.NewLine, new string[] {
                "What's your name?",
                "How old are you?",
                "Hello Somebody, you are 99 years old!!"
            }) + Environment.NewLine;

            Assert.AreEqual(output, expectedOutput); 
        }
    }
}