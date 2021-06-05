using CustomCommandSystem.Common.Enums;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomCommandSystem.Services.Parser
{
    internal class WrongUsageHandler : IWrongUsageHandler
    {
        private readonly ICommandsConfiguration _configuration;

        public WrongUsageHandler(ICommandsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Handle(Player player, UserInputData userInputData, List<CommandMethodData> commandMethods, List<CommandMethodData> filteredMethods)
        {
            switch (_configuration.UsageOutputType)
            {
                case UsageOutputType.OneUsage:
                    return OutputOneUsage(player, userInputData, commandMethods);
                case UsageOutputType.AllUsages:
                    if (commandMethods.Count <= 1)
                        return OutputOneUsage(player, userInputData, commandMethods);
                    else
                        return OutputMultipleUsages(player, userInputData, commandMethods);

                case UsageOutputType.OneUsageOnWrongTypes:
                    return OutputOneUsage(player, userInputData, filteredMethods);
                case UsageOutputType.AllUsagesOnWrongTypes:
                    if (filteredMethods.Count <= 1)
                        return OutputOneUsage(player, userInputData, filteredMethods);
                    else
                        return OutputMultipleUsages(player, userInputData, filteredMethods);
            }
            return false;
        }

        private bool OutputOneUsage(Player player, UserInputData userInputData, List<CommandMethodData> commandMethods)
        {
            var methodData = commandMethods.FirstOrDefault();
            if (methodData is null) return false;

            var strBuilder = new StringBuilder(_configuration.SingleUsageOutputPrefix);
            AddUsageText(strBuilder, userInputData.Command, methodData);

            var messageOutputData = new CommandOutputData(player, strBuilder.ToString(), userInputData);
            _configuration.MessageOutputHandler.Invoke(messageOutputData);
            return true;
        }

        private bool OutputMultipleUsages(Player player, UserInputData userInputData, List<CommandMethodData> commandMethods)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(_configuration.MultipleUsagesOutputPrefix);

            foreach (var methodData in commandMethods)
            {
                strBuilder.AppendLine();
                AddUsageText(strBuilder, userInputData.Command, methodData);
            }


            var messageOutputData = new CommandOutputData(player, strBuilder.ToString(), userInputData);
            _configuration.MessageOutputHandler.Invoke(messageOutputData);
            return true;
        }


        private void AddUsageText(StringBuilder strBuilder, string cmd, CommandMethodData methodData)
        {
            strBuilder.Append(_configuration.CommandPrefix + cmd);
            foreach (var parameter in methodData.UserParameters)
            {
                strBuilder.Append($" [{parameter.Name}");
                if (_configuration.UsageAddDefaultValues && parameter.HasDefaultValue)
                    strBuilder.Append($" = {(parameter.DefaultValue?.ToString() ?? _configuration.NullDefaultValueName)}");
                strBuilder.Append("]");
            }
        }
    }
}
