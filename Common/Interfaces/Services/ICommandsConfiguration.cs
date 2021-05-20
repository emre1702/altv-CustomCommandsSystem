namespace CustomCommandSystem.Services.Utils
{
    public interface ICommandsConfiguration
    {
        /// <summary>
        ///     Error if the command does not exist.<br/>
        ///     Set <see langword="null"/> if you want to disable it.
        /// </summary>
        string? CommandDoesNotExistError { get; set; }

        /// <summary>
        ///     Command prefix used.<br/>
        ///     Only really needed if you have a custom chat, trigger the remote event or call the method by yourself and don't want to have to remove the prefix by yourself.
        /// </summary>
        string CommandPrefix { get; set; }

        /// <summary>
        ///     Error if the command has been used incorrectly.<br/>
        ///     Set <see langword="null"/> if you want to disable it.
        /// </summary>
        string? CommandUsedIncorrectly { get; set; }

        /// <summary>
        ///     Error if the command exist, but not with these arguments. <br/>
        ///     Set <see langword="null"/> if you want to disable it.
        /// </summary>
        string? CommandWithTheseArgsDoesNotExistError { get; set; }

        /// <summary>
        ///     Set this to false if you don't want the command method to be executed in NAPI.Task.Run (in main thread).
        ///     Has propably no effect if the command method is async.
        /// </summary>
        bool RunCommandMethodInMainThread { get; set; }
    }
}