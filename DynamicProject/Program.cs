using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamicProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> words = DictionaryParser.parseDictionary1("dictionary.txt");
            string input;

            Console.Write("Enter the name of the input file. Make sure it's in the current directory: ");
            input = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, Console.ReadLine()));

            Console.WriteLine("Your input is: \n" + input);

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }
    }
}
