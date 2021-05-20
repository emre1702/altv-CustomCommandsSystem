using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System.Collections.Generic;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandArgumentsParser
    {
        string[] ParseUserArguments(string remainingMessageWithoutCmd);
        IAsyncEnumerable<object> ParseInvokeArguments(Player player, CommandMethodData commandMethodData, string[] userArgs);
    }
}
