using System;
using System.Collections.Generic;
using System.Linq;
using static DynamicProject.Program;

namespace DynamicProject
{
    class DynamicAlgorithm
    {
        // hashmap global variable to store the lists of valid sentence we've already calculated. 
        static Dictionary<string, List<WeightedString>> _savedSentences;

        // Wrapper function for computeSentenceDynamic() to avoid repeated code
        public static List<WeightedString> computeSentence(string sentence, char punctuation)
        {
            // Reset the memory from the dynamic programming function
            _savedSentences = new Dictionary<string, List<WeightedString>>();

            // Compute the senctence, sort the possibilities by score, only keep the top 100 sentences
            List<WeightedString> currentSentence = computeSentenceDynamic(sentence, punctuation);
            currentSentence = currentSentence.OrderBy(x => x.score).ToList();
            if (currentSentence.Count > TOP_SENTENCES_TO_KEEP)
                currentSentence.RemoveRange(TOP_SENTENCES_TO_KEEP, currentSentence.Count - TOP_SENTENCES_TO_KEEP);

            return currentSentence;
        }

        // Input: sentence string without puncutuation or whitespace, char of the punctuation to put at the end
        // Output: sorted list of all valid sentences with whitespace and their scores
        //
        // ** This is a memory function of dynamic programming **
        //    It uses a global variable (a hashmap) _savedSentences to store the lists of valid
        //    sentences we've already calculated <key = sentence, value = list of valid sentences>
        static List<WeightedString> computeSentenceDynamic(string sentence, char punctuation)
        {
            // List of all valid sentences for a given sentence
            List<WeightedString> result = new List<WeightedString>();

            // BASE CASE - sentence length is 0. Return the punctuation.
            if (sentence.Length == 0)
            {
                result.Add(new WeightedString(punctuation + " ", 0));
                return result;
            }

            // RECURSIVE CASE
            string strBuilder = "";

            for (int i = 0; i < Math.Min(MAX_WORD_SIZE, sentence.Length); i++)
            {
                strBuilder += sentence[i];

                // Check if the current word is valid. If so, add it to the result dictionary
                if (dictionary.ContainsKey(strBuilder.ToLower()))
                {
                    // Now, take the rest of the string, and see if we've already caluclated it before.
                    // DYNAMIC PART - if we have, take the results from the hashmap _saveSentences
                    // If we haven't, run it as it's own sentence recursivly through this function
                    string next = sentence.Substring(strBuilder.Length);

                    if (!_savedSentences.ContainsKey(next))
                        _savedSentences[next] = computeSentenceDynamic(next, punctuation);

                    // Add all valid results to the list
                    foreach (var possibility in _savedSentences[next])
                    {
                        string toAdd = isPunctuation(possibility.value[0]) ? strBuilder : strBuilder + " ";
                        result.Add(new WeightedString(toAdd + possibility.value, possibility.score + dictionary[strBuilder.ToLower()]));
                    }
                }
            }

            return result;
        }

    }
}
