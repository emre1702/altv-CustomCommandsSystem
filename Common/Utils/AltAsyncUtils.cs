using AltV.Net.Async;
using System;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Common.Utils
{
    internal static class AltAsyncUtils
    {
        internal static bool InTest { private get; set; }

        public static Task DoConsideringTest(Action action)
        {
            if (InTest)
            {
                action();
                return Task.CompletedTask;
            }

            return AltAsync.Do(action);
        }

        public static Task<T> DoConsideringTest<T>(Func<T> func)
        {
            if (InTest) return Task.FromResult(func());

            return AltAsync.Do(func);
        }
    }
}
