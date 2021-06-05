using GTANetworkAPI;

namespace CustomCommandSystem.Common.Models
{
    public class CommandOutputData
    {
        public Player Player { get; }
        public string MessageToOutput { get; }
        public UserInputData UserInputData { get; }

        internal CommandOutputData(Player player, string messageToOutput, UserInputData userInputData)
        {
            Player = player;
            MessageToOutput = messageToOutput;
            UserInputData = userInputData;
        }
    }
}
