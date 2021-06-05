using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CustomCommandSystem.Services
{
    internal class CommandsHandler : ICommandsHandler
    {
        public static CommandsHandler? Instance { get; private set; }

        private readonly ICommandMessageCleaner _cleaner;
        private readonly ICommandParser _commandParser;
        private readonly ICommandArgumentsParser _argumentsParser;
        private readonly ICommandMethodExecuter _methodExecuter;
        private readonly ICommandMethodsParser _methodParser;
        private readonly ICommandsConfiguration _configuration;
        private readonly ICommandsLoader _commandsLoader;
        private readonly IWrongUsageHandler _wrongUsageHandler;

        public CommandsHandler(ICommandMessageCleaner cleaner, ICommandParser commandParser, ICommandArgumentsParser argumentsParser, ICommandsLoader commandsLoader,
             ICommandMethodExecuter methodExecuter, ICommandMethodsParser methodParser, ICommandsConfiguration configuration, IWrongUsageHandler wrongUsageHandler)
        {
            _cleaner = cleaner;
            _commandParser = commandParser;
            _argumentsParser = argumentsParser;
            _methodExecuter = methodExecuter;
            _methodParser = methodParser;
            _configuration = configuration;
            _commandsLoader = commandsLoader;
            _wrongUsageHandler = wrongUsageHandler;

            Instance = this;
        }

        public async void ExecuteCommand(Player player, string message)
        {
            message = _cleaner.Clean(message);
            var (command, remainingMessage) = _commandParser.Parse(message);
            var usedParameters = _argumentsParser.ParseUserArguments(remainingMessage);
            var userInputData = new UserInputData(command, message, usedParameters);

            if (!TryGetCommandData(player, userInputData, out var commandData))
                return;
            if (!TryGetPossibleMethods(player, userInputData, usedParameters, commandData, out var possibleMethods))
                return;

            await _methodExecuter.TryExecuteSuitable(player, userInputData, commandData, possibleMethods);
        }

        private bool TryGetCommandData(Player player, UserInputData userInputData, [NotNullWhen(true)] out CommandData? commandData)
        {
            commandData = _commandsLoader.GetCommandData(userInputData.Command);

            if (commandData is null && _configuration.CommandDoesNotExistError is { } text)
                _configuration.MessageOutputHandler.Invoke(new CommandOutputData(player, text, userInputData));

            return !(commandData is null);
        }

        private bool TryGetPossibleMethods(Player player, UserInputData userInputData, string[] usedParameters, CommandData commandData, [NotNullWhen(true)] out List<CommandMethodData>? possibleMethods)
        {
            if (commandData is null)
            {
                possibleMethods = null;
                if (_configuration.CommandDoesNotExistError is { } text)
                    _configuration.MessageOutputHandler.Invoke(new CommandOutputData(player, text, userInputData));
                return false;
            }
            
            possibleMethods = _methodParser.GetPossibleMethods(userInputData.Command, usedParameters, commandData).ToList();
            if (!possibleMethods.Any())
            {
                var wrongUsageOutputted = _wrongUsageHandler.Handle(player, userInputData, commandData.Methods, possibleMethods);
                if (!wrongUsageOutputted)
                {
                    if (_configuration.CommandUsedIncorrectly is { } text)
                        _configuration.MessageOutputHandler.Invoke(new CommandOutputData(player, text, userInputData));
                    else if (_configuration.CommandDoesNotExistError is { } text2)
                        _configuration.MessageOutputHandler.Invoke(new CommandOutputData(player, text2, userInputData));
                }
                    
                return false;
            }
            return true;
        }
    }
}
