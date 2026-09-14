using __SourceName__;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace __SourceName__.Tests
{
    [TestClass]
    public class AnalyzerSeverityTests
    {
        [TestMethod]
        public void ParseUnsetOrUnknownIsWarning()
        {
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse(null));
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse(string.Empty));
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse("  "));
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse("warning"));
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse("WARNING"));
            Assert.AreEqual(DiagnosticSeverity.Warning, AnalyzerSeverity.Parse("nope"));
        }

        [TestMethod]
        public void ParseErrorInfoAndOff()
        {
            Assert.AreEqual(DiagnosticSeverity.Error, AnalyzerSeverity.Parse("error"));
            Assert.AreEqual(DiagnosticSeverity.Info, AnalyzerSeverity.Parse("message"));
            Assert.AreEqual(DiagnosticSeverity.Info, AnalyzerSeverity.Parse("information"));
            Assert.AreEqual(DiagnosticSeverity.Info, AnalyzerSeverity.Parse("info"));
            Assert.IsNull(AnalyzerSeverity.Parse("off"));
            Assert.IsNull(AnalyzerSeverity.Parse("none"));
            Assert.IsNull(AnalyzerSeverity.Parse("silent"));
        }

        [TestMethod]
        public void WithSeverityReusesWarningAndClonesOthers()
        {
            var warning = new DiagnosticDescriptor(
                "X000",
                "Title",
                "Message",
                "Typography",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);
            Assert.AreSame(warning, AnalyzerSeverity.WithSeverity(warning, DiagnosticSeverity.Warning));

            var error = AnalyzerSeverity.WithSeverity(warning, DiagnosticSeverity.Error);
            Assert.AreNotSame(warning, error);
            Assert.AreEqual(DiagnosticSeverity.Error, error.DefaultSeverity);
            Assert.AreEqual("X000", error.Id);

            var info = AnalyzerSeverity.WithSeverity(warning, DiagnosticSeverity.Info);
            Assert.AreEqual(DiagnosticSeverity.Info, info.DefaultSeverity);
        }

        [TestMethod]
        public void SelectDescriptorMapsSeverities()
        {
            var warning = new DiagnosticDescriptor(
                "X000",
                "Title",
                "Message",
                "Typography",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true);
            var error = AnalyzerSeverity.WithSeverity(warning, DiagnosticSeverity.Error);
            var info = AnalyzerSeverity.WithSeverity(warning, DiagnosticSeverity.Info);

            Assert.AreSame(error, AnalyzerSeverity.SelectDescriptor(warning, error, info, DiagnosticSeverity.Error));
            Assert.AreSame(info, AnalyzerSeverity.SelectDescriptor(warning, error, info, DiagnosticSeverity.Info));
            Assert.AreSame(warning, AnalyzerSeverity.SelectDescriptor(warning, error, info, DiagnosticSeverity.Warning));
            Assert.AreSame(warning, AnalyzerSeverity.SelectDescriptor(warning, error, info, DiagnosticSeverity.Hidden));
        }
    }
}
