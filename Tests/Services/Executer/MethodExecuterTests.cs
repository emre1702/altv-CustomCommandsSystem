using CustomCommandSystem.Common.Extensions;
using CustomCommandSystem.Common.Interfaces.Services;
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
        private MethodParser _methodParser;
        private MethodExecuter _methodExecuter;
        #nullable restore

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var fastMethodInvoker = new FastMethodInvoker();
            var logger = Substitute.For<ILogger>();
            var argumentsConverter = new ArgumentsConverter();
            var methodsLoader = new CommandsLoader(fastMethodInvoker, logger, argumentsConverter);
            methodsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            _methodParser = new MethodParser(methodsLoader);
            var argumentsParser = new ArgumentsParser(argumentsConverter);
            _methodExecuter = new MethodExecuter(argumentsParser, new CommandsConfiguration());
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
            var methods = _methodParser.GetPossibleMethods("Test4", arguments);

            var result = await _methodExecuter.TryExecuteSuitable(player, "Test4", methods.ToList(), arguments);

            Assert.IsTrue(result);
            Assert.AreEqual("Test4Static called 5 - hallo 123 123 123", _stringWriter.ToString());

            ConsoleUtils.ResetOut();
        }

        [Test]
        [TestCase("output", "Hello", ExpectedResult = "Output 1 Hello")]
        [TestCase("outputCancel", "Hello", ExpectedResult = "")]
        [TestCase("outputCancel", "NotHello", ExpectedResult = "OutputCancel 1 NotHello")]
        public async Task<string> TryExecuteSuitable_ChecksRequirementAttribute(string cmd, string arg)
        {
            var player = new Player(new NetHandle());
            var args = new string[] { arg };
            var methods = _methodParser.GetPossibleMethods(cmd, args);

            var result = await _methodExecuter.TryExecuteSuitable(player, cmd, methods.ToList(), args);
           
            Assert.IsTrue(result);
            
            return _stringWriter.ToString();
        }
    }
}
