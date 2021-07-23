using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Enums;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace CustomCommandsSystem.Tests.Services.Utils
{
    class WrongUsageHandlerTests
    {
#nullable disable
        private ICommandsConfiguration _config;
        private WrongUsageHandler _wrongUsageHandler;
        private IPlayer _player;
#nullable restore

        [SetUp]
        public void SetUp()
        {
            _config = new CommandsConfiguration();
            _wrongUsageHandler = new WrongUsageHandler(_config);
            _player = Substitute.For<IPlayer>();
        }

        [Test]
        [TestCase("Test", "asd", false, "null", UsageOutputType.OneUsage, true, ExpectedResult = "USAGE: /Test [test1] [test2]")]
        [TestCase("Test", "asd", false, "null", UsageOutputType.AllUsages, true,
            ExpectedResult = "USAGES:\r\n/Test [test1] [test2]\r\n/Test\r\n/Test [vector3] [testInt] [a] [b] [c] [remainingText]\r\n/Test")]
        [TestCase("Test", "asd", true, "null", UsageOutputType.OneUsage, true, ExpectedResult = "USAGE: /Test [test1] [test2]")]
        [TestCase("Test", "asd", true, "nothing", UsageOutputType.AllUsages, true,
            ExpectedResult = "USAGES:\r\n/Test [test1] [test2]\r\n/Test\r\n/Test [vector3] [testInt] [a = test] [b = 5] [c = nothing] [remainingText = ]\r\n/Test")]
        [TestCase("Test", "asd", false, "null", UsageOutputType.OneUsageOnWrongTypes, false, ExpectedResult = "")]
        [TestCase("Test", "asd", false, "null", UsageOutputType.AllUsagesOnWrongTypes, false, ExpectedResult = "")]

        public string Output_Works(string cmd, string remainingInput, bool addDefaultValues, string nullValueName, UsageOutputType usageOutputType, bool shouldWork)
        {
            var logger = Substitute.For<ILogger>();
            var argumentsConverter = new ArgumentsConverter(_config, logger);
            var argumentsParser = new ArgumentsParser(argumentsConverter);
            var userArgs = argumentsParser.ParseUserArguments(remainingInput);
            var commandData = GetCommandData(cmd, argumentsConverter);
            var methodsParser = new MethodsParser();
            var filteredMethods = methodsParser.GetPossibleMethods(cmd, userArgs, commandData).ToList();
            var msg = string.Empty;
            _config.UsageOutputType = usageOutputType;
            _config.UsageAddDefaultValues = addDefaultValues;
            _config.NullDefaultValueName = nullValueName;
            _config.MessageOutputHandler = (data) => msg = data.MessageToOutput;
            var userInputData = new UserInputData(cmd, $"{cmd} {remainingInput}", remainingInput.Split(' '));

            var result = _wrongUsageHandler.Handle(_player, userInputData, commandData!.Methods, filteredMethods);

            Assert.AreEqual(shouldWork, result);
            return msg;
        }

        private CommandData GetCommandData(string cmd, ArgumentsConverter argumentsConverter)
        {
            var fastMethodInvoker = new FastMethodInvoker();
            var consoleLogger = new ConsoleLogger();
            var commandsLoader = new CommandsLoader(fastMethodInvoker, consoleLogger, argumentsConverter);
            commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());

            return ((ICommandsLoader)commandsLoader).GetCommandData(cmd)!;
        }
    }
}
