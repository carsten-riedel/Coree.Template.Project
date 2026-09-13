using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass]
    public class AdditionalFilePatternsTests
    {
        [TestMethod]
        public void MatchesRejectsMissingPathOrPatterns()
        {
            Assert.IsFalse(AdditionalFilePatterns.Matches(null, "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.Matches(string.Empty, "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.Matches("  ", "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.Matches("a.txt", null));
            Assert.IsFalse(AdditionalFilePatterns.Matches("a.txt", string.Empty));
            Assert.IsFalse(AdditionalFilePatterns.Matches("a.txt", "  "));
            Assert.IsFalse(AdditionalFilePatterns.Matches("a.txt", " ; ; "));
        }

        [TestMethod]
        public void MatchesGlobsAndExactNames()
        {
            Assert.IsTrue(AdditionalFilePatterns.Matches(@"C:\proj\notes.txt", "*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.Matches(@"C:\proj\notes.TXT", "*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.Matches(@"C:\proj\Host.csproj", "*.txt;*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.Matches(@"C:\proj\Host.csproj", "*.txt|*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.Matches(@"C:\proj\Host.csproj", "*.txt,*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.Matches("notes.txt", " *.md ; *.txt "));
            Assert.IsTrue(AdditionalFilePatterns.Matches("notes.txt", "notes.txt"));
            Assert.IsTrue(AdditionalFilePatterns.Matches("notes.txt", "*"));
            Assert.IsTrue(AdditionalFilePatterns.Matches("notes.txt", "*.*"));
            Assert.IsFalse(AdditionalFilePatterns.Matches("notes.md", "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.Matches("notes.txt", "other.txt"));
        }
    }
}
