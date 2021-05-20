using CustomCommandSystem.Common.Extensions;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Services.Utils;

namespace CustomCommandSystem.Services.Cleaner
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
