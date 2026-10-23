using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace __SourceName__.Tests
{
    [TestClass]
    public class AdditionalFilePatternsTests
    {
        [TestMethod]
        public void IsSelectedRejectsMissingPathOrIncludes()
        {
            Assert.IsFalse(AdditionalFilePatterns.IsSelected(null, "*.txt", null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected(string.Empty, "*.txt", null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected("  ", "*.txt", null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected("a.txt", null, null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected("a.txt", string.Empty, null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected("a.txt", "  ", null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected("a.txt", " ; ; ", null, null));
        }

        [TestMethod]
        public void IsMatchSupportsStarQuestionAndExactNames()
        {
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.txt", "*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.TXT", "*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.txt", "notes.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.txt", "*"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.txt", "*.*"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("a.txt", "?.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("ab.txt", "a?.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("notes.md", "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("notes.txt", "other.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("ab.txt", "?.md"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("a/b", "a?b"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("ab", "ab?"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("abc", "ab"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("file", "**"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("a/b.txt", "?"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch(null, "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("a.txt", null));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch(null, null));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("foo.md", "**/*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("a/b.md", "**/*.txt"));
            Assert.AreEqual("C:/projx/x", AdditionalFilePatterns.ToRelativePath(@"C:\projx\x", @"C:\proj"));
        }

        [TestMethod]
        public void IsMatchTreatsStarAsSingleSegmentAndDoubleStarAsRecursive()
        {
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("sub/notes.txt", "*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("notes.txt", "**/*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("sub/notes.txt", "**/*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("a/b/notes.txt", "**/*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("sub/notes.txt", "sub/*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("sub/deep/notes.txt", "sub/*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("sub/deep/notes.txt", "sub/**/*.txt"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch("dir/file", "**"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("dir/file", "*"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch("notes.txt", "other/**/*.txt"));
        }

        [TestMethod]
        [Timeout(10_000)]
        public void IsMatchHandlesDeepNonmatchingRecursivePatterns()
        {
            var directoryPath = string.Join("/", Enumerable.Repeat("nested", 64));

            Assert.IsFalse(AdditionalFilePatterns.IsMatch(directoryPath + "/notes.md", "**/*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.IsMatch(directoryPath + "/notes.md", "**/missing/*.md"));
            Assert.IsTrue(AdditionalFilePatterns.IsMatch(directoryPath + "/notes.txt", "**/*.txt"));
        }

        [TestMethod]
        [DataRow("target/notes.txt", "**/target/**/*.txt", true)]
        [DataRow("root/target/notes.txt", "**/target/**/*.txt", true)]
        [DataRow("root/target/deep/notes.txt", "**/target/**/*.txt", true)]
        [DataRow("root/other/deep/notes.txt", "**/target/**/*.txt", false)]
        [DataRow("root/target/deep/notes.md", "**/target/**/*.txt", false)]
        [DataRow("root/target/deep/notes.txt", "root/**/target/**/notes.txt", true)]
        [DataRow("root/target/deep/notes.md", "root/**/target/**/notes.txt", false)]
        [DataRow("root/target/deep/notes.txt", "**/**/notes.txt", true)]
        [DataRow("root/target/deep/notes.md", "**/**/notes.txt", false)]
        [DataRow(@"root\target\deep\NOTES.TXT", @"**\target\**\notes.txt", true)]
        public void IsMatchPreservesMultipleRecursiveSegments(string path, string pattern, bool expected)
        {
            Assert.AreEqual(expected, AdditionalFilePatterns.IsMatch(path, pattern));
        }

        [TestMethod]
        public void AnyMatchSplitsListsAndSkipsBlankParts()
        {
            Assert.IsTrue(AdditionalFilePatterns.AnyMatch("Host.csproj", "*.txt;*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.AnyMatch("Host.csproj", "*.txt|*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.AnyMatch("Host.csproj", "*.txt,*.csproj"));
            Assert.IsTrue(AdditionalFilePatterns.AnyMatch("notes.txt", " *.md ; *.txt "));
            Assert.IsFalse(AdditionalFilePatterns.AnyMatch(null, "*.txt"));
            Assert.IsFalse(AdditionalFilePatterns.AnyMatch("notes.txt", " ; ; "));
        }

        [TestMethod]
        public void ToRelativePathStripsProjectDirectory()
        {
            Assert.AreEqual("notes.txt", AdditionalFilePatterns.ToRelativePath(@"C:\proj\notes.txt", @"C:\proj"));
            Assert.AreEqual("notes.txt", AdditionalFilePatterns.ToRelativePath(@"C:\proj\notes.txt", @"C:\proj\"));
            Assert.AreEqual("sub/notes.txt", AdditionalFilePatterns.ToRelativePath(@"C:\proj\sub\notes.txt", @"C:\proj"));
            Assert.AreEqual(string.Empty, AdditionalFilePatterns.ToRelativePath(@"C:\proj", @"C:\proj"));
            Assert.AreEqual("notes.txt", AdditionalFilePatterns.ToRelativePath("././notes.txt", null));
            Assert.AreEqual(@"D:/other/notes.txt", AdditionalFilePatterns.ToRelativePath(@"D:\other\notes.txt", @"C:\proj"));
            Assert.AreEqual(null, AdditionalFilePatterns.ToRelativePath(null, @"C:\proj"));
            Assert.AreEqual("  ", AdditionalFilePatterns.ToRelativePath("  ", @"C:\proj"));
        }

        [TestMethod]
        public void IsSelectedAppliesIncludesThenExcludes()
        {
            Assert.IsTrue(AdditionalFilePatterns.IsSelected(
                @"C:\proj\notes.txt",
                "*.txt;*.csproj",
                null,
                @"C:\proj"));
            Assert.IsTrue(AdditionalFilePatterns.IsSelected(
                @"C:\proj\sub\notes.txt",
                "**/*.txt",
                "other/*.txt",
                @"C:\proj"));
            Assert.IsFalse(AdditionalFilePatterns.IsSelected(
                @"C:\proj\sub\notes.txt",
                "**/*.txt",
                "sub/*.txt",
                @"C:\proj"));
            Assert.IsTrue(AdditionalFilePatterns.IsSelected(
                @"C:\proj\sub\deep\notes.txt",
                "**/*.txt",
                "sub/*.txt",
                @"C:\proj"));
        }
    }
}
