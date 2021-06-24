using AltV.Net.Elements.Entities;

namespace CustomCommandsSystem.Common.Interfaces.Services
{
    internal interface ICommandsHandler
    {
        void ExecuteCommand(IPlayer player, string message);
    }
}