using AltV.Net.Resources.Chat.Api;
using CustomCommandsSystem.Common.Enums;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Common.Utils;
using System;

namespace CustomCommandsSystem.Services.Utils
{
    internal class CommandsConfiguration : ICommandsConfiguration
    {
        internal static Lazy<CommandsConfiguration> LazyInstance { get; } = new Lazy<CommandsConfiguration>();

        public string CommandPrefix { get; set; } = "/";
        public string? CommandDoesNotExistError { get; set; } = "This command does not exist.";
        public string? CommandUsedIncorrectly { get; set; } = "The command was used incorrectly.";
        public string? PlayerNotFoundErrorMessage { get; set; } = "A player with that name could not be found.";

        public bool RunCommandMethodInMainThread { get; set; } = true;
        public string NullDefaultValueName { get; set; } = "-";
        public UsageOutputType UsageOutputType { get; set; } = UsageOutputType.OneUsage;
        public string SingleUsageOutputPrefix { get; set; } = "USAGE: ";
        public string MultipleUsagesOutputPrefix { get; set; } = "USAGES:";
        public bool UsageAddDefaultValues { get; set; } = true;
        public Action<CommandOutputData> MessageOutputHandler { get; set; } = (data) =>
        {
            var messages = data.MessageToOutput.Split(Environment.NewLine);
            AltAsyncUtils.DoConsideringTest(() =>
            {
                foreach (var line in messages)
                    data.Player.SendChatMessage(line);
            });
        };

        public IServiceProvider? ServiceProviderForInstances { get; set; }
    }
}
