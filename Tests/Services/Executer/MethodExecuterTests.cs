using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Common.Utils;
using CustomCommandsSystem.Services.Executer;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Tests.Services.Executer
{
    class MethodExecuterTests
    {
#nullable disable
        private MethodsParser _methodParser;
        private MethodExecuter _methodExecuter;
        private ICommandsLoader _commandsLoader;
#nullable restore

        [SetUp]
        public void SetUp()
        {
            AltAsyncUtils.InTest = true;
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

        [Test]
        public async Task TryExecuteSuitable_ExecutesCorrectMethod()
        {
            var player = Substitute.For<IPlayer>();
            var arguments = new string[] { "5", "hallo", "123", "123", "123" };
            var commandData = _commandsLoader.GetCommandData("Test4")!;
            var methods = _methodParser.GetPossibleMethods("Test4", arguments, commandData);
            var message = string.Empty;

            var userInputData = new UserInputData("Test4", "Test4 " + string.Join(' ', arguments), arguments);
            var result = await _methodExecuter.TryExecuteSuitable(player, userInputData, commandData, methods.ToList());

            Assert.IsTrue(result);
            player.Received().SetData("Test4Static called 5 - hallo 123 123 123", Arg.Any<object>());
        }

        [Test]
        [TestCase("output", "Hello", ExpectedResult = "Output 1 Hello")]
        [TestCase("outputCancel", "Hello", ExpectedResult = "")]
        [TestCase("outputCancel", "NotHello", ExpectedResult = "OutputCancel 1 NotHello")]
        [TestCase("Output", "", ExpectedResult = "Output empty called")]
        public async Task<string> TryExecuteSuitable_ChecksRequirementAttribute(string cmd, string arg)
        {
            var player = Substitute.For<IPlayer>();
            var args = arg.Length > 0 ? new string[] { arg } : Array.Empty<string>();
            string message = string.Empty;
            player.When(p => p.SetData(Arg.Any<string>(), Arg.Any<object>())).Do(x => message = x[0].ToString()!);

            var commandData = _commandsLoader.GetCommandData(cmd)!;
            var methods = _methodParser.GetPossibleMethods(cmd, args, commandData);

            var userInputData = new UserInputData(cmd, "Test4 " + string.Join(' ', args), args);
            var result = await _methodExecuter.TryExecuteSuitable(player, userInputData, commandData, methods.ToList());

            Assert.IsTrue(result);

            return message;
        }

        [Test]
        public void GetSuitableMethodInfo_ExceptionIsCatched()
        {
            var commandData = _commandsLoader.GetCommandData("Test3false")!;
            var methods = _methodParser.GetPossibleMethods("Test3false", new string[] { "a", "b", "0", "1", "2", "true", "5", "6" }, commandData);

            var config = new CommandsConfiguration { RunCommandMethodInMainThread = false };
            var argumentsParser = Substitute.For<ICommandArgumentsParser>();
            argumentsParser.ParseInvokeArguments(Arg.Any<IPlayer>(), Arg.Any<CommandMethodData>(), Arg.Any<UserInputData>())
#pragma warning disable CS0162 // Unreachable code detected
                .ReturnsForAnyArgs((x) => { throw new Exception(); return null; });
#pragma warning restore CS0162 // Unreachable code detected
            var wrongUsageHandler = new WrongUsageHandler(config);
            _methodExecuter = new MethodExecuter(argumentsParser, config, wrongUsageHandler);

            Assert.DoesNotThrowAsync(async () => await _methodExecuter.GetSuitableMethodInfo(null!, new()
            {
                new(methods.First().Method, null!, 0)
            }, new("Test3false", "Test3false hello", new string[] { "hello" })));
        }
    }
}
