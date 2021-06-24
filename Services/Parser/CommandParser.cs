using CustomCommandsSystem.Common.Interfaces.Services;

namespace CustomCommandsSystem.Services.Parser
{
    internal class CommandParser : ICommandParser
    {
        public (string Command, string RemainingMessage) Parse(string message)
        {
            var commandEndIndex = message.IndexOf(' ');
            var cmd = commandEndIndex == -1 ? message : message[0..commandEndIndex];
            return (cmd, message[cmd.Length..].Trim());
        }
    }
}
