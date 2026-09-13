using ClassLibrary;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ClassLibrary.Tests
{
    [TestClass]
    public class FunctionalTests
    {
        private const string EmDashSource = "class C { string s = \"a\u2014b\"; }";
        private const string QuoteSource = "class C { string s = \"x\u201Cy\"; }";

        [TestMethod]
        public async Task EmDashInStringReportsDiagnostic()
        {
            var expected = DiagnosticResult
                .CompilerWarning(EmDashAnalyzer.DiagnosticId)
                .WithSpan(1, 24, 1, 25);

            await CSharpAnalyzerVerifier<EmDashAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(EmDashSource, expected);
        }

        [TestMethod]
        public async Task AsciiHyphenReportsNoDiagnostic()
        {
            const string test = "class C { string s = \"a-b\"; }";
            await CSharpAnalyzerVerifier<EmDashAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task TypographicQuoteInStringReportsDiagnostic()
        {
            var expected = DiagnosticResult
                .CompilerWarning(SmartQuotesAnalyzer.DiagnosticId)
                .WithSpan(1, 24, 1, 25);

            await CSharpAnalyzerVerifier<SmartQuotesAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(QuoteSource, expected);
        }

        [TestMethod]
        public async Task AsciiQuotesReportNoDiagnostic()
        {
            const string test = "class C { string s = \"hello\"; }";
            await CSharpAnalyzerVerifier<SmartQuotesAnalyzer, DefaultVerifier>.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task EmDashSeverityErrorReportsError()
        {
            var expected = DiagnosticResult
                .CompilerError(EmDashAnalyzer.DiagnosticId)
                .WithSpan(1, 24, 1, 25);

            await VerifyWithSeverityAsync<EmDashAnalyzer>(
                EmDashSource,
                EmDashAnalyzer.SeverityPropertyName,
                "error",
                expected);
        }

        [TestMethod]
        public async Task EmDashSeverityOffReportsNothing()
        {
            await VerifyWithSeverityAsync<EmDashAnalyzer>(
                EmDashSource,
                EmDashAnalyzer.SeverityPropertyName,
                "off");
        }

        [TestMethod]
        public async Task SmartQuotesSeverityMessageReportsInfo()
        {
            var expected = new DiagnosticResult(SmartQuotesAnalyzer.DiagnosticId, DiagnosticSeverity.Info)
                .WithSpan(1, 24, 1, 25);

            await VerifyWithSeverityAsync<SmartQuotesAnalyzer>(
                QuoteSource,
                SmartQuotesAnalyzer.SeverityPropertyName,
                "message",
                expected);
        }

        [TestMethod]
        public async Task EmDashInMatchingAdditionalFileReportsDiagnostic()
        {
            var expected = DiagnosticResult
                .CompilerWarning(EmDashAnalyzer.DiagnosticId)
                .WithSpan("sample.txt", 1, 2, 1, 3);

            await VerifyWithAdditionalFileAsync<EmDashAnalyzer>(
                EmDashAnalyzer.AdditionalFilesPropertyName,
                "*.txt",
                "sample.txt",
                "a\u2014b",
                expected);
        }

        [TestMethod]
        public async Task SmartQuotesInMatchingAdditionalFileReportsDiagnostic()
        {
            var expected = DiagnosticResult
                .CompilerWarning(SmartQuotesAnalyzer.DiagnosticId)
                .WithSpan("sample.txt", 1, 2, 1, 3);

            await VerifyWithAdditionalFileAsync<SmartQuotesAnalyzer>(
                SmartQuotesAnalyzer.AdditionalFilesPropertyName,
                "*.txt",
                "sample.txt",
                "x\u201Cy",
                expected);
        }

        [TestMethod]
        public async Task AdditionalFileWithoutPropertyReportsNothing()
        {
            var test = new CSharpAnalyzerTest<EmDashAnalyzer, DefaultVerifier>
            {
                TestCode = "class C { }",
            };
            test.TestState.AdditionalFiles.Add(("sample.txt", "a\u2014b"));
            await test.RunAsync();
        }

        [TestMethod]
        public async Task AdditionalFileNotMatchingGlobReportsNothing()
        {
            await VerifyWithAdditionalFileAsync<EmDashAnalyzer>(
                EmDashAnalyzer.AdditionalFilesPropertyName,
                "*.csproj",
                "sample.txt",
                "a\u2014b");
        }

        [TestMethod]
        public async Task EmDashInMatchingCsprojAdditionalFileReportsDiagnostic()
        {
            var expected = DiagnosticResult
                .CompilerWarning(EmDashAnalyzer.DiagnosticId)
                .WithSpan("sample.csproj", 1, 6, 1, 7);

            await VerifyWithAdditionalFileAsync<EmDashAnalyzer>(
                EmDashAnalyzer.AdditionalFilesPropertyName,
                "*.txt|*.csproj",
                "sample.csproj",
                "<!-- \u2014 -->",
                expected);
        }

        [TestMethod]
        public async Task EmptyAdditionalFilesPropertyReportsNothing()
        {
            await VerifyWithAdditionalFileAsync<EmDashAnalyzer>(
                EmDashAnalyzer.AdditionalFilesPropertyName,
                string.Empty,
                "sample.txt",
                "a\u2014b");
        }

        private static async Task VerifyWithSeverityAsync<TAnalyzer>(
            string source,
            string propertyName,
            string severity,
            params DiagnosticResult[] expected)
            where TAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
            {
                TestCode = source,
            };
            test.ExpectedDiagnostics.AddRange(expected);
            test.TestState.AnalyzerConfigFiles.Add((
                "/.globalconfig",
                "is_global = true\nbuild_property." + propertyName + " = " + severity + "\n"));
            await test.RunAsync();
        }

        private static async Task VerifyWithAdditionalFileAsync<TAnalyzer>(
            string propertyName,
            string patterns,
            string additionalPath,
            string additionalContent,
            params DiagnosticResult[] expected)
            where TAnalyzer : Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer, new()
        {
            var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
            {
                TestCode = "class C { }",
            };
            test.ExpectedDiagnostics.AddRange(expected);
            test.TestState.AdditionalFiles.Add((additionalPath, additionalContent));
            test.TestState.AnalyzerConfigFiles.Add((
                "/.globalconfig",
                "is_global = true\nbuild_property." + propertyName + " = " + patterns + "\n"));
            await test.RunAsync();
        }
    }
}
