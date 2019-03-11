using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamicProject
{
    class Program
    {
        const int MAX_WORD_SIZE = 16;
        static Dictionary<string, int> dictionary;

        static void Main(string[] args)
        {
            dictionary = DictionaryParser.parseDictionary1("dictionary.txt");
            string input;

            //// Read the input file
            //Console.Write("Enter the name of the input file. Make sure it's in the current directory: ");
            //input = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, Console.ReadLine()));
            //Console.WriteLine("Your input is: \n" + input + "\n");

            Console.Write("Enter your input: ");
            input = Console.ReadLine().ToLower();

            List<string> all = computeSentence(input);

            foreach(var str in all)
            {
                Console.WriteLine(str);
            }

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }

        //static string[] addWhiteSpace(string input)
        //{

        //}

        static List<string> computeSentence(string sentence)
        {
            // Base case - sentence length is 0
            if (sentence.Length == 0)
                return new List<string>() { " " };

            List<string> result = new List<string>();
            string strBuilder = "";
            int lookingAtIndex = 0;

            for(int i = lookingAtIndex; i < Math.Min(MAX_WORD_SIZE, sentence.Length); i++)
            {
                strBuilder += sentence[i];
                Console.Write(" Checking " + strBuilder + " in '" + sentence + "'");

                // Check if this current word is valid. If so, add it to the result dictionary
                if(dictionary.ContainsKey(strBuilder))
                {
                    Console.Write("....... Match!");
                    string next = sentence.Substring(strBuilder.Length);
                    List<string> nextPossibilities = computeSentence(next);

                    foreach (string possibility in nextPossibilities)
                        result.Add(strBuilder + " " + possibility);
                }
                Console.WriteLine();
            }

            return result;
        }
    }
}
