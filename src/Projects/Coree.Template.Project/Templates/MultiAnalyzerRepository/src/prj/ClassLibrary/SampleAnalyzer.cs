#if( KeepScaffoldUnusedUsings )
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endif
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ClassLibrary
{
    /// <summary>
    /// Sample analyzer: warns when a named type's identifier contains a lowercase letter.
    /// Replace this type with your own diagnostic analyzer.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SampleAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Diagnostic identifier for the sample naming rule.
        /// </summary>
        public const string DiagnosticId = "ANL001";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            "Type name contains lowercase letters",
            "Type name '{0}' contains lowercase letters",
            "Naming",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Type names in this sample are expected to be all uppercase.");

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
            context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            var namedType = (INamedTypeSymbol)context.Symbol;
            if (namedType.Name.Any(char.IsLower))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, namedType.Locations[0], namedType.Name));
            }
        }
    }
}
