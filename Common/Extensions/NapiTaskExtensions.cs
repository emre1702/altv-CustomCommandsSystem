using System;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Extensions
{
    internal static class NapiTaskExtensions
    {
        internal static bool InTest { get; set; }

        public static async Task RunWait(this GTANetworkMethods.Task task, Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            if (InTest)
            {
                action();
                return;
            }

            task.Run(() =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });

            await taskCompletionSource.Task.ConfigureAwait(false);
        }

        public static Task<T> RunWait<T>(this GTANetworkMethods.Task task, Func<T> func)
        {
            var taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (InTest)
                return Task.FromResult(func());

            task.Run(() =>
            {
                var result = func();
                taskCompletionSource.SetResult(result);
            });

            return taskCompletionSource.Task;
        }
    }
}
