using GTANetworkAPI;

namespace CustomCommandSystem.Common.Interfaces.Services
{
    internal interface ICommandsHandler
    {
        void ExecuteCommand(Player player, string message);
    }
}