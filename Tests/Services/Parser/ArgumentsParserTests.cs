using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Tests.Services.Parser
{
    class ArgumentsParserTests
    {
#nullable disable
        private ArgumentsParser _argumentsParser;
#nullable enable

        [SetUp]
        public void SetUp()
        {
            var argumentsConverter = Substitute.For<ICommandArgumentsConverter>();
            _argumentsParser = new ArgumentsParser(argumentsConverter);
        }

        [Test]
        public void ParseUserArguments_ReturnsCorrectArguments()
        {
            var expectedArgs = new List<string> { "hello", "test", "1", "2", "3", "123", "true" };
            var userArguments = _argumentsParser.ParseUserArguments(string.Join(' ', expectedArgs));

            for (var i = 0; i < expectedArgs.Count; ++i)
                Assert.AreEqual(expectedArgs[i], userArguments[i]);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task ParseInvokeArguments_ReturnsCorrectArguments(bool withCommandInfos)
        {
            var amountDefaultArgs = withCommandInfos ? 2 : 1;
            var player = Substitute.For<IPlayer>();
            var fastMethodInvoker = new FastMethodInvoker();
            var logger = new ConsoleLogger();
            var argumentsConverter = new ArgumentsConverter(new CommandsConfiguration(), logger);
            var config = new CommandsConfiguration();
            var methodsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter, config);
            methodsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var commandMethodData = (methodsLoader as ICommandsLoader).GetCommandData("Test3" + withCommandInfos.ToString())!.Methods.First();
            var userArgs = new string[] { "hello", "a", "1", "2.23", "123", "true", "12.412", "423" };
            
            var argumentsParser = new ArgumentsParser(argumentsConverter);
            var userInputData = new UserInputData("Test3", "Test3 " + string.Join(' ', userArgs), userArgs);
            var invokeArgs = await argumentsParser.ParseInvokeArguments(player, commandMethodData, userInputData).ToListAsync();

            Assert.AreEqual(player, invokeArgs[0]);
            if (withCommandInfos)
                Assert.IsInstanceOf(typeof(CustomCommandInfo), invokeArgs[1]);
            Assert.AreEqual("hello", invokeArgs[0 + amountDefaultArgs]);
            Assert.AreEqual('a', invokeArgs[1 + amountDefaultArgs]);
            var position = (Position)invokeArgs[2 + amountDefaultArgs]!;
            Assert.AreEqual(1, position.X, 0.001);
            Assert.AreEqual(2.23, position.Y, 0.001);
            Assert.AreEqual(123, position.Z, 0.001);
            Assert.AreEqual(true, invokeArgs[3 + amountDefaultArgs]);
            Assert.AreEqual(12.412, (float)invokeArgs[4 + amountDefaultArgs]!, 0.001);
            Assert.AreEqual(423, invokeArgs[5 + amountDefaultArgs]);
        }
    }
}
