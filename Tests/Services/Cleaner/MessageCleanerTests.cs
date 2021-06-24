using CustomCommandsSystem.Services.Cleaner;
using CustomCommandsSystem.Services.Utils;
using NUnit.Framework;

namespace CustomCommandsSystem.Tests.Services.Cleaner
{
    class MessageCleanerTests
    {
        [Test]
        [TestCase("kick Person1 Test 123", "/kick Person1 Test 123  ")]
        [TestCase("kick Person2 Yes yes yes", "   /kick Person2     Yes yes     yes  ")]
        public void Clean_DoesWork(string expected, string message)
        {
            var commandMessageCleaner = new MessageCleaner(new CommandsConfiguration());
            message = commandMessageCleaner.Clean(message);

            Assert.AreEqual(expected, message);
        }
    }
}
