using System;
using System.IO;

namespace CustomCommandSystem.Common.Extensions
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
