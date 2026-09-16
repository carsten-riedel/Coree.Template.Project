using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace __SourceName__.Tests
{
    [TestClass]
    public sealed class JsonSupportGeneratorTests
    {
        [TestMethod]
        public void PublicClassGeneratesWorkingJsonRoundTrip()
        {
            const string source = """
                using __SourceName__;

                namespace Demo;

                [GenerateJsonSupport]
                public sealed class Customer
                {
                    public string Id { get; set; } = "";

                    public int Number { get; set; }
                }
                """;

            var result = RunGenerator(source);
            AssertNoErrors(result.OutputCompilation);

            var generated = GetGeneratedSource(result.RunResult, "Demo.Customer.JsonSupport.g.cs");
            StringAssert.Contains(generated, "public static class CustomerJson");
            StringAssert.Contains(generated, "global::Demo.Customer");

            using var assemblyStream = new MemoryStream();
            var emitResult = result.OutputCompilation.Emit(assemblyStream);
            Assert.IsTrue(emitResult.Success, string.Join(Environment.NewLine, emitResult.Diagnostics));

            assemblyStream.Position = 0;
            var assembly = AssemblyLoadContext.Default.LoadFromStream(assemblyStream);
            var customerType = assembly.GetType("Demo.Customer", throwOnError: true)!;
            var helperType = assembly.GetType("Demo.CustomerJson", throwOnError: true)!;
            var customer = Activator.CreateInstance(customerType)!;
            customerType.GetProperty("Id")!.SetValue(customer, "C-42");
            customerType.GetProperty("Number")!.SetValue(customer, 42);

            var json = (string)helperType.GetMethod("Serialize")!.Invoke(null, new[] { customer, true })!;
            StringAssert.Contains(json, "\"Id\": \"C-42\"");
            StringAssert.Contains(json, "\"Number\": 42");

            var roundTrip = helperType.GetMethod("Deserialize")!.Invoke(null, new object[] { json })!;
            Assert.AreEqual("C-42", customerType.GetProperty("Id")!.GetValue(roundTrip));
            Assert.AreEqual(42, customerType.GetProperty("Number")!.GetValue(roundTrip));
        }

        [TestMethod]
        public void InternalClassInGlobalNamespaceGeneratesInternalHelper()
        {
            const string source = """
                [__SourceName__.GenerateJsonSupport]
                internal sealed class Settings
                {
                    public bool Enabled { get; set; }
                }
                """;

            var result = RunGenerator(source);
            AssertNoErrors(result.OutputCompilation);

            var generated = GetGeneratedSource(result.RunResult, "Settings.JsonSupport.g.cs");
            StringAssert.Contains(generated, "internal static class SettingsJson");
            Assert.IsFalse(generated.Contains("namespace ", StringComparison.Ordinal));
        }

        [TestMethod]
        public void GenericClassReportsDiagnostic()
        {
            const string source = """
                [__SourceName__.GenerateJsonSupport]
                public sealed class Envelope<T>
                {
                }
                """;

            var result = RunGenerator(source);
            var diagnostic = AssertSingleGeneratorDiagnostic(result.RunResult);
            Assert.AreEqual(JsonSupportGenerator.DiagnosticId, diagnostic.Id);
            StringAssert.Contains(diagnostic.GetMessage(), "Envelope<T>");
        }

        [TestMethod]
        public void NestedClassReportsDiagnostic()
        {
            const string source = """
                public static class Container
                {
                    [__SourceName__.GenerateJsonSupport]
                    public sealed class Nested
                    {
                    }
                }
                """;

            var result = RunGenerator(source);
            var diagnostic = AssertSingleGeneratorDiagnostic(result.RunResult);
            Assert.AreEqual(JsonSupportGenerator.DiagnosticId, diagnostic.Id);
            StringAssert.Contains(diagnostic.GetMessage(), "Container.Nested");
        }

        [TestMethod]
        public void StaticClassReportsDiagnostic()
        {
            const string source = """
                [__SourceName__.GenerateJsonSupport]
                public static class StaticSettings
                {
                }
                """;

            var result = RunGenerator(source);
            var diagnostic = AssertSingleGeneratorDiagnostic(result.RunResult);
            Assert.AreEqual(JsonSupportGenerator.DiagnosticId, diagnostic.Id);
            StringAssert.Contains(diagnostic.GetMessage(), "StaticSettings");
        }

        [TestMethod]
        public void FileLocalClassReportsDiagnostic()
        {
            const string source = """
                [__SourceName__.GenerateJsonSupport]
                file sealed class FileSettings
                {
                }
                """;

            var result = RunGenerator(source);
            var diagnostic = AssertSingleGeneratorDiagnostic(result.RunResult);
            Assert.AreEqual(JsonSupportGenerator.DiagnosticId, diagnostic.Id);
            StringAssert.Contains(diagnostic.GetMessage(), "FileSettings");
        }

        private static GeneratorResult RunGenerator(string source)
        {
            var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
            var syntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);
            var compilation = CSharpCompilation.Create(
                "GeneratorTests_" + Guid.NewGuid().ToString("N"),
                new[] { syntaxTree },
                GetPlatformReferences(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            GeneratorDriver driver = CSharpGeneratorDriver.Create(
                new[] { new JsonSupportGenerator().AsSourceGenerator() },
                parseOptions: parseOptions);
            driver = driver.RunGeneratorsAndUpdateCompilation(
                compilation,
                out var outputCompilation,
                out var generatorDiagnostics);

            Assert.IsFalse(
                generatorDiagnostics.Any(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error),
                string.Join(Environment.NewLine, generatorDiagnostics));
            return new GeneratorResult(outputCompilation, driver.GetRunResult());
        }

        private static IReadOnlyList<MetadataReference> GetPlatformReferences()
        {
            var trustedAssemblies = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
            Assert.IsFalse(string.IsNullOrWhiteSpace(trustedAssemblies));

            return trustedAssemblies!
                .Split(Path.PathSeparator)
                .Select(static path => MetadataReference.CreateFromFile(path))
                .ToArray();
        }

        private static string GetGeneratedSource(GeneratorDriverRunResult result, string hintName)
        {
            var generated = result.Results.Single().GeneratedSources.Single(source => source.HintName == hintName);
            return generated.SourceText.ToString();
        }

        private static Diagnostic AssertSingleGeneratorDiagnostic(GeneratorDriverRunResult result)
        {
            return result.Results.Single().Diagnostics.Single();
        }

        private static void AssertNoErrors(Compilation compilation)
        {
            var errors = compilation.GetDiagnostics().Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
            Assert.AreEqual(0, errors.Count(), string.Join(Environment.NewLine, errors));
        }

        private sealed class GeneratorResult
        {
            internal GeneratorResult(Compilation outputCompilation, GeneratorDriverRunResult runResult)
            {
                OutputCompilation = outputCompilation;
                RunResult = runResult;
            }

            internal Compilation OutputCompilation { get; }

            internal GeneratorDriverRunResult RunResult { get; }
        }
    }
}
