using CustomCommandsSystem.Services;
using AltV.Net.Elements.Entities;
using AltV.Net;

namespace CustomCommandsSystem.Integration
{
    internal class ClientEvents
    {
        public ClientEvents()
        {
            Alt.OnClient<IPlayer, string>("CustomCommandsSystem:Call", ExecuteCommand);
        }

        private void ExecuteCommand(IPlayer player, string command)
            => CommandsHandler.Instance?.ExecuteCommand(player, command);
    }
}
