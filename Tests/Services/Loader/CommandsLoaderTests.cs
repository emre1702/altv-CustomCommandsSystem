using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Services.Loader;
using CustomCommandSystem.Services.Parser;
using CustomCommandSystem.Services.Utils;
using GTANetworkAPI;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace CustomCommandSystem.Tests.Services.Loader
{
    class CommandsLoaderTests
    {
#nullable disable
        private CommandsLoader _commandsLoader;
#nullable restore

        [SetUp]
        public void Setup()
        {
            var fastMethodInvoker = new FastMethodInvoker();
            var consoleLogger = new ConsoleLogger();
            var argumentsConverter = new ArgumentsConverter();
            _commandsLoader = new CommandsLoader(fastMethodInvoker, consoleLogger, argumentsConverter);
        }

        [Test]
        public void LoadCommands_FindsAllMethods()
        {
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var commandMethods = _commandsLoader.GetCommandDatas();

            Assert.AreEqual(11, commandMethods.Keys.Count);
            Assert.AreEqual(17, commandMethods.Values.SelectMany(v => v.Methods).Distinct().Count());
        }

        [Test]
        public void LoadCommands_SortsByPriorityDesc()
        {
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var commandMethods = _commandsLoader.GetCommandDatas();

            foreach (var entry in commandMethods)
            {
                var lastPriority = int.MaxValue;
                foreach (var command in entry.Value.Methods)
                {
                    Assert.GreaterOrEqual(lastPriority, command.Priority);
                    lastPriority = command.Priority;
                }
            }
        }

        [Test]
        public void UnloadCommands_Works()
        {
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            _commandsLoader.UnloadCommands(Assembly.GetExecutingAssembly());
            var commandMethods = _commandsLoader.GetCommandDatas();

            Assert.IsEmpty(commandMethods);
        }

        [Test]
        public void ReloadCommands_Works()
        {
            _commandsLoader.ReloadCommands(Assembly.GetExecutingAssembly());
            var commandMethods = _commandsLoader.GetCommandDatas();

            Assert.IsNotEmpty(commandMethods);
        }

        [Test]
        public void GetAllCommands_ReturnsCorrectMethods()
        {
            var cmd = "Test";

            _commandsLoader.ReloadCommands(Assembly.GetExecutingAssembly());
            var allMethods = _commandsLoader.GetCommandDatas();
            var commandMethods = (_commandsLoader as ICommandsLoader).GetCommandData(cmd);

            Assert.AreEqual(allMethods[cmd], commandMethods);
        }

        [Test]
        public void LoadCommands_AddsNewCommandDatasForAliases()
        {
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var cmdCommandData = ((ICommandsLoader)_commandsLoader).GetCommandData("Test")!;
            var aliasCommandData1 = ((ICommandsLoader)_commandsLoader).GetCommandData("Test1")!;
            var aliasCommandData2 = ((ICommandsLoader)_commandsLoader).GetCommandData("FirstTest")!;

            Assert.AreEqual(cmdCommandData.Methods.Count, aliasCommandData1.Methods.Count + 1);
            Assert.AreEqual(cmdCommandData.Methods.Count, aliasCommandData2.Methods.Count + 1);
            Assert.AreNotEqual(aliasCommandData1, aliasCommandData2);
        }
    }
}
