using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using CustomCommandsSystem.Common.Attributes;
using CustomCommandsSystem.Common.Models;
using System;

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0060 // Remove unused parameter
namespace CustomCommandsSystem.Tests.Services.Data
{
    internal class TestCommands
    {
        [CustomCommand("Test", 3)]
        [CustomCommandAlias("Test1", "FirstTest")]
        public void TestPublicCommand(IPlayer player)
        {

        }

        [CustomCommand("Test", -5)]
        [CustomCommandAlias("Test1")]
        [CustomCommandAlias("FirstTest")]
        public static void TestPublicStaticCommand(CustomCommandInfo info)
        {

        }

        [CustomCommand("Test", 9551)]
        [CustomCommandAlias("Test1", "FirstTest")]
        private void TestPrivateCommand(IPlayer player, int test1, string test2)
        {

        }

        [CustomCommand("Test")]

        private static void TestPrivateStaticCommand(IPlayer player, CustomCommandInfo info, Position vector3, int testInt, string a = "test", int b = 5, string? c = null,
            [CustomCommandRemainingText] string remainingText = "")
        {

        }

        [CustomCommand("Test2")]

        public void Test2Command(IPlayer player)
        {

        }

        [CustomCommand("Test2")]

        public static void Test2StaticCommand(IPlayer player)
        {

        }

        [CustomCommand("Test3false")]
        public static void Test3(IPlayer player, string a, char b, Position c, bool d, float e, int f)
        {

        }

        [CustomCommand("Test3true")]
        public static void Test3(IPlayer player, CustomCommandInfo info, string a, char b, Position c, bool d, float e, int f)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(IPlayer player, int a, char b)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(IPlayer player, int a, char b, string c = "a", int d = 0, Position? e = null)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(IPlayer player, int a, char b, [CustomCommandRemainingText] string c)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(IPlayer player, CustomCommandInfo info, int a)
        {

        }

        [CustomCommand("Test4", 10)]
        public static void Test4Static(IPlayer player, int a, [CustomCommandRemainingText] string b)
        {
            Console.Write("Test4Static called " + a + " - " + b);
        }

        [CustomCommand("Test4")]
        public void Test4(int a, string b)
        {

        }

        [CustomCommand("output")]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(IPlayer player, [CustomCommandRemainingText] string message)
        {
            Console.Write($"Output 1 {message}");
        }

        [CustomCommand("outputCancel")]
        [CancelIfTextIsHello]
        public void OutputCancel(IPlayer player, [CustomCommandRemainingText] string message)
        {
            Console.Write($"OutputCancel 1 {message}");
        }

        [CustomCommand("output", 1)]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(IPlayer player, int number, [CustomCommandRemainingText] string message)
        {
            Console.Write($"Output 2 {number} {message}");
        }

        [CustomCommand("output", 2)]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(IPlayer player, OutputTestModel model)
        {
            Console.Write($"Output 3 {model.Id} {model.AnyString}");
        }

        [CustomCommand("output", 1)]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output()
        {
            Console.Write("Output empty called");
        }

        public class OutputTestModel
        {
            public int Id { get; set; }
            public string? AnyString { get; set; }
        }

    }
}
#pragma warning restore IDE0051 // Remove unused private members
#pragma warning restore IDE0060 // Remove unused parameter
