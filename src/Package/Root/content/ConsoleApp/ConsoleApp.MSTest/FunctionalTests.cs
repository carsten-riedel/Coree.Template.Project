using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleApp.MSTest
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

            //Prepare answers for Console.Readline
            string[] inputs = new string[] {
                "Somebody",
            };
            //Set answers for Console.Readline
            Console.SetIn(new StringReader(String.Join(Environment.NewLine, inputs)));

            var param = new string[] { "parameter" };
            Program.Main(param);

            string? output = @out.ToString();

            string expectedOutput = String.Join(Environment.NewLine, new string[] {
                $@"Hello, World! {param[0]} {inputs[0]} {ConsoleApp.Library.Class1.Foo()}",
            }) + Environment.NewLine;

            Assert.AreEqual(output, expectedOutput); 
        }
    }
}

