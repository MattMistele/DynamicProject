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

        // Datastruture to define a sentence/document
        // and it's score (where less = more likely)
        struct WeightedString
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

            List<SortedDictionary<int, string>> all = computeDocument(input);

            for(int i = 0; i < all.Count; i++)
            {
                Console.WriteLine("All possibilities for sentence " + i + ": ");

                foreach(var sentence in all[i])
                {
                    Console.WriteLine("     Score:" + sentence.Key + " Sentence: " + sentence.Value);
                }
                Console.WriteLine();
            }

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }

        // Input: Text without whitespace, may contain sentence splitting puncutation (.?!:;,)
        // Output: 1. Top 5 most likely documents with their score in a BST
        //         2. A list (of each sentence) of sorted lists (all valid possibilities for that sentence) 
        //
        // **Note: I am changing the spec from "all possible original documents" to this output, 
        //         because the output got super big with larger documents
        static List<SortedDictionary<int, string>> computeDocument(string input)
        {
            SortedSet<WeightedString> mostLikelyDocuments = new SortedSet<WeightedString>();
            List<List<WeightedString>> allPossibleSentences = new List<List<WeightedString>>();

            string current = "";

            for (int i = 0; i < input.Length; i++)
            {
                // If we find punctuation that splits words, compute it as a sentence.
                if (isPunctuation(input[i]))
                {
                    // Reset the memory from the dynamic programming function, compute the sentence
                    _savedSentences = new Dictionary<string, SortedDictionary<int, string>>();
                    SortedDictionary<int, string> currentSentence = computeSentence(current, input[i]);
                    allPossibleSentences.Add(currentSentence);

                    // Because we want to come up with each possible essay, add
                    // each possibility for this sentance to every other possiblility we already have in the result
                    foreach (var document in mostLikelyDocuments)
                    {
                        //  document.
                    }

                    current = "";
                }
                else
                {
                    current += input[i];
                }
            }

            return allPossibleSentences;
        }


        // Input: sentence string without puncutuation or whitespace, char of the punctuation to put at the end
        // Output: list of all valid sentences with whitespace and their scores
        //
        // ** This is a memory function of dynamic programming **
        //    It uses a global variable (a hashmap) _savedSentences to store the lists of valid
        //    sentences we've already calculated <key = sentence, value = list of valid sentences>
        static Dictionary<string, List<WeightedString>> _savedSentences = new Dictionary<string, List<WeightedString>>();
        
        static List<WeightedString> computeSentence(string sentence, char punctuation)
        {
            // List of all valid sentences for a given sentence
            List<WeightedString> result = new List<WeightedString>();

            // Base case - sentence length is 0. Return a backspace and the punctuation.
            if (sentence.Length == 0)
            {
                result.Add(new WeightedString("\b" + punctuation, 0));
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
                        _savedSentences[next] = computeSentence(next, punctuation);
                    else
                        Console.WriteLine(" Already calculated term '" + next + "'. Skipping.");

                    // Add all valid results to the list

                    // TODO: Allow senteces with the same score in
                    foreach (var possibility in _savedSentences[next])
                        result.Add(new WeightedString(strBuilder + " " + possibility.value, possibility.score + dictionary[strBuilder]));
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
