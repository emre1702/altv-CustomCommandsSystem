using CustomCommandSystem.Common.Datas;
using System;

namespace CustomCommandSystem.Services.Utils
{
    internal class CommandsConfiguration : ICommandsConfiguration
    {
        internal static Lazy<CommandsConfiguration> LazyInstance { get; } = new Lazy<CommandsConfiguration>();

        public string CommandPrefix { get; set; } = "/";
        public string? CommandDoesNotExistError { get; set; } = "This command does not exist.";
        public string? CommandUsedIncorrectly { get; set; } = "The command was used incorrectly.";
        public string? CommandWithTheseArgsDoesNotExistError { get; set; } = "A command with these arguments does not exist.";
        public string? PlayerNotFoundErrorMessage
        {
            get => _playerNotFoundErrorMessage;
            set
            {
                _playerNotFoundErrorMessage = value;
                DefaultConverters.PlayerNotFoundErrorMessage = value;
            }
        }
        public bool RunCommandMethodInMainThread { get; set; } = true;

        private string? _playerNotFoundErrorMessage = "A player with that name could not be found.";

    }
}
