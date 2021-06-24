using CustomCommandsSystem.Common.Extensions;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Services.Utils;

namespace CustomCommandsSystem.Services.Cleaner
{
    internal class MessageCleaner : ICommandMessageCleaner
    {
        private readonly ICommandsConfiguration _configuration;

        public MessageCleaner(ICommandsConfiguration configuration)
            => _configuration = configuration;

        public string Clean(string message)
        {
            message = message
                .Trim()
                .RemoveDuplicateSpaces();

            if (_configuration.CommandPrefix.Length > 0 && message.StartsWith(_configuration.CommandPrefix))
                message = message[_configuration.CommandPrefix.Length..];

            return message;
        }
    }
}
