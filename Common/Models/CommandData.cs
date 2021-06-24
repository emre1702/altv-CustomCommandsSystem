using System.Collections.Generic;

namespace CustomCommandsSystem.Common.Models
{
    internal class CommandData
    {
        public string Command { get; }
        public List<CommandMethodData> Methods { get; } = new List<CommandMethodData>();

        public CommandData(string cmd)
        {
            Command = cmd;
        }
    }
}
