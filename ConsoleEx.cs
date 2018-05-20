using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager
{
    public static class ConsoleEx
    {
        private static ConsoleColor defaultColor = ConsoleColor.White;

        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = defaultColor;
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = defaultColor;
        }
    }
}
