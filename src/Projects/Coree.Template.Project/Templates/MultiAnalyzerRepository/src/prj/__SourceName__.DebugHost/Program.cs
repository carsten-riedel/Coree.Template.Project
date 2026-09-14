using System;

namespace __SourceName__.DebugHost
{
    internal static class Program
    {
        private static void Main()
        {
            // Make __SourceName__ the Visual Studio startup project and start debugging from there (F5).
            // Select the __SourceName__ Roslyn Component launch profile. Do not F5 this console.
            // Visual Studio needs the .NET Compiler Platform SDK component.
            // F5 on this console only runs Main; it does not attach to the analyzer.

            // Change EmDashAnalyzerSeverity / SmartQuotesAnalyzerSeverity on this csproj
            // (warning, error, message, or off).
            // Change EmDashAnalyzerIncludes / SmartQuotesAnalyzerIncludes
            // and EmDashAnalyzerExcludes / SmartQuotesAnalyzerExcludes
            // (semicolon-separated globs; empty includes skip additional files).
            // ASCII hyphen and quotes do not report.
            Console.WriteLine("1-2");
            Console.WriteLine("\"hello\"");

            // Em dash reports EMD001; typographic quotes report TSQ001.
            Console.WriteLine("1—2");
            Console.WriteLine("“hello”");
        }
    }
}
