using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicProject
{
    public static class Logger
    {
        public static StringBuilder LogString = new StringBuilder();
        public static void Out(string str)
        {
            Console.WriteLine(str);
            LogString.Append(str).Append(Environment.NewLine);
        }
        public static void Out()
        {
            Console.WriteLine();
            LogString.Append("").Append(Environment.NewLine);
        }
    }
}
