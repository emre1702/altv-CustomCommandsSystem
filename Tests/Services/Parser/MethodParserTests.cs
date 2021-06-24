using CustomCommandsSystem.Common.Delegates;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace CustomCommandsSystem.Tests.Services.Parser
{
    class MethodParserTests
    {
        [Test]
        public void GetPossibleMethods_ReturnsCorrectAmountOfMethods()
        {
            var fastMethodInvoker = new FastMethodInvoker();
            var logger = new ConsoleLogger();
            var config = new CommandsConfiguration();
            var argumentsConverter = new ArgumentsConverter(config);
            ICommandsLoader commandsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter);
            var methodsParser = new MethodsParser();
            var args = new string[] { "1", "2" };

            commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var possibleMethods = methodsParser.GetPossibleMethods("TeSt4", args, commandsLoader.GetCommandData("TeSt4")!).ToList();

            Assert.AreEqual(4, possibleMethods.Count);
        }
    }
}
