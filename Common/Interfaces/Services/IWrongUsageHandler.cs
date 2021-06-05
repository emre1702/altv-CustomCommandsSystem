using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System.Collections.Generic;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface IWrongUsageHandler
    {
        bool Handle(Player player, UserInputData userInputData, List<CommandMethodData> commandMethods, List<CommandMethodData> filteredMethods);
    }
}