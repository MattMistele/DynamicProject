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


            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
            
        }
    }
}
