using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ClassLibrary
{
    /// <summary>
    /// Sample analyzer: warns when source text contains typographic quotation marks
    /// (curly quotes and guillemets), not ASCII <c>"</c> or <c>'</c>.
    /// Replace this type with your own diagnostic analyzer.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SmartQuotesAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Diagnostic identifier for the sample typographic quote rule.
        /// </summary>
        public const string DiagnosticId = "TSQ001";

        internal const string SeverityPropertyName = "SmartQuotesAnalyzerSeverity";

        // “ ” „ ‟ « »
        private const string TypographicQuoteCharacters = "\u201C\u201D\u201E\u201F\u00AB\u00BB";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Source contains a typographic quote",
            "Source contains a typographic quotation mark. Use ASCII double quote or apostrophe.",
            "Typography",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Copy-paste from word processors often inserts curly quotes or guillemets instead of ASCII quotes.");

        private static readonly DiagnosticDescriptor ErrorRule =
            AnalyzerSeverity.WithSeverity(Rule, DiagnosticSeverity.Error);

        private static readonly DiagnosticDescriptor InfoRule =
            AnalyzerSeverity.WithSeverity(Rule, DiagnosticSeverity.Info);

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            SourceCharacterScanner.Register(
                context,
                Rule,
                ErrorRule,
                InfoRule,
                TypographicQuoteCharacters,
                SeverityPropertyName);
        }
    }
}
