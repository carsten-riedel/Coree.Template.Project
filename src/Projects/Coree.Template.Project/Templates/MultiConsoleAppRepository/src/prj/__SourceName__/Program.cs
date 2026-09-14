#if( KeepScaffoldUnusedUsings )
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endif
namespace __SourceName__
{
    /// <summary>
    /// Console entry point. Tests call <see cref="Main"/> as a method.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Writes a greeting and exits successfully so <c>dotnet run</c> does something visible.
        /// </summary>
        /// <returns>0 (success).</returns>
        public static int Main()
        {
            System.Console.WriteLine("Hello, World!");
            return 0;
        }
    }
}
