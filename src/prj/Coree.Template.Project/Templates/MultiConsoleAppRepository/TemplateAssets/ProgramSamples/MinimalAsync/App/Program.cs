#if( CSharpProjectOptions == "DisableImplicitUsings" )
using System;
using System.Threading.Tasks;

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
        /// <returns>A task that completes with 0 (success).</returns>
        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            await Task.CompletedTask;
            return 0;
        }
    }
}
