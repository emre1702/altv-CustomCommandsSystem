using CustomCommandSystem.Common.Interfaces.Services;
using System;

namespace CustomCommandSystem.Services.Utils
{
    internal class ConsoleLogger : ILogger
    {
        public void LogError(string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[Error in CustomCommandSystem] " + message);
            Console.ForegroundColor = oldColor;
        }

        public void LogWarning(string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[Warning in CustomCommandSystem] " + message);
            Console.ForegroundColor = oldColor;
        }
    }
}
