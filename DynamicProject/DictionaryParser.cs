using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamicProject
{
    class DictionaryParser
    {
        
        // Input: dictionary.txt
        // Output: Dictionary (basically a hashmap). Key = word, Value = popularity
        public static Dictionary<string, int> parseDictionary1(string fileName)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();
            string[] lines = File.ReadAllLines(fileName);
            int currentScore = 1;

            Console.Write("Parsing dictionary.txt into hashmap... ");

            // start parsing at the third line, bc the first two lines are just comments
            for(int i = 2; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("#!comment:"))
                    currentScore += 100;
                else
                {
                    // there are repeated words in dictionary.txt (ex "but" on line 23 and line 70)
                    // check this word isn't already in our hashmap
                    if (!words.ContainsKey(lines[i]))
                        words.Add(lines[i], currentScore);
                }
            }
            Console.Write("Done.");

            return words;
        }

    }
}
