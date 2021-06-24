using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Models;
using System.Collections.Generic;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandArgumentsParser
    {
        string[] ParseUserArguments(string remainingMessageWithoutCmd);
        IAsyncEnumerable<object?> ParseInvokeArguments(IPlayer player, CommandMethodData commandMethodData, UserInputData userInputData);
    }
}
