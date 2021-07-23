using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Interfaces.Services;
using CustomCommandsSystem.Common.Models;
using CustomCommandsSystem.Services.Parser;
using CustomCommandsSystem.Services.Utils;
using CustomCommandsSystem.Tests.Services.Parser.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandsSystem.Tests.Services.Parser
{
    class ArgumentsConverterTests
    {
        #nullable disable
        private ArgumentsConverter _argumentsConverter;
        private IPlayer _player;
        #nullable restore

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new CommandsConfiguration();
            var logger = Substitute.For<ILogger>();
            _argumentsConverter = new ArgumentsConverter(configuration, logger);
            _player = Substitute.For<IPlayer>();
        }
        

        [Test]
        public async Task Convert_WithBasicTypesWorks()
        {
            var valuesAndTypes = new List<(string, object, Type)> 
            { 
                ("true", true, typeof(bool)),
                ("false", false, typeof(bool)),
                ("yes", true, typeof(bool)),
                ("no", false, typeof(bool)),
                ("0", (byte)0, typeof(byte)), 
                ("1", (sbyte)1, typeof(sbyte)), 
                ("a", 'a', typeof(char)),
                ("5.4", (decimal)5.4, typeof(decimal)), 
                ("5.4", (double)5.4, typeof(double)),
                ("5.4", 5.4f, typeof(float)),
                ("5", 5, typeof(int)),
                ("5", (uint)5, typeof(uint)),
                ("5", 5L, typeof(long)),
                ("5", (ulong)5, typeof(ulong)),
                ("5", (short)5, typeof(short)),
                ("5", (ushort)5, typeof(ushort)),
                ("string", "string", typeof(string)),
            };
            var values = valuesAndTypes.Select(e => e.Item1).ToArray();

            for (int i = 0; i < valuesAndTypes.Count;)
            {
                var (convertedValue, lengthUsed, allowNull) = await _argumentsConverter.Convert(_player, new("", "", values), i, valuesAndTypes[i].Item3, new());
                Assert.AreEqual(valuesAndTypes[i].Item2, convertedValue);
                Assert.AreEqual(1, lengthUsed);
                i += lengthUsed;
            }   
        }

        [Test]
        public async Task Convert_WithLongTypesWorks()
        {
            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("false", false, typeof(bool), 1),
                ("255.31 0.5 254", new Position(255.31f, 0.5f, 254), typeof(Position), 3),
                ("255 0 0", new Rgba(255, 0, 0, 5), typeof(Rgba), 3)

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed, allowNull) = await _argumentsConverter.Convert(_player, new UserInputData("", "", values), i, valuesTypesAndLengths[i].Item3, new CancelEventArgs());
                Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);
                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }

        [Test]
        public async Task Convert_WithCustomTypesWorks()
        {
            _argumentsConverter.SetConverter(typeof(TestClassForArgumentsConverter), 2, 
                (player, userInputData, args, cancel) => new TestClassForArgumentsConverter(args[0]) { B = int.Parse(args[1]) });

            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("hallo 12", new TestClassForArgumentsConverter("hallo") { B = 12 }, typeof(TestClassForArgumentsConverter), 2),
                ("false", false, typeof(bool), 1),

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed, allowNull) = await _argumentsConverter.Convert(_player, new UserInputData("", "", values), i, valuesTypesAndLengths[i].Item3, new CancelEventArgs());
                Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);
                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }

        [Test]
        public async Task Convert_WithCustomTypesWorksAsync()
        {
            _argumentsConverter.SetAsyncConverter(typeof(TestClassForArgumentsConverter), 2,
                async (player, userInputData, args, cancel) => { await Task.Yield(); return new TestClassForArgumentsConverter(args[0]) { B = int.Parse(args[1]) }; });

            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("hallo 12", new TestClassForArgumentsConverter("hallo") { B = 12 }, typeof(TestClassForArgumentsConverter), 2),
                ("false", false, typeof(bool), 1),

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed, allowNull) = await _argumentsConverter.Convert(_player, new UserInputData("", "", values), i, valuesTypesAndLengths[i].Item3, new CancelEventArgs());
                Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);
                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }
    }
}
