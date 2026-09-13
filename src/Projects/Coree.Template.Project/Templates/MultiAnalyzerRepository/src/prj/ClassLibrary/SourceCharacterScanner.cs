using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace ClassLibrary
{
    internal static class SourceCharacterScanner
    {
        internal static void Register(
            AnalysisContext context,
            DiagnosticDescriptor warningRule,
            DiagnosticDescriptor errorRule,
            DiagnosticDescriptor infoRule,
            string characters,
            string severityPropertyName)
        {
            context.RegisterCompilationStartAction(startContext =>
            {
                var severity = AnalyzerSeverity.Read(
                    startContext.Options.AnalyzerConfigOptionsProvider.GlobalOptions,
                    severityPropertyName);
                if (!severity.HasValue)
                {
                    return;
                }

                var rule = AnalyzerSeverity.SelectDescriptor(
                    warningRule,
                    errorRule,
                    infoRule,
                    severity.Value);
                startContext.RegisterSyntaxTreeAction(treeContext =>
                    ReportEachMatch(treeContext, rule, characters));
            });
        }

        internal static void ReportEachMatch(
            SyntaxTreeAnalysisContext context,
            DiagnosticDescriptor rule,
            string characters)
        {
            var tree = context.Tree;
            var text = tree.GetText(context.CancellationToken);
            var length = text.Length;
            for (var i = 0; i < length; i++)
            {
                if (characters.IndexOf(text[i]) < 0)
                {
                    continue;
                }

                context.ReportDiagnostic(
                    Diagnostic.Create(rule, Location.Create(tree, TextSpan.FromBounds(i, i + 1))));
            }
        }
    }
}
