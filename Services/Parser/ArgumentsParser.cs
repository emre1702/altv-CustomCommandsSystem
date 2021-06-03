using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace CustomCommandSystem.Services.Parser
{
    internal class ArgumentsParser : ICommandArgumentsParser
    {
        private readonly ICommandArgumentsConverter _argumentsConverter;

        public ArgumentsParser(ICommandArgumentsConverter argumentsConverter)
            => _argumentsConverter = argumentsConverter;

        public string[] ParseUserArguments(string remainingMessageWithoutCmd)
            => string.IsNullOrEmpty(remainingMessageWithoutCmd) ? new string[0] : remainingMessageWithoutCmd.Split(' ');

        public async IAsyncEnumerable<object?> ParseInvokeArguments(Player player, CommandMethodData commandMethodData, string[] userArgs)
        {
            if (commandMethodData.IsPlayerRequired)
                yield return player;
            if (commandMethodData.IsCommandInfoRequired)
                yield return new CustomCommandInfo();

            var cmdErrorOnNullCancel = new CancelEventArgs();
            var methodParameters = commandMethodData.UserParameters;
            for (int methodParamIndex = 0, userArgIndex = 0; methodParamIndex < methodParameters.Count; ++methodParamIndex)
            {
                var methodParameter = methodParameters[methodParamIndex];
                if (userArgIndex >= userArgs.Length)
                {
                    if (methodParameter.HasDefaultValue)
                    {
                        yield return methodParameter.DefaultValue;
                        ++userArgIndex;
                    }
                    else ThrowError(cmdErrorOnNullCancel);
                }
                else if (methodParameter.IsRemainingText)
                {
                    yield return string.Join(' ', new ArraySegment<string>(userArgs, userArgIndex, userArgs.Length - userArgIndex));
                    userArgIndex = userArgs.Length;
                }
                else
                {
                    var (convertedArg, amountArgsUsed) = await _argumentsConverter.Convert(player, userArgs, userArgIndex, methodParameter.Type, cmdErrorOnNullCancel);
                    if (convertedArg is null && !methodParameter.IsNullable)
                        ThrowError(cmdErrorOnNullCancel);
                    cmdErrorOnNullCancel.Cancel = false;
                    yield return convertedArg;
                    userArgIndex += amountArgsUsed;
                }
            }
        }

        private void ThrowError(CancelEventArgs cmdErrorOnNullCancel)
        {
            if (cmdErrorOnNullCancel.Cancel)
                throw new ArgumentException();
            else 
                throw new Exception();
        }
    }
}
