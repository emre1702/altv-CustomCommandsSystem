using CustomCommandSystem.Services.Parser;
using CustomCommandSystem.Tests.Services.Parser.Data;
using GTANetworkAPI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCommandSystem.Tests.Services.Parser
{
    class ArgumentsConverterTests
    {
        [Test]
        public async Task Convert_WithBasicTypesWorks()
        {
            var converter = new ArgumentsConverter();
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
                var (convertedValue, lengthUsed) = await converter.Convert(values, i, valuesAndTypes[i].Item3);
                Assert.AreEqual(valuesAndTypes[i].Item2, convertedValue);
                Assert.AreEqual(1, lengthUsed);
                i += lengthUsed;
            }   
        }

        [Test]
        public async Task Convert_WithLongTypesWorks()
        {
            var converter = new ArgumentsConverter();
            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("false", false, typeof(bool), 1),
                ("255.31 0.5 254", new Vector3(255.31, 0.5, 254), typeof(Vector3), 3),
                ("255 0 0", new Color(255, 0, 0), typeof(Color), 3)

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed) = await converter.Convert(values, i, valuesTypesAndLengths[i].Item3);
                if (convertedValue is Vector3 convertedVector3)
                {
                    var vector3 = (Vector3)valuesTypesAndLengths[i].Item2;
                    Assert.AreEqual(vector3.X, convertedVector3.X, 0.01);
                    Assert.AreEqual(vector3.Y, convertedVector3.Y, 0.01);
                    Assert.AreEqual(vector3.Z, convertedVector3.Z, 0.01);
                } 
                else
                    Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);

                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }

        [Test]
        public async Task Convert_WithCustomTypesWorks()
        {
            var converter = new ArgumentsConverter();
            converter.SetConverter(typeof(TestClassForArgumentsConverter), 2, 
                args => new TestClassForArgumentsConverter(args[0]) { B = int.Parse(args[1]) });

            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("hallo 12", new TestClassForArgumentsConverter("hallo") { B = 12 }, typeof(TestClassForArgumentsConverter), 2),
                ("false", false, typeof(bool), 1),

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed) = await converter.Convert(values, i, valuesTypesAndLengths[i].Item3);
                Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);
                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }

        [Test]
        public async Task Convert_WithCustomTypesWorksAsync()
        {
            var converter = new ArgumentsConverter();
            converter.SetAsyncConverter(typeof(TestClassForArgumentsConverter), 2,
                async args => { await Task.Yield(); return new TestClassForArgumentsConverter(args[0]) { B = int.Parse(args[1]) }; });

            var valuesTypesAndLengths = new List<(string, object, Type, int)>
            {
                ("true", true, typeof(bool), 1),
                ("hallo 12", new TestClassForArgumentsConverter("hallo") { B = 12 }, typeof(TestClassForArgumentsConverter), 2),
                ("false", false, typeof(bool), 1),

            };
            var values = valuesTypesAndLengths.SelectMany(e => e.Item1.Split(' ')).ToArray();

            for (int i = 0; i < valuesTypesAndLengths.Count;)
            {
                var (convertedValue, lengthUsed) = await converter.Convert(values, i, valuesTypesAndLengths[i].Item3);
                Assert.AreEqual(valuesTypesAndLengths[i].Item2, convertedValue);
                Assert.AreEqual(valuesTypesAndLengths[i].Item4, lengthUsed);
                i += lengthUsed;
            }
        }
    }
}
