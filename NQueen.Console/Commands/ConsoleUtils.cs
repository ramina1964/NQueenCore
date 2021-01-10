using System;

namespace NQueen.ConsoleApp.Commands
{
    public static class ConsoleUtils
    {
        public static void WriteLineColored(ConsoleColor color, string str)
        {
            ConsoleColor priorColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = priorColor;
        }
    }
}
