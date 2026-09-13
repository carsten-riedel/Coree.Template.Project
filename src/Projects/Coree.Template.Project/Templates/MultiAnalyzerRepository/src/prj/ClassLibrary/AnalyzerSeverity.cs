#nullable disable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ClassLibrary
{
    internal static class AnalyzerSeverity
    {
        internal static DiagnosticSeverity? Parse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return DiagnosticSeverity.Warning;
            }

            switch (raw.Trim().ToLowerInvariant())
            {
                case "error":
                    return DiagnosticSeverity.Error;
                case "message":
                case "information":
                case "info":
                    return DiagnosticSeverity.Info;
                case "off":
                case "none":
                case "silent":
                    return null;
                case "warning":
                default:
                    return DiagnosticSeverity.Warning;
            }
        }

        internal static DiagnosticSeverity? Read(AnalyzerConfigOptions options, string msbuildPropertyName)
        {
            string raw;
            options.TryGetValue("build_property." + msbuildPropertyName, out raw);
            return Parse(raw);
        }

        internal static DiagnosticDescriptor WithSeverity(DiagnosticDescriptor rule, DiagnosticSeverity severity)
        {
            if (rule.DefaultSeverity == severity)
            {
                return rule;
            }

            return new DiagnosticDescriptor(
                rule.Id,
                rule.Title,
                rule.MessageFormat,
                rule.Category,
                severity,
                rule.IsEnabledByDefault,
                rule.Description,
                rule.HelpLinkUri);
        }

        internal static DiagnosticDescriptor SelectDescriptor(
            DiagnosticDescriptor warningRule,
            DiagnosticDescriptor errorRule,
            DiagnosticDescriptor infoRule,
            DiagnosticSeverity severity)
        {
            switch (severity)
            {
                case DiagnosticSeverity.Error:
                    return errorRule;
                case DiagnosticSeverity.Info:
                    return infoRule;
                default:
                    return warningRule;
            }
        }
    }
}
