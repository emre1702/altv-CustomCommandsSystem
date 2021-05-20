using CustomCommandSystem.Services.Parser;
using NUnit.Framework;

namespace CustomCommandSystem.Tests.Services.Parser
{
    class CommandParserTests
    {
        [Test]
        [TestCase("kick", "Player1 hallo", "kick Player1 hallo")]
        [TestCase("BanSad", "Player2 test 123 132", "BanSad Player2 test 123 132")]
        public void CommandParser_Works(string expectedCmd, string expectedRemainingMessage, string message)
        {
            var commandParser = new CommandParser();

            var (actualCmd, actualRemainingMessage) = commandParser.Parse(message);

            Assert.AreEqual(expectedCmd, actualCmd);
            Assert.AreEqual(expectedRemainingMessage, actualRemainingMessage);
        }
    }
}
