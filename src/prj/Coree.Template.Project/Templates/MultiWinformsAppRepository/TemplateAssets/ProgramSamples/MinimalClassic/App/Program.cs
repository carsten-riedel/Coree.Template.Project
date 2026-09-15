#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;

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
        /// <param name="args">Command-line arguments.</param>
        /// <returns>0 (success).</returns>
        public static int Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            return 0;
        }
    }
}
