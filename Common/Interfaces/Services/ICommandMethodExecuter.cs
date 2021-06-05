using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandMethodExecuter
    {
        Task<bool> TryExecuteSuitable(Player player, UserInputData userInputData, CommandData commandData, List<CommandMethodData> possibleMethods);
    }
}