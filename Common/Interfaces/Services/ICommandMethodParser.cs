using CustomCommandSystem.Common.Models;
using System.Collections.Generic;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandMethodParser
    {
        IEnumerable<CommandMethodData>? GetPossibleMethods(string cmd, string[] userArgs);
    }
}