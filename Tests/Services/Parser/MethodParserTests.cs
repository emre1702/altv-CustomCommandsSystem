using CustomCommandSystem.Common.Delegates;
using CustomCommandSystem.Services.Loader;
using CustomCommandSystem.Services.Parser;
using CustomCommandSystem.Services.Utils;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace CustomCommandSystem.Tests.Services.Parser
{
    class MethodParserTests
    {
        [Test]
        public void GetPossibleMethods_ReturnsCorrectAmountOfMethods()
        {
            var fastMethodInvoker = new FastMethodInvoker();
            var logger = new ConsoleLogger();
            var argumentsConverter = new ArgumentsConverter();
            var methodsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter);
            var methodParser = new MethodParser(methodsLoader);
            var args = new string[] { "1", "2" };

            methodsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var possibleMethods = methodParser.GetPossibleMethods("TeSt4", args).ToList();

            Assert.AreEqual(4, possibleMethods.Count);
        }
    }
}
