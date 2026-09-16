using System;
using __SourceName__;

namespace __SourceName__.DebugHost
{
    [GenerateJsonSupport]
    internal sealed class Customer
    {
        public string Id { get; set; } = string.Empty;

        public int Number { get; set; }
    }

    internal static class Program
    {
        private static void Main()
        {
            // Make __SourceName__ the Visual Studio startup project and start debugging from there (F5).
            // Select the __SourceName__ Roslyn Component launch profile. Do not F5 this console.
            // Visual Studio needs the .NET Compiler Platform SDK component.
            // F5 on this console only runs Main; generator breakpoints are hit while this project compiles.

            var customer = new Customer { Id = "C-42", Number = 42 };
            var json = CustomerJson.Serialize(customer);
            var roundTrip = CustomerJson.Deserialize(json);

            Console.WriteLine(json);
            Console.WriteLine($"Round trip: {roundTrip?.Id} / {roundTrip?.Number}");
        }
    }
}
