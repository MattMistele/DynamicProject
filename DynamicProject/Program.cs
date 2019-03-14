using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DynamicProject
{
    class Program
    {
        public const int MAX_WORD_SIZE = 16;
        public static Dictionary<string, int> dictionary;

        // Datastruture to define a sentence/document
        // and it's score (where less = more likely)
        public struct WeightedString
        {
            public string value;
            public int score;

            public WeightedString(string v, int s) { value = v; score = s; }
        }

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

            List<List<WeightedString>> allSentences = computeDocument(input);

            for(int i = 0; i < allSentences.Count; i++)
            {
                Console.WriteLine("All possibilities for sentence " + i + ": ");

                foreach(WeightedString sentence in allSentences[i])
                {
                    Console.WriteLine("     Score:" + sentence.score + " Sentence: " + sentence.value);
                }
                Console.WriteLine();
            }

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }

        // Input: list (of each sentence in the document) of sorted lists (all valid possibilities for each sentence)
        // Output: The top 5 most likely origional documents
        //
        // **Note: I am changing the spec from "all possible original documents" to this output, 
        //         because the output got super big with larger documents
        static List<string> aggregateTopDocuments(List<List<WeightedString>> allSentences)
        {

        }

        // Input: Text without whitespace, may contain sentence splitting puncutation (.?!:;,)
        // Output: A list (of each sentence in the document) of sorted lists (all valid possibilities for each sentence) 
        static List<List<WeightedString>> computeDocument(string input)
        {
            List<List<WeightedString>> allPossibleSentences = new List<List<WeightedString>>();
            string current = "";

            for (int i = 0; i < input.Length; i++)
            {
                // If we find punctuation that splits words, compute it as a sentence.
                if (isPunctuation(input[i]))
                {
                    allPossibleSentences.Add(DynamicAlgorithm.computeSentence(current, input[i]));
                    current = "";
                }
                else
                    current += input[i];
            }

            // If the document doesn't end with punctuation, we'll still have stuff left in current.
            // Compute what we have left as it's own sentence
            if (current.Length >= 0)
                allPossibleSentences.Add(DynamicAlgorithm.computeSentence(current, ' '));

            return allPossibleSentences;
        }

        static bool isPunctuation(char c)
        {
            return c.Equals('!') || c.Equals('?') || c.Equals('.') || c.Equals(':') || c.Equals(';') || c.Equals(',');
        }
    }
}
