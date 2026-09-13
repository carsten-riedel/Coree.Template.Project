using ClassLibrary;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ClassLibrary.Tests
{
    [TestClass]
    public class FunctionalTests
    {
        [TestMethod]
        public async Task TypeNameWithLowercaseReportsDiagnostic()
        {
            const string test = "class lowercase { }";
            var expected = DiagnosticResult
                .CompilerWarning(SampleAnalyzer.DiagnosticId)
                .WithSpan(1, 7, 1, 16)
                .WithArguments("lowercase");

            await CSharpAnalyzerVerifier<SampleAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(test, expected);
        }

        [TestMethod]
        public async Task TypeNameAllUppercaseReportsNoDiagnostic()
        {
            const string test = "class UPPER { }";
            await CSharpAnalyzerVerifier<SampleAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(test);
        }
    }
}
