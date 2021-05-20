using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomCommandSystem.Services.Parser
{
    internal class MethodParser : ICommandMethodParser
    {
        private readonly ICommandsLoader _methodsLoader;

        public MethodParser(ICommandsLoader methodsLoader)
            => _methodsLoader = methodsLoader;

        public IEnumerable<CommandMethodData>? GetPossibleMethods(string cmd, string[] userArgs)
        {
            var commandData = _methodsLoader.GetCommandData(cmd);
            if (commandData is null) return null;

            var methods = FilterByArgsAmount(commandData.Methods, userArgs.Length);
            return methods;
        }

        private IEnumerable<CommandMethodData> FilterByArgsAmount(IEnumerable<CommandMethodData> methods, int argsAmount)
            => methods.Where(m => m.ExactUserArgsAmount.HasValue
                ? m.ExactUserArgsAmount.Value == argsAmount
                : m.MinUserArgsAmount <= argsAmount && m.MaxUserArgsAmount >= argsAmount);
    }
}
