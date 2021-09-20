namespace CustomCommandsSystem.Tests.Services.Loader.Data
{
    internal class ClassWithConstructorParameter
    {
        internal TestParameter Parameter { get; }

        public ClassWithConstructorParameter(TestParameter parameter)
        {
            Parameter = parameter;
        }

        public void TestMethod() { }
    }
}
