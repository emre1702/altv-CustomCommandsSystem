using CustomCommandSystem.Common.Extensions;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Models;
using CustomCommandSystem.Services.Executer;
using CustomCommandSystem.Services.Loader;
using CustomCommandSystem.Services.Parser;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandSystem.Tests.Services.Executer
{
    class MethodExecuterTests
    {
        #nullable disable
        private StringWriter _stringWriter;
        private MethodsParser _methodParser;
        private MethodExecuter _methodExecuter;
        private ICommandsLoader _commandsLoader;
#nullable restore

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            NapiTaskExtensions.InTest = true;

            var config = new CommandsConfiguration { RunCommandMethodInMainThread = false };
            var fastMethodInvoker = new FastMethodInvoker();
            var logger = Substitute.For<ILogger>();
            var argumentsConverter = new ArgumentsConverter(config);
            _commandsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter);
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            _methodParser = new MethodsParser();
            var argumentsParser = new ArgumentsParser(argumentsConverter);
            var wrongUsageHandler = new WrongUsageHandler(config);
            _methodExecuter = new MethodExecuter(argumentsParser, config, wrongUsageHandler);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _stringWriter?.Dispose();
            ConsoleUtils.ResetOut();
        }

        [SetUp]
        public void CreateNewStringWriter()
        {
            _stringWriter = new StringWriter();
            Console.SetOut(_stringWriter);
        }

        [Test]
        public async Task TryExecuteSuitable_ExecutesCorrectMethod()
        {
            var player = new Player(new NetHandle());
            var arguments = new string[] { "5", "hallo", "123", "123", "123" };
            var commandData = _commandsLoader.GetCommandData("Test4")!;
            var methods = _methodParser.GetPossibleMethods("Test4", arguments, commandData);

            var userInputData = new UserInputData("Test4", "Test4 " + string.Join(' ', arguments), arguments);
            var result = await _methodExecuter.TryExecuteSuitable(player, userInputData, commandData, methods.ToList());

            Assert.IsTrue(result);
            Assert.AreEqual("Test4Static called 5 - hallo 123 123 123", _stringWriter.ToString());

            ConsoleUtils.ResetOut();
        }

        [Test]
        [TestCase("output", "Hello", ExpectedResult = "Output 1 Hello")]
        [TestCase("outputCancel", "Hello", ExpectedResult = "")]
        [TestCase("outputCancel", "NotHello", ExpectedResult = "OutputCancel 1 NotHello")]
        [TestCase("Output", "", ExpectedResult = "Output empty called")]
        public async Task<string> TryExecuteSuitable_ChecksRequirementAttribute(string cmd, string arg)
        {
            var player = new Player(new NetHandle());
            var args = arg.Length > 0 ? new string[] { arg } : new string[0];
            var commandData = _commandsLoader.GetCommandData(cmd)!;
            var methods = _methodParser.GetPossibleMethods(cmd, args, commandData);

            var userInputData = new UserInputData(cmd, "Test4 " + string.Join(' ', args), args);
            var result = await _methodExecuter.TryExecuteSuitable(player, userInputData, commandData, methods.ToList());
           
            Assert.IsTrue(result);
            
            return _stringWriter.ToString();
        }
    }
}
