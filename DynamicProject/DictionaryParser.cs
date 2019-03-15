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
            string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, fileName));
            int currentScore = 1;

            Logger.Out("Parsing dictionary.txt into hashmap... ");

            // start parsing at the third line, bc the first two lines are just comments
            for(int i = 2; i < lines.Length; i++)
            {
                string currentLine = lines[i].ToLower();

                if (currentLine.StartsWith("#!comment:"))
                    currentScore += 100;
                else
                {
                    // there are repeated words in dictionary.txt (ex "but" on line 23 and line 70)
                    // check this word isn't already in our hashmap
                    // Also, don't add 1 letter words
                    if (!words.ContainsKey(currentLine) && currentLine.Length > 1)
                        words.Add(currentLine, currentScore);
                }
            }

            // Edge case - 1 letter words
            words.Add("I", 1);
            words.Add("A", 1);
            // TODO: change - only do upper/lower case?
            words.Add("i", 1);
            words.Add("a", 1);

            Logger.Out("Done.");
            Logger.Out();

            return words;
        }

    }
}
