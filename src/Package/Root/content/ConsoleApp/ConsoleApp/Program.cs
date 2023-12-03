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
            var name = Console.ReadLine();
            string arg = String.Empty;
            if (args.Length > 0)
            {
                arg = args[0];
            }
            Console.WriteLine($@"Hello, World! {arg} {name} {ConsoleApp.Library.Class1.Foo()}");
        }
    }
}

