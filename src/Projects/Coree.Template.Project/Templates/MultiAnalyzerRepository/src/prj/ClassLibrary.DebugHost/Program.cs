using System;

namespace ClassLibrary.DebugHost
{
    internal static class Program
    {
        private static void Main()
        {
            // Make ClassLibrary the Visual Studio startup project and start debugging from there (F5).
            // Select the ClassLibrary Roslyn Component launch profile. Do not F5 this console.
            // Visual Studio needs the .NET Compiler Platform SDK component.
            // F5 on this console only runs Main; it does not attach to the analyzer.
            Console.WriteLine(typeof(lowercase).Name);
            Console.WriteLine(typeof(UPPER).Name);
        }
    }

    // lowercase reports ANL001 at compile time; UPPER does not.
    internal class lowercase
    {
    }

    internal class UPPER
    {
    }
}
