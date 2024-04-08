using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Output a prompt message to the console instructing the user to enter a value.
            Console.WriteLine("Please enter a value:");

            // Read the user's input from the console and store it in the 'name' variable.
            var name = Console.ReadLine();

            // Initialize a variable to hold a command-line argument if provided.
            string arg = string.Empty;

            // Check if any command-line arguments were provided.
            if (args.Length > 0)
            {
                // Assign the first command-line argument to the 'arg' variable.
                arg = args[0];
            }

            // Output a greeting message to the console, incorporating the command-line argument, user input, and result of the Foo() method call.
            Console.WriteLine($"Hello, World! {arg} {name} {ConsoleApp.Library.Class1.Foo()}");
        }
    }
}

