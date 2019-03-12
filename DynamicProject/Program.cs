using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            SortedDictionary<int, string> all = computeSentence(input);

            foreach(var str in all)
            {
                Console.WriteLine(str.Key + " " + str.Value);
            }

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }

        //static string[] addWhiteSpace(string input)
        //{

        //}

        // 1st attempt at dynamic part. Storing each sentence so we already know its possibilities
        static Dictionary<string, SortedDictionary<int, string>> savedSentences = new Dictionary<string, SortedDictionary<int, string>>();

        // Returns a SortedDictionary (C#'s generic version of a binary search tree)
        // ordered by <key = score, value = sentence>
        static SortedDictionary<int, string> computeSentence(string sentence)
        {
            SortedDictionary<int, string> result = new SortedDictionary<int, string>();

            // Base case - sentence length is 0. Return a space.
            if (sentence.Length == 0)
            {
                result.Add(0, " ");
                return result;
            }

            string strBuilder = "";
 
            for(int i = 0; i < Math.Min(MAX_WORD_SIZE, sentence.Length); i++)
            {
                strBuilder += sentence[i];
                Console.Write(" Checking " + strBuilder + " in '" + sentence + "'");

                // Check if the current word is valid. If so, add it to the result dictionary
                if(dictionary.ContainsKey(strBuilder))
                {
                    Console.Write("....... Match!");
                    string next = sentence.Substring(strBuilder.Length);

                    if (!savedSentences.ContainsKey(next))
                        savedSentences[next] = computeSentence(next);
                    else
                        Console.WriteLine(" Already calculated term '" + next + "'. Skipping.");

                    foreach (var possibility in savedSentences[next])
                        result.Add(possibility.Key + dictionary[strBuilder], strBuilder + " " + possibility.Value);
                }
                Console.WriteLine();
            }

            return result;
        }
    }
}
