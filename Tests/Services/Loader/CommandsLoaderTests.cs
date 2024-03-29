﻿using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Services.Loader;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using CustomCommandsSystem.Tests.Services.Loader.Data;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Reflection;

namespace CustomCommandsSystem.Tests.Services.Loader
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
            var configuration = new CommandsConfiguration();
            var argumentsConverter = new ArgumentsConverter(configuration, consoleLogger);
            _commandsLoader = new CommandsLoader(fastMethodInvoker, consoleLogger, argumentsConverter, configuration);
        }

        [Test]
        public void LoadCommands_FindsAllMethods()
        {
            _commandsLoader.LoadCommands(Assembly.GetExecutingAssembly());
            var commandMethods = _commandsLoader.GetCommandDatas();

            Assert.AreEqual(11, commandMethods.Keys.Count);
            Assert.AreEqual(19, commandMethods.Values.SelectMany(v => v.Methods).Distinct().Count());
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

        [Test]
        public void LoadCommands_CanCreateInstance()
        {
            var methodInfo = typeof(ClassWithoutConstructorParameter).GetMethod("TestMethod")!;
            var instance = _commandsLoader.GetMethodInstance(methodInfo, null) as ClassWithoutConstructorParameter;
            Assert.IsNotNull(instance);
            Assert.DoesNotThrow(() => instance!.TestMethod());
        }

        [Test]
        public void LoadCommands_CanCreateInstanceWithDependencyInjection()
        {
            var methodInfoWithoutParameter = typeof(ClassWithoutConstructorParameter).GetMethod("TestMethod")!;
            var instanceWithoutParameter = _commandsLoader.GetMethodInstance(methodInfoWithoutParameter, null) as ClassWithoutConstructorParameter;

            var testParameterData = "Test123";
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ClassWithConstructorParameter>();
            serviceCollection.AddSingleton(new TestParameter() { Data = testParameterData });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var methodInfoWithParameter = typeof(ClassWithConstructorParameter).GetMethod("TestMethod")!;
            var instanceWithParameter = _commandsLoader.GetMethodInstance(methodInfoWithParameter, serviceProvider) as ClassWithConstructorParameter;
           
            Assert.IsNotNull(instanceWithoutParameter, "Can't create instance without parameter.");
            Assert.DoesNotThrow(() => instanceWithoutParameter!.TestMethod());

            Assert.IsNotNull(instanceWithParameter, "Can't create instance with parameter.");
            Assert.DoesNotThrow(() => instanceWithParameter!.TestMethod());
            Assert.IsNotNull(instanceWithParameter!.Parameter);
            Assert.AreEqual(instanceWithParameter!.Parameter!.Data!, testParameterData);
        }
    }
}
