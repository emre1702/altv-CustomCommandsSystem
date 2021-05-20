using CustomCommandSystem.Common;
using CustomCommandSystem.Common.Datas;
using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandSystem.Services.Executer
{
    internal class MethodExecuter : ICommandMethodExecuter
    {
        internal static BeforeCommandExecuteDelegate? BeforeCommandExecute { get; set; }
        internal static AfterCommandExecuteDelegate? AfterCommandExecute { get; set; }

        private readonly ICommandArgumentsParser _argumentsParser;
        private readonly ICommandsConfiguration _configuration;

        public MethodExecuter(ICommandArgumentsParser argumentsParser, ICommandsConfiguration configuration)
            => (_argumentsParser, _configuration) = (argumentsParser, configuration);

        public async Task<bool> TryExecuteSuitable(Player player, string cmd, List<CommandMethodData> possibleMethods, string[] userArguments)
        {
            var suitableMethodInfo = await GetSuitableMethodInfo(player, possibleMethods, userArguments);
            if (suitableMethodInfo.MethodData is null)
            {
                if (suitableMethodInfo.OutputCommandUsedWrongIfNoneFound && _configuration.CommandUsedIncorrectly is { })
                    NAPI.Task.Run(() => player.SendChatMessage(_configuration.CommandUsedIncorrectly));
                else if (_configuration.CommandDoesNotExistError is { })
                    NAPI.Task.Run(() => player.SendChatMessage(_configuration.CommandDoesNotExistError));
                return false;
            }

            var args = suitableMethodInfo.ConvertedArgs ?? Array.Empty<object>();

            if (!AreCustomRequirementsMet(suitableMethodInfo.MethodData, player, args)) return true;

            var cancel = new CancelEventArgs();
            BeforeCommandExecute?.Invoke(player, cmd, args, cancel);
            if (cancel.Cancel) return true;

            if (_configuration.RunCommandMethodInMainThread)
                RunSync(suitableMethodInfo.MethodData, args);
            else 
                Run(suitableMethodInfo.MethodData, args);
           

            AfterCommandExecute?.Invoke(player, cmd, args);
            return true;
        }

        private async Task<SuitableMethodInfo> GetSuitableMethodInfo(Player player, List<CommandMethodData> possibleMethods, string[] userArguments)
        {
            var outputCommandUsedWrongIfNoneFound = false;
            foreach (var possibleMethod in possibleMethods)
            {
                try
                {
                    var convertedArgs = await _argumentsParser.ParseInvokeArguments(player, possibleMethod, userArguments).ToArrayAsync();
                    if (convertedArgs != null)
                        return new SuitableMethodInfo
                        {
                            ConvertedArgs = convertedArgs,
                            OutputCommandUsedWrongIfNoneFound = false,
                            MethodData = possibleMethod
                        };
                }
                catch (ArgumentException) { }
                catch (Exception)
                {
                    outputCommandUsedWrongIfNoneFound = true;
                }
            }

            return new SuitableMethodInfo { OutputCommandUsedWrongIfNoneFound = outputCommandUsedWrongIfNoneFound };
        }

        private bool AreCustomRequirementsMet(CommandMethodData methodData, Player player, object?[] methodArgs)
        {
            if (methodData.RequirementCheckers.Length == 0) return true;

            CustomCommandInfo? customCommandInfo = methodData.IsCommandInfoRequired ? (CustomCommandInfo)methodArgs[methodData.UserParametersStartIndex - 1]! : null;
            var args = new ArraySegment<object?>(methodArgs, methodData.UserParametersStartIndex, methodArgs.Length - methodData.UserParametersStartIndex);
            foreach (var checker in methodData.RequirementCheckers)
                if (!checker.CanExecute(player, customCommandInfo, args))
                    return false;
            return true;
        }

        private void RunSync(CommandMethodData methodData, object?[] args)
        {
            NAPI.Task.Run(() =>
            {
                if (methodData.FastInvokeHandler is FastInvokeHandler nonStaticHandler)
                    nonStaticHandler.Invoke(methodData.Instance, args);
                else if (methodData.FastInvokeHandler is FastInvokeHandlerStatic staticHandler)
                    staticHandler.Invoke(args);
            });
        }

        private void Run(CommandMethodData methodData, object?[] args)
        {
            if (methodData.FastInvokeHandler is FastInvokeHandler nonStaticHandler)
                nonStaticHandler.Invoke(methodData.Instance, args);
            else if (methodData.FastInvokeHandler is FastInvokeHandlerStatic staticHandler)
                staticHandler.Invoke(args);
        }
    }
}