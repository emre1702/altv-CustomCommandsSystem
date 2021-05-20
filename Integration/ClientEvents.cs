using CustomCommandSystem.Services;
using GTANetworkAPI;
using CustomCommandSystem.Common.Extensions;

namespace CustomCommandSystem.Integration
{
    internal class ClientEvents
    {
        public ClientEvents()
        {
            NAPI.ClientEvent.Register<Player, string>("CustomCommandSystem:Call", this, ExecuteCommand);
        }

        private void ExecuteCommand(Player player, string command)
            => CommandsHandler.Instance?.ExecuteCommand(player, command);
    }
}
