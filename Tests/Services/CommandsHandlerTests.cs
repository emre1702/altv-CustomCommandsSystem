using CustomCommandSystem.Common.Extensions;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Services;
using CustomCommandSystem.Services.Cleaner;
using CustomCommandSystem.Services.Executer;
using CustomCommandSystem.Services.Loader;
using CustomCommandSystem.Services.Parser;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using static CustomCommandSystem.Tests.Services.Data.TestCommands;

namespace CustomCommandSystem.Tests.Services
{
    class CommandsHandlerTests
    {
#nullable disable
        private StringWriter _stringWriter;
        private Player _player;
        private CommandsHandler _commandsHandler;

#nullable enable

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new CommandsConfiguration();
            _player = new Player(new NetHandle());
            var cleaner = new MessageCleaner(configuration);
            var commandParser = new CommandParser();
            var argumentsConverter = new ArgumentsConverter();
            var argumentsParser = new ArgumentsParser(argumentsConverter);
            var logger = Substitute.For<ILogger>();
            var fastMethodInvoker = new FastMethodInvoker();
            var commandsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter);
            var methodExecuter = new MethodExecuter(argumentsParser, configuration);
            var methodParser = new MethodParser(commandsLoader);
            _commandsHandler = new CommandsHandler(cleaner, commandParser, argumentsParser, commandsLoader, methodExecuter, methodParser, configuration);

            commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            argumentsConverter.SetConverter(typeof(OutputTestModel), 2, args => new OutputTestModel { Id = int.Parse(args[0]), AnyString = args[1] });
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _stringWriter.Dispose();
            ConsoleUtils.ResetOut();
        }

        [SetUp]
        public void SetUp()
        {
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [Test]
        [TestCase("/output test test test test", ExpectedResult = "Output 1 test test test test")]
        [TestCase("/console 5 test test test", ExpectedResult = "Output 2 5 test test test")]
        [TestCase("/ConsoleOutput 5 test", ExpectedResult = "Output 3 5 test")]
        public string ExecuteCommand_TestCommandsWork(string cmd)
        {
            _commandsHandler.ExecuteCommand(_player, cmd);

            return _stringWriter.ToString()!;
        }
    }
}
