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
                    allPossibleSentences.Add(computeSentence(current, input[i]));
                    current = "";
                }
                else
                    current += input[i];
            }

            // If the document doesn't end with punctuation, we'll still have stuff left in current.
            // Compute what we have left as it's own sentence
            if (current.Length >= 0)
                allPossibleSentences.Add(computeSentence(current, ' '));

            return allPossibleSentences;
        }

        // Input: sentence string without puncutuation or whitespace, char of the punctuation to put at the end
        // Output: sorted list of all valid sentences with whitespace and their scores
        //
        // ** This is a memory function of dynamic programming **
        //    It uses a global variable (a hashmap) _savedSentences to store the lists of valid
        //    sentences we've already calculated <key = sentence, value = list of valid sentences>
        static Dictionary<string, List<WeightedString>> _savedSentences;

        // Wrapper function for computeSentenceDynamic() to avoid repeated code
        static List<WeightedString> computeSentence(string sentence, char punctuation)
        {
            // Reset the memory from the dynamic programming function
            _savedSentences = new Dictionary<string, List<WeightedString>>();

            // Compute the senctence, sort the possibilities by score
            List<WeightedString> currentSentence = computeSentenceDynamic(sentence, punctuation);
            currentSentence = currentSentence.OrderBy(x => x.score).ToList();
            return currentSentence;
        }

        static List<WeightedString> computeSentenceDynamic(string sentence, char punctuation)
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
                        _savedSentences[next] = computeSentenceDynamic(next, punctuation);
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
