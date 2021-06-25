using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Exceptions;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomCommandsSystem.Services.Parser
{
    internal class ArgumentsParser : ICommandArgumentsParser
    {
        private readonly ICommandArgumentsConverter _argumentsConverter;

        public ArgumentsParser(ICommandArgumentsConverter argumentsConverter)
            => _argumentsConverter = argumentsConverter;

        public string[] ParseUserArguments(string remainingMessageWithoutCmd)
            => string.IsNullOrEmpty(remainingMessageWithoutCmd) ? Array.Empty<string>() : remainingMessageWithoutCmd.Split(' ');

        public async IAsyncEnumerable<object?> ParseInvokeArguments(IPlayer player, CommandMethodData commandMethodData, UserInputData userInputData)
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
                if (userArgIndex >= userInputData.Arguments.Length)
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
                    yield return string.Join(' ', new ArraySegment<string>(userInputData.Arguments, userArgIndex, userInputData.Arguments.Length - userArgIndex));
                    userArgIndex = userInputData.Arguments.Length;
                }
                else
                {
                    var (convertedArg, amountArgsUsed, allowNull) = await _argumentsConverter.Convert(player, userInputData, userArgIndex, methodParameter.Type, cmdErrorOnNullCancel);
                    if (!IsConvertedArgResultAllowed(convertedArg, methodParameter, allowNull))
                        ThrowError(cmdErrorOnNullCancel);
                    cmdErrorOnNullCancel.Cancel = false;
                    yield return convertedArg;
                    userArgIndex += amountArgsUsed;
                }
            }
        }

        private bool IsConvertedArgResultAllowed(object? convertedArg, CommandParameterData methodParameter, bool? allowNull)
        {
            if (convertedArg is not null) return true;
            if (allowNull is null) return methodParameter.IsNullable;
            return allowNull == true;
        }

        private void ThrowError(CancelEventArgs cmdErrorOnNullCancel)
        {
            if (cmdErrorOnNullCancel.Cancel)
                throw new ExpectionWithoutOutput();
            else 
                throw new Exception();
        }
    }
}
