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
        public const int TOP_SENTENCES_TO_KEEP = 100;
        public const int TOP_DOCUMENTS_TO_KEEP = 50;
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

            // Read the input file
            Logger.Out("Enter the name of the input file. Make sure it's in the current directory: ");
            input = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, Console.ReadLine()));
            Logger.Out("Your input is: \n" + input + "\n");


            // Get the top 100 valid sentences for each sentence in the document
            List<List<WeightedString>> allSentences = computeDocument(input);

            // Put them together to get the top 50 most likely origional documents
            List<WeightedString> topDocuments = aggregateTopDocuments(allSentences);

            Logger.Out();

            for (int i = 0; i < allSentences.Count; i++)
            {
                Logger.Out("Top 100 possibilities for sentence #" + i + ": ");

                foreach(WeightedString sentence in allSentences[i])
                {
                    Logger.Out("     Score:" + sentence.score + " Sentence: " + sentence.value);
                }
                Logger.Out();
            }

            Logger.Out(); Logger.Out();

            for (int i = 0; i < topDocuments.Count; i++)
            {
                Logger.Out("Top document #" + i + " with score " + topDocuments[i].score + ": ");
                Logger.Out("    " + topDocuments[i].value + "\n");
            }

            // Save to file
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "output.txt"), Logger.LogString.ToString());
            Console.WriteLine("Results saved to output.txt");

            // keep the console open at the end so it doesn't immediatly close
            Console.ReadLine();
        }

        // Input: list (of each sentence in the document) of sorted lists (all valid possibilities for each sentence)
        // Output: The top 50 most likely origional documents with their score
        //
        // **Note: I am changing the spec from "all possible original documents" to this output, 
        //         because the output got super big with larger documents
        //
        // **Note: In order to make the documents different enough from each other, 
        //         take the #1 most likely sentences for document #1, the #2 most likely sentences for document #2, etc..
        static List<WeightedString> aggregateTopDocuments(List<List<WeightedString>> allSentences)
        {
            List<WeightedString> result = new List<WeightedString>();

            for (int documentIndex = 0; documentIndex < TOP_DOCUMENTS_TO_KEEP; documentIndex++)
            {
                WeightedString currentDocument = new WeightedString("", 0);
                foreach (List<WeightedString> sentencePossibilities in allSentences)
                {
                    // If a sentence only has say 2 possibilities, use the last possibility for computing the rest of the 3 documents.
                    int sentenceToPull = sentencePossibilities.Count - 1 < documentIndex ? sentencePossibilities.Count - 1 : documentIndex;

                    currentDocument.value += sentencePossibilities[sentenceToPull].value;
                    currentDocument.score += sentencePossibilities[sentenceToPull].score;
                }
                result.Add(currentDocument);
            }

            return result;
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

        public static bool isPunctuation(char c)
        {
            return c.Equals('!') || c.Equals('?') || c.Equals('.') || c.Equals(':') || c.Equals(';') || c.Equals(',');
        }
    }
}
