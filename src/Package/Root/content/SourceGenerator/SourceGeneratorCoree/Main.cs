using Microsoft.CodeAnalysis;

namespace SourceGeneratorCoree
{

    //https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview

    //https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md

    //https://learn.microsoft.com/en-us/nuget/guides/analyzers-conventions#analyzers-path-format

    [Generator]
    public class HelloGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Find the main method
            var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);

            // Build up the source code
            string source = $@"// <auto-generated/>
using System;

namespace {mainMethod.ContainingNamespace.ToDisplayString()}
{{
    public static partial class {mainMethod.ContainingType.Name}
    {{
        public static partial void HelloFrom(string name) =>
            Console.WriteLine($""Generatorx says: Hi from '{{name}}'"");
    }}
}}
";
            var typeName = mainMethod.ContainingType.Name;

            // Add the source code to the compilation
            context.AddSource($"{typeName}.g.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}