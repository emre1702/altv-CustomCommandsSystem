namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandParser
    {
        (string Command, string RemainingMessage) Parse(string message);
    }
}