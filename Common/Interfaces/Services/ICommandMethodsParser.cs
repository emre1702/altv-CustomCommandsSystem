using CustomCommandsSystem.Common.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandMethodsParser
    {
        IEnumerable<CommandMethodData> GetPossibleMethods(string cmd, string[] userArgs, CommandData commandData);
    }
}