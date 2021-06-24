using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandMethodExecuter
    {
        Task<bool> TryExecuteSuitable(IPlayer player, UserInputData userInputData, CommandData commandData, List<CommandMethodData> possibleMethods);
    }
}