using CustomCommandsSystem.Common.Interfaces.Services;
using System;

namespace CustomCommandsSystem.Services.Utils
{
    internal class ConsoleLogger : ILogger
    {
        public void LogError(string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[Error in CustomCommandsSystem] " + message);
            Console.ForegroundColor = oldColor;
        }

        public void LogWarning(string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[Warning in CustomCommandsSystem] " + message);
            Console.ForegroundColor = oldColor;
        }
    }
}
