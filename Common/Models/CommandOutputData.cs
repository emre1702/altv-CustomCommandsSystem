using AltV.Net.Elements.Entities;
namespace CustomCommandsSystem.Common.Models
{
    public class CommandOutputData
    {
        public IPlayer Player { get; }
        public string MessageToOutput { get; }
        public UserInputData UserInputData { get; }

        internal CommandOutputData(IPlayer player, string messageToOutput, UserInputData userInputData)
        {
            Player = player;
            MessageToOutput = messageToOutput;
            UserInputData = userInputData;
        }
    }
}
