using System;
using System.IO;

namespace CustomCommandsSystem.Common.Extensions
{
    internal static class ConsoleUtils
    {
        public static void ResetOut()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(standardOutput);
        }
    }
}
