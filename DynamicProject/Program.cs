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

        // Input: Text without whitespace, may contain splitting puncutation (.?!:;,)
        // Output: 1. Top 5 most likely documents with their score
        //         2. A list (of each sentence) of lists (all valid possibilities for that sentence) 
        // ** Note: I am changing the spec from "all possible original documents" to this output, 
        //          because the output got super big with larger documents
        static SortedDictionary<int, string> addWhiteSpace(string input)
        {
            string current = "";
            SortedDictionary<int, string> result = new SortedDictionary<int, string>();

            for(int i = 0; i < input.Length; i++)
            {
                // If we find punctuation that splits words, compute it as a sentence.
                if (isPunctuation(input[i]))
                {
                    SortedDictionary<int, string> currentSentence = computeSentence(current);

                    // Because we want to come up with each possible essay, add
                    // each possibility for this sentance to every other possiblility we already have in the result
                    foreach(var essay in result)
                    {

                    }
                }
            }

            return result;
        }


        // Input: sentence string without puncutuation or whitespace
        // Output: A binary search tree with all valid sentences with whitespace, 
        //         ordered by their score (smaller score = more likelyhood of being the actual sentence)
        //
        // ** This is a memory function of dynamic programming **
        //    It uses a global variable (a hashmap) _savedSentences to store the lists of valid
        //    sentences we've already calculated <key = sentence, value = list of valid sentences>
        static Dictionary<string, SortedDictionary<int, string>> _savedSentences = new Dictionary<string, SortedDictionary<int, string>>();

        static SortedDictionary<int, string> computeSentence(string sentence)
        {
            // BST of all valid sentences for a given sentence, ordered by score
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

                    // Now, take the rest of the string, and see if we've already caluclated it before.
                    // DYNAMIC PART - if we have, take the results from the hashmap _saveSentences
                    // If we haven't, run it as it's own sentence recursivly through this function
                    if (!_savedSentences.ContainsKey(next))
                        _savedSentences[next] = computeSentence(next);
                    else
                        Console.WriteLine(" Already calculated term '" + next + "'. Skipping.");

                    // Add all valid results to the list
                    foreach (var possibility in _savedSentences[next])
                        result.Add(possibility.Key + dictionary[strBuilder], strBuilder + " " + possibility.Value);
                }
                Console.WriteLine();
            }

            return result;
        }


        static bool isPunctuation(char c)
        {
            return c.Equals('!') || c.Equals('?') || c.Equals('.') || c.Equals(':') || c.Equals(';') || c.Equals(',');
        }
    }
}
