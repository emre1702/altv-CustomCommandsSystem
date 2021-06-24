using CustomCommandsSystem.Common.Models;
using AltV.Net.Elements.Entities;
using System.Collections.Generic;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface IWrongUsageHandler
    {
        bool Handle(IPlayer player, UserInputData userInputData, List<CommandMethodData> commandMethods, List<CommandMethodData> filteredMethods);
    }
}