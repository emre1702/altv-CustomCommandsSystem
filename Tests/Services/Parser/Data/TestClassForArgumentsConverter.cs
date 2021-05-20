using System;
using System.Diagnostics.CodeAnalysis;

namespace CustomCommandSystem.Tests.Services.Parser.Data
{
    internal class TestClassForArgumentsConverter : IEquatable<TestClassForArgumentsConverter>
    {
        public string A { get; set; }
        public int B { get; set; }

        public TestClassForArgumentsConverter(string a)
            => A = a;

        public bool Equals([AllowNull] TestClassForArgumentsConverter other)
            => A == other?.A && B == other?.B;
    }
}
