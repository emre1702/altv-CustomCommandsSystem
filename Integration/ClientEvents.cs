using CustomCommandsSystem.Services;
using AltV.Net.Elements.Entities;
using AltV.Net;
using AltV.Net.Elements.Args;
using System;
using CustomCommandsSystem.Services.Utils;

namespace CustomCommandsSystem.Integration
{
    internal class ClientEvents
    {
        public ClientEvents()
        {
            Alt.OnClient<IPlayer, string>("chat:message", OnChatMessage, OnChatMessageParser);
        }

        private void OnChatMessage(IPlayer player, string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            var config = CommandsConfiguration.LazyInstance.Value;
            if (!message.StartsWith(config.CommandPrefix)) return;

            CommandsHandler.Instance?.ExecuteCommand(player, message);
        }

        private static void OnChatMessageParser(IPlayer player, MValueConst[] mValueArray, Action<IPlayer, string> action)
        {
            if (mValueArray.Length != 1) return;
            var arg = mValueArray[0];
            if (arg.type != MValueConst.Type.String) return;
            action(player, arg.GetString());
        }
    }
}
