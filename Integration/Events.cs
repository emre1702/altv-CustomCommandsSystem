using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Common.Models;
using GTANetworkAPI;

namespace CustomCommandSystem.Integration
{
    /// <summary>
    /// Contains the events of the CustomCommandSystem.
    /// </summary>
    public static class Events
    {
        /// <summary>
        ///     Before a command gets executed.<br/>
        ///     You can use this to e.g. to cancel events.
        /// </summary>
        public static event BeforeCommandExecuteDelegate? BeforeCommandExecute;

        /// <summary>
        ///     After a command got executed.<br/>
        ///     You can use this to e.g. to log the command usages.
        /// </summary>
        public static event AfterCommandExecuteDelegate? AfterCommandExecute;

        internal static void OnBeforeCommandExecute(Player player, UserInputData userInputData, object?[] args, CancelEventArgs cancelEventArgs)
            => BeforeCommandExecute?.Invoke(player, userInputData, args, cancelEventArgs);

        internal static void OnAfterCommandExecute(Player player, UserInputData userInputData, object?[] args)
            => AfterCommandExecute?.Invoke(player, userInputData, args);
    }
}

