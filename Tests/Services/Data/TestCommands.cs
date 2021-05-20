using CustomCommandSystem.Common.Attributes;
using CustomCommandSystem.Common.Models;
using GTANetworkAPI;
using System;

#pragma warning disable IDE0051 // Remove unused private members
namespace CustomCommandSystem.Tests.Services.Data
{
    internal class TestCommands
    {
        [CustomCommand("Test", 3)]
        [CustomCommandAlias("Test1", "FirstTest")]
        public void TestPublicCommand(Player player)
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
        private void TestPrivateCommand(Player player, int test1, string test2)
        {

        }

        [CustomCommand("Test")]

        private static void TestPrivateStaticCommand(Player player, CustomCommandInfo info, Vector3 vector3, int testInt, string a = "test", int b = 5,
            [CustomCommandRemainingText] string remainingText = "")
        {

        }

        [CustomCommand("Test2")]

        public void Test2Command(Player player)
        {

        }

        [CustomCommand("Test2")]

        public static void Test2StaticCommand(Player player)
        {

        }

        [CustomCommand("Test3false")]
        public static void Test3(Player player, string a, char b, Vector3 c, bool d, float e, int f)
        {

        }

        [CustomCommand("Test3true")]
        public static void Test3(Player player, CustomCommandInfo info, string a, char b, Vector3 c, bool d, float e, int f)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(Player player, int a, char b)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(Player player, int a, char b, string c = "a", int d = 0, Vector3? e = null)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(Player player, int a, char b, [CustomCommandRemainingText] string c)
        {

        }

        [CustomCommand("Test4")]
        public void Test4(Player player, CustomCommandInfo info, int a)
        {

        }

        [CustomCommand("Test4", 10)]
        public static void Test4Static(Player player, int a, [CustomCommandRemainingText] string b)
        {
            Console.Write("Test4Static called " + a + " - " + b);
        }

        [CustomCommand("Test4")]
        public void Test4(int a, string b)
        {

        }

        [CustomCommand("output")]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(Player player, [CustomCommandRemainingText] string message)
        {
            Console.Write($"Output 1 {message}");
        }

        [CustomCommand("outputCancel")]
        [CancelIfTextIsHello]
        public void OutputCancel(Player player, [CustomCommandRemainingText] string message)
        {
            Console.Write($"OutputCancel 1 {message}");
        }

        [CustomCommand("output", 1)]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(Player player, int number, [CustomCommandRemainingText] string message)
        {
            Console.Write($"Output 2 {number} {message}");
        }

        [CustomCommand("output", 2)]
        [CustomCommandAlias("console", "ConsoleOutput")]
        public void Output(Player player, OutputTestModel model)
        {
            Console.Write($"Output 3 {model.Id} {model.AnyString}");
        }

        public class OutputTestModel
        {
            public int Id { get; set; }
            public string? AnyString { get; set; }
        }

    }
}
#pragma warning restore IDE0051 // Remove unused private members
