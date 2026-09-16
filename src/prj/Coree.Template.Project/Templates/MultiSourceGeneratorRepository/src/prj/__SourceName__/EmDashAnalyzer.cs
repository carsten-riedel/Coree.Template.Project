#if( KeepScaffoldUnusedUsings )
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endif
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace __SourceName__
{
    /// <summary>
    /// Sample analyzer: warns when C# source or matching additional files contain an em dash (U+2014).
    /// Replace this type with your own diagnostic analyzer.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class EmDashAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Diagnostic identifier for the sample em dash rule.
        /// </summary>
        public const string DiagnosticId = "EMD001";

        internal const string SeverityPropertyName = "EmDashAnalyzerSeverity";

        internal const string IncludesPropertyName = "EmDashAnalyzerIncludes";

        internal const string ExcludesPropertyName = "EmDashAnalyzerExcludes";

        private const string EmDashCharacters = "\u2014";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Source contains an em dash",
            "Source contains an em dash (U+2014). Use ASCII hyphen-minus.",
            "Typography",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Copy-paste from word processors often inserts em dashes instead of ASCII hyphens.");

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
                EmDashCharacters,
                SeverityPropertyName,
                IncludesPropertyName,
                ExcludesPropertyName);
        }
    }
}
